
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
    /// Creates a layer from the name, position and a copy of image and adds it to the layer manager. 
    /// </summary>
    /// <param name="name">The name of the layer</param>
    /// <param name="position">The position of the image relative to the final image</param>
    /// <param name="image">The image this layer will contain a copy of</param>
    /// <param name="flags">Extra data about the layer</param>
    public void addLayer(string name, Point position, Bitmap image, int flags = 0)
    {
        if (m_layerCurrent == m_maxLayers)
            return;

        CanvasFragment fragment = new CanvasFragment(
                image,
                position.X,
                position.Y);

        m_layers[m_layerCount] = new Layer(
            m_layerCount, 
            name,
            fragment,
            flags);

        m_layerCurrent = m_layerCount;
        m_layerCount++;

        updateTotalSize();
    }

    /// <summary>
    /// Removes all layers
    /// </summary>
    public void clearAllLayers()
    {
        for (int i = 0; i < m_layerCount; i++)
        {
            m_layers[i].clearLayer();
        }
        m_layerCount = 0;
    }

    /// <summary>
    /// Applies the drawbuffer image to the current layer.
    /// </summary>
    /// <param name="drawBuffer">The draw buffer image</param>
    /// <param name="updateRegion">The region that needs to be redrawn.</param>
    /// <param name="mode">The blendmode used to apply the image.</param>
    public void applyDrawBuffer(Bitmap drawBuffer, Rectangle updateRegion, BlendMode mode)
    {
        Layer layer = m_layers[m_layerCurrent];

        // create region relative to the image to update
        Rectangle imageRegion = layer.getRectangle();

        // return if there's nothing to update
        if (!updateRegion.IntersectsWith(imageRegion))
            return;

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
            m.DrawImage(drawBuffer,
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
    }
    
    /// <summary>
    /// Flattens drawbuffer to current layer and applies the changes to cached image.
    /// </summary>
    /// <param name="displaySize">The size of the display region in the application</param>
    /// <param name="updateRegion">The region within the final image that will be changed</param>
    /// <param name="cachedImage">The previous final image</param>
    /// <returns></returns>
    public Bitmap flattenImage(Size displaySize, Rectangle updateRegion, int startPos, int endPos, Bitmap cachedImage)
    {
        diagnostics.restartTimer();

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

            // create region relative to the image to update
            Rectangle imageRegion = layer.getRectangle();

            // get a handle on the layers image
            Bitmap layerImage = layer.getImage();

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
    public void deleteLayer(int position)
    {
        if (m_layerCount <= 0)
            return;

        // shift following layers to the left
        int end = m_layerCount - 1;
        for (int i = position; i < end; i++)
        {
            m_layers[i] = m_layers[i + 1];
            m_layers[i].setId(i);
        }

        // clear last layer
        m_layers[m_layerCount].clearLayer();
        m_layerCount--;

        updateTotalSize();
    }

    /// <summary>
    /// Sets the specified layer as the active layer
    /// </summary>
    /// <param name="position">The id of the layer to be set as active</param>
    public void setActiveLayer(int position)
    {
        m_layerCurrent = position;
    }

    /// <summary>
    /// Returns the current visibility of the specified layer and sets its visibility to the opposite.
    /// </summary>
    /// <param name="position">The id of the layer to toggle</param>
    /// <returns></returns>
    public bool toggleLayerIsVisible(int position)
    {
        if (position > m_layerCount)
            return false;

        bool iv = m_layers[position].getIsVisible();
        iv = !iv;
        
        setLayerIsVisible(position, iv);

        return iv;
    }

    /// <summary>
    /// Sets the specified layer to the specified visiblilty.
    /// </summary>
    /// <param name="position">The id of the layer to set visibility</param>
    /// <param name="isVisible">The value of visiblity to be set</param>
    public void setLayerIsVisible(int position, bool isVisible)
    {
        if (position > m_layerCount)
            return;

        m_layers[position].setIsVisible(isVisible);
    }

    /// <summary>
    /// Returns true if the specified layer is visible.
    /// </summary>
    /// <param name="position">The id of the layer to check</param>
    /// <returns></returns>
    public bool getLayerIsVisible(int position)
    {
        if (position > m_layerCount)
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
    /// </summary>
    /// <param name="position">The id of the layer to get the region of</param>
    /// <returns></returns>
    public Rectangle getLayerRegion(int position)
    {
        if (position > m_layerCount)
            return new Rectangle(0, 0, 0, 0);

        return m_layers[position].getRectangle();
    }

    /// <summary>
    /// Compiles the positions and sizes of all layers to calculate 
    /// the size of an image made from all layers then stores that value.
    /// </summary>
    private void updateTotalSize()
    {
        Point start = new Point(int.MaxValue, int.MaxValue);
        Point end = new Point(0, 0);
        for (int i = 0; i < m_layerCount; i++)
        {
            Rectangle r = m_layers[i].getRectangle();

            if (r.X < start.X)
                start.X = r.X;
            if (r.Y < start.Y)
                start.Y = r.Y;

            if (r.X + r.Width > end.X)
                end.X = r.X + r.Width;
            if (r.Y + r.Height > end.Y)
                end.Y = r.Y + r.Height;
        }

        m_layerTotalSize.Width = start.X + end.X;
        m_layerTotalSize.Height = start.Y + end.Y;
    }

    /// <summary>
    /// Returns the size of an image made from every layer continaed within the manager
    /// </summary>
    /// <returns></returns>
    public Size getTotalSize()
    {
        return m_layerTotalSize;
    }

    /// <summary>
    /// Returns a rectangle that represents the position and size of the current layer
    /// </summary>
    /// <returns></returns>
    public Rectangle getCurrentLayerRegion()
    {
        return m_layers[m_layerCurrent].getRectangle();
    }

    /// <summary>
    ///  Returns a handle to the image contianed within the specified layer.
    /// </summary>
    /// <param name="layerId">The id of the layer to get the image from</param>
    /// <returns></returns>
    public Bitmap getLayerImage(int layerId)
    {
        return m_layers[layerId].getImage();
    }

    /// <summary>
    /// Stores a copy of the image in the specified layer.
    /// </summary>
    /// <param name="layerId"></param>
    /// <param name="image"></param>
    public void setLayerImage(int layerId, Bitmap image)
    {
        m_layers[layerId].setImage(image);
    }
}
