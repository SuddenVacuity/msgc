
using System;
using System.Drawing;
using System.Drawing.Imaging;

using static EnumList;

public class LayerManager
{
    private Diagnostics diagnostics = new Diagnostics("LayerManager", ConsoleColor.Green);
    private int m_layerCount;
    private int m_layerCurrent;
    private int m_maxLayers;
    private Layer[] m_layers;
    // the size of an image that is made off all layers
    private Size m_layerTotalSize;

    public LayerManager(int maxLayers = 255)
    {
        diagnostics.setActive(true);

        m_maxLayers = maxLayers;
        m_layers = new Layer[m_maxLayers];
        m_layerTotalSize = new Size(0, 0);
    }

    /// <summary>
    /// Returns true if layer was sucessfully added. Creates a layer from the name, position and a copy of image and adds it to the layer manager. 
    /// </summary>
    /// <param name="name">The name of the layer</param>
    /// <param name="position">The position of the image relative to the final image</param>
    /// <param name="image">The image this layer will contain a copy of</param>
    /// <param name="flags">Extra data about the layer</param>
    public bool addLayer(string name, Rectangle region, Bitmap image, int flags = 0)
    {
        if (m_layerCurrent == m_maxLayers)
            return false;

        CanvasFragment fragment = null;
        Bitmap img = null;

        // tries to clone even if null
        //if(image != null)
        //    img = (Bitmap)image.Clone();
        try { img = (Bitmap)image.Clone(); }
        catch { img = null; }

        if (img != null)
            fragment = new CanvasFragment(img, region);

        m_layers[m_layerCount] = new Layer(
            m_layerCount,
            name,
            fragment,
            flags);

        m_layerCurrent = m_layerCount;
        m_layerCount++;

        updateTotalSize();
        return true;
    }

    /// <summary>
    /// Removes all layers
    /// </summary>
    public void clearAllLayers()
    {
        for (int i = 0; i < m_layerCount; i++)
        {
            m_layers[i].clearLayer();
            m_layers[i] = null;
        }
        m_layerCount = 0;
    }

    /// <summary>
    /// Overlays the specified layer's image with the input image. 
    /// Returns true on success.
    /// </summary>
    /// <param name="layerId">the id of the layer to apply the image to</param>
    /// <param name="image">The image to apply</param>
    /// <param name="updateRegion">The region that needs to be redrawn.</param>
    /// <param name="mode">The blendmode used to apply the image.</param>
    public bool applyImageOnLayer(int layerId, Bitmap image, Rectangle updateRegion, BlendMode mode)
    {
        Layer layer = m_layers[layerId];

        // check if theres an image to apply to
        if (layer.hasFragment() == false)
            return false;

        // create region relative to the image to update
        Rectangle imageRegion = layer.getLayerRegion();

        // return if there's nothing to update
        if (!updateRegion.IntersectsWith(imageRegion))
            return false;

        // reduce the size of the redraw zone to only include area the layer's image covers
        Rectangle redrawRegion = Rectangle.Intersect(updateRegion, imageRegion);

        // area to redraw relative to the layer image
        // intersect to reduce area to redraw
        Point redrawLocalPosition = redrawRegion.Location;

        // offset by layer image position
        redrawLocalPosition.X -= imageRegion.X;
        redrawLocalPosition.Y -= imageRegion.Y;

        // get the layer's image
        Bitmap layerImage = layer.getImage();

        // image that will be a cropped section of drawbuffer
        Bitmap bufferCrop = new Bitmap(
            redrawRegion.Width,
            redrawRegion.Height,
            PixelFormat.Format32bppArgb);

        // copy the section of the drawbuffer that will be redarwn into the cropbuffer image
        using (Graphics m = Graphics.FromImage(bufferCrop))
        {
            m.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            m.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            m.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            m.DrawImage(image,
                new Rectangle(0, 0, redrawRegion.Width, redrawRegion.Height),
                redrawRegion,
                GraphicsUnit.Pixel);
        }

        //// apply the cropped image to the current layer image
        Bitmap bmp = RasterBlend.mergeImages(
            layerImage,
            bufferCrop,
            redrawLocalPosition,
            redrawRegion.Size,
            mode);

        bufferCrop.Dispose();

        // set the new image as the layer image
        layer.setImage(bmp);
        bmp.Dispose();
        return true;
    }

    /// <summary>
    /// Flattens drawbuffer to current layer and applies the changes to cached image. Runs Dispose() on cachedImage. 
    /// Returns null on fail
    /// </summary>
    /// <param name="displaySize">The size of the display region in the application</param>
    /// <param name="updateRegion">The region within the final image that will be changed</param>
    /// <param name="cachedImage">The previous final image - gets Bitmap.Dispose()</param>
    /// <returns></returns>
    public Bitmap flattenImage(Size displaySize, Rectangle updateRegion, int startPos, int endPos, Bitmap cachedImage)
    {
        diagnostics.restartTimer();

        if (updateRegion.Width <= 0 || updateRegion.Height <= 0)
            return null;
        if (startPos >= endPos)
            return null;

        if (startPos < 0)
            startPos = 0;
        if (endPos > m_layerCount)
            endPos = m_layerCount;

        // copy previous image into the result to reduce work needed
        Bitmap result = cachedImage;

        // the image to stack layers into and apply to the reswult image after stacking
        Bitmap redrawnSection = new Bitmap(updateRegion.Width, updateRegion.Height);

        // remove all data that will be overwritten from flattening
        // this must happen before redrawnSection is written to
        using (Graphics gr = Graphics.FromImage(result))
        {
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            gr.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            gr.DrawImage(redrawnSection,
                updateRegion,
                new Rectangle(new Point(0, 0), redrawnSection.Size),
                GraphicsUnit.Pixel);
        }

        diagnostics.printTimeElapsedAndRestart("PREPARE RESULT IMAGE TIME ");

        // move pixels from drawBuffer to display_canvas
        // flatten all layers
        for (int l = startPos; l < endPos; l++)
        //for (int l = m_layerCount - 1; l >= 0; l--)
        {
            // get a handle on the layer
            Layer layer = m_layers[l];
            // Console.Write("\nLayer " + l + " isVisible = " + layer.getIsVisible());

            // skip layer if is not visible or has not been set
            if (layer.getId() == int.MinValue)
                continue;

            if (layer.getIsVisible() == false)
                continue;

            if (layer.hasFragment() == false)
                continue;

            // create region relative to the image to update
            Rectangle imageRegion = layer.getLayerRegion();

            // get a handle on the layers image
            Bitmap layerImage = layer.getImage();

            if (layerImage == null)
                continue;

            // reduce the size of the redraw zone to only include area the layer's image covers
            Rectangle redrawRegion = Rectangle.Intersect(updateRegion, imageRegion);

            // area to redraw relative to the layer image
            // intersect to reduce area to redraw
            Point redrawLocalPosition = redrawRegion.Location;
            // offset by layer image position
            redrawLocalPosition.X -= imageRegion.X;
            redrawLocalPosition.Y -= imageRegion.Y;

            // go to next layer if there's nothing to update
            if (!updateRegion.IntersectsWith(imageRegion))
                continue;

            // create new image the size of redraw region
            Bitmap layerCrop = new Bitmap(
                redrawRegion.Width,
                redrawRegion.Height,
                PixelFormat.Format32bppArgb);

            // cut section out of layer that will be applied to the image
            using (Graphics gr = Graphics.FromImage(layerCrop))
            {
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gr.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gr.DrawImage(layerImage,
                    new Rectangle(0, 0, redrawRegion.Width, redrawRegion.Height),
                    new Rectangle(redrawLocalPosition, redrawRegion.Size),
                    GraphicsUnit.Pixel);
            }

            // apply cutout to the result image
            Bitmap stack = RasterBlend.mergeImages(
                redrawnSection,
                layerCrop,
                new Point(redrawRegion.X -= updateRegion.X, redrawRegion.Y -= updateRegion.Y),//redrawLocalPosition,
                redrawRegion.Size,
                BlendMode.Add);

            layerCrop.Dispose();

            if (redrawnSection != null)
                redrawnSection.Dispose();

            redrawnSection = stack;

            diagnostics.printTimeElapsedAndRestart("FLATTEN LAYER " + l + " TIME");
        } // END l

        // create final image
        Bitmap final = RasterBlend.mergeImages(
            result,
            redrawnSection,
            updateRegion.Location,
            updateRegion.Size,
            BlendMode.Add);

        // this is now the final version of Bitmap stack
        redrawnSection.Dispose();

        if (result != null)
            result.Dispose();

        // set new image as the result
        result = final;

        diagnostics.printTimeElapsedAndRestart("SET RESULT AND DISPOSE TIME ");
        return result;
    }

    /// <summary>
    /// Removes the specified layer from the manager
    /// </summary>
    /// <param name="position">The id of the layer to be removed</param>
    public bool deleteLayer(int position)
    {
        if (position >= m_layerCount ||
            position < 0)
            return false;

        // shift following layers to the left
        int end = m_layerCount - 1;
        for (int i = position; i < end; i++)
        {
            m_layers[i] = m_layers[i + 1];
            m_layers[i].setId(i);
        }

        // clear last layer
        m_layers[m_layerCount].clearLayer();
        m_layers[m_layerCount] = null;
        m_layerCount--;

        updateTotalSize();
        return true;
    }

    /// <summary>
    /// Sets the specified layer as the active layer. 
    /// Returns true on success.
    /// </summary>
    /// <param name="position">The id of the layer to be set as active</param>
    public bool setActiveLayer(int position)
    {
        if (position >= m_layerCount ||
            position < 0)
            return false;

        m_layerCurrent = position;
        return true;
    }

    /// <summary>
    /// Returns the current visibility of the specified layer and sets its visibility to the opposite.
    /// </summary>
    /// <param name="position">The id of the layer to toggle</param>
    /// <returns></returns>
    public bool toggleLayerIsVisible(int position)
    {
        if (position >= m_layerCount ||
            position < 0)
            return false;

        bool iv = m_layers[position].getIsVisible();
        iv = !iv;

        setLayerIsVisible(position, iv);

        return iv;
    }

    /// <summary>
    /// Sets the specified layer to the specified visiblilty. 
    /// Returns true on success.
    /// </summary>
    /// <param name="position">The id of the layer to set visibility</param>
    /// <param name="isVisible">The value of visiblity to be set</param>
    public bool setLayerIsVisible(int position, bool isVisible)
    {
        if (position >= m_layerCount ||
            position < 0)
            return false;

        m_layers[position].setIsVisible(isVisible);
        return true;
    }

    /// <summary>
    /// Returns true if the specified layer is visible. 
    /// Returns false if the specified layer is not visible OR position is out of range
    /// </summary>
    /// <param name="position">The id of the layer to check</param>
    /// <returns></returns>
    public bool getLayerIsVisible(int position)
    {
        if (position >= m_layerCount ||
            position < 0)
            return false;

        return m_layers[position].getIsVisible();
    }

    /// <summary>
    /// Returns the id of the current layer.
    /// </summary>
    /// <returns></returns>
    public int getActiveLayer()
    {
        return m_layerCurrent;
    }

    /// <summary>
    /// Returns the total nuber of layers contianed within the manager.
    /// </summary>
    /// <returns></returns>
    public int getLayerCount()
    {
        return m_layerCount;
    }

    /// <summary>
    /// Returns a rectangle that represents the poistion and size of a specified layer. 
    /// Returns Rectangle.Empty on fail.
    /// </summary>
    /// <param name="position">The id of the layer to get the region of</param>
    /// <returns></returns>
    public Rectangle getLayerRegion(int position)
    {
        if (position >= m_layerCount ||
            position < 0)
            return Rectangle.Empty;

        return m_layers[position].getLayerRegion();
    }

    /// <summary>
    /// Returns a rectangle that represents the poistion and size of a specified range of layers. 
    /// Returns Rectangle.Empty on fail.
    /// </summary>
    /// <param name="startPos">The id of the layer at the start of the range</param>
    /// <param name="endPos">The id of the layer to that the range ends at</param>
    /// <returns></returns>
    public Rectangle getLayerRegion(int startPos, int endPos)
    {
        if (startPos < 0 || endPos > m_layerCount ||
            startPos >= endPos)
            return Rectangle.Empty;

        Rectangle result = new Rectangle(0, 0, 0, 0);

        for (int i = startPos; i < endPos; i++)
        {
            if (m_layers[i].hasFragment() == false)
                continue;

            Rectangle rec = m_layers[i].getLayerRegion();

            if (rec.X < result.X)
                result.X = rec.X;
            if (rec.Y < result.Y)
                result.Y = rec.Y;
            
            if (rec.X + rec.Width > result.X + result.Width)
                result.Width = (rec.X + rec.Width) - result.X;
            if (rec.Y + rec.Height > result.Y + result.Height)
                result.Height = (rec.Y + rec.Height) - result.Y;
        }

        return result;
    }

    /// <summary>
    /// Compiles the positions and sizes of all layers to calculate 
    /// the size of an image made from all layers then stores that value.
    /// </summary>
    private void updateTotalSize()
    {
        // tracks the lowest x, y point in all the layer's regions
        Point start = new Point(int.MaxValue, int.MaxValue);

        // tracks the highest x, y point in all the layer's regions
        Point end = new Point(0, 0);
        for (int i = 0; i < m_layerCount; i++)
        {
            if (m_layers[i].hasFragment() == false)
                continue;

            Rectangle r = m_layers[i].getLayerRegion();

            // check if lower than the current lowest
            if (r.X < start.X)
                start.X = r.X;
            if (r.Y < start.Y)
                start.Y = r.Y;

            // check if higher than the current highest
            if (r.X + r.Width > end.X)
                end.X = r.X + r.Width;
            if (r.Y + r.Height > end.Y)
                end.Y = r.Y + r.Height;
        }

        // get overall size from the two points
        m_layerTotalSize.Width = end.X - start.X;
        m_layerTotalSize.Height = end.Y - start.Y;
    }

    /// <summary>
    /// Returns the size of an image made from every layer continaed within the manager. 
    /// Returns Size.Empty on fail.
    /// </summary>
    /// <returns></returns>
    public Size getTotalSize()
    {
        return m_layerTotalSize;
    }

    /// <summary>
    /// Returns a rectangle that represents the position and size of the current layer. 
    /// Returns Rectangle.Empty on fail.
    /// </summary>
    /// <returns></returns>
    public Rectangle getCurrentLayerRegion()
    {
        return m_layers[m_layerCurrent].getLayerRegion();
    }

    /// <summary>
    ///  Returns a handle to the image contianed within the specified layer. 
    ///  Returns null on fail.
    /// </summary>
    /// <param name="layerId">The id of the layer to get the image from</param>
    /// <returns></returns>
    public Bitmap getLayerImage(int layerId)
    {
        if (layerId < 0 ||
            layerId >= m_layerCount ||
            m_layers[layerId].hasFragment() == false)
            return null;

        return m_layers[layerId].getImage();
    }

    /// <summary>
    /// Stores a copy of the image in the specified layer. 
    /// Returns true on success.
    /// </summary>
    /// <param name="layerId"></param>
    /// <param name="image"></param>
    public bool setLayerImage(int layerId, Bitmap image)
    {
        if (layerId >= m_layerCount ||
            layerId < 0)
            return false;

        return m_layers[layerId].setImage(image);
    }
    /// <summary>
    /// Stores a copy of the image in the specified layer. 
    /// Returns true on success.
    /// </summary>
    /// <param name="layerId"></param>
    /// <param name="image"></param>
    /// <param name="offset"></param>
    public bool setLayerImage(int layerId, Bitmap image, Point location)
    {
        if (layerId >= m_layerCount ||
            layerId < 0)
            return false;

        if (!m_layers[layerId].setImage(image))
            return false;

        m_layers[layerId].setRegion(new Rectangle(location, image.Size));
        return true;
    }

    public void debugSaveLayerImagesToFile(string message, int startPos, int endPos)
    {
        if (startPos < 0)
            startPos = 0;
        if (endPos > m_layerCount)
            endPos = m_layerCount;
        if (startPos >= endPos)
            return;

        for (int i = startPos; i < endPos; i++)
        {
            if (m_layers[i].hasFragment() == false)
                continue;

            Bitmap bmp = m_layers[i].getImage();

            if (bmp != null)
                bmp.Save("LayerManager_" + message + "_" + i + "_" + m_layers[i].getName() + ".png");
        }
    }

    /// <summary>
    /// Returns the name of the layer. 
    /// Returns null on fail.
    /// </summary>
    /// <returns></returns>
    public string getName(int position)
    {
        if (position >= m_layerCount ||
            position < 0)
            return null;

        return m_layers[position].getName();
    }
}
