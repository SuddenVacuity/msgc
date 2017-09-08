
using System;
using System.Drawing;
using System.Drawing.Imaging;

using static EnumList;

public class LayerManager
{
    private int m_layerCount;
    private int m_layerCurrent;
    private int m_maxLayers = 255;
    private Layer[] m_layers;

    public LayerManager()
    {
        m_layers = new Layer[m_maxLayers];
    }

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
    }

    public void clearAllLayers()
    {
        for (int i = 0; i < m_layerCount; i++)
        {
            m_layers[i].clearLayer();
        }
        m_layerCount = 0;
    }
    
    public Bitmap flattenImage(Size displaySize, Bitmap drawBuffer, BlendMode mode)
    {

        Bitmap result = new Bitmap(displaySize.Width, displaySize.Height, PixelFormat.Format32bppArgb);

        // move pixels from drawBuffer to display_canvas
        // flatten all layers

        for (int l = 0; l < m_layerCount; l++)
        {
            // get a handle on the layer
            Layer layer = m_layers[l];
            // Console.Write("\nLayer " + l + " isVisible = " + layer.getIsVisible());

            // skip layer if is not visible or has not been set
            if (layer.getId() == int.MinValue || layer.getIsVisible() == false)
                continue;

            // get a handle on the layers image
            Bitmap layerImage = layer.getImage();

            // create region relative to the image to update
            Rectangle rec = layer.getRectangle();
            Point imagePos = rec.Location;

            // apply drawbuffer if l is current layer
            if (l == m_layerCurrent)
            {
                Console.Write("\nDrawing on layer " + l);

                Bitmap buffer = new Bitmap(layerImage.Width, layerImage.Height, PixelFormat.Format32bppArgb);

                // cut section out of draw buffer that is the same size and position as the layer image
                using (Graphics m = Graphics.FromImage(buffer))
                {
                    m.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    m.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                    m.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    m.DrawImage(drawBuffer,
                        new Rectangle(0, 0, rec.Width, rec.Height),
                        new Rectangle(imagePos.X, imagePos.Y, buffer.Width, buffer.Height),
                        GraphicsUnit.Pixel);
                }

                // add draw buffer to current layer
                Bitmap bmp = RasterBlend.mergeImages(layerImage, buffer, new Rectangle(0, 0, rec.Width, rec.Height), mode);

                layer.setImage(bmp);
                buffer.Dispose();

                // get a handle on the image again
                layerImage = layer.getImage();
            } // END (l == m_layerCurrent)

            Point layerImagePosition = layer.getImagePosition();
            Bitmap final = RasterBlend.mergeImages(result, layerImage, rec, BlendMode.DrawAdd);
            result = final;
        } // END l

        return result;
    }

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
    }

    public void setActiveLayer(int position)
    {
        m_layerCurrent = position;
    }

    public bool toggleLayerIsVisible(int position)
    {
        if (position > m_layerCount)
            return false;

        bool iv = m_layers[position].getIsVisible();
        iv = !iv;

        m_layers[position].setIsVisible(iv);

        return iv;
    }

    public void setLayerIsVisible(int position, bool isVisible)
    {
        if (position > m_layerCount)
            return;

        m_layers[position].setIsVisible(isVisible);
    }

    public bool getLayerIsVisible(int position)
    {
        if (position > m_layerCount)
            return false;

        return m_layers[position].getIsVisible();
    }

    public int getActiveLayer()
    {
        return m_layerCurrent;
    }

    public int getLayerCount()
    {
        return m_layerCount;
    }

    public Rectangle getLayerRegion(int position)
    {
        if (position > m_layerCount)
            return new Rectangle(0, 0, 0, 0);

        return m_layers[position].getRectangle();
    }

    public Rectangle getCurrentLayerRegion()
    {
        return m_layers[m_layerCurrent].getRectangle();
    }
}
