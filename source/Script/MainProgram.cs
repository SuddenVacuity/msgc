using System;
using System.Drawing;
using System.Drawing.Imaging;

using static EnumList;

public class MainProgram
{
    Diagnostics diagnostics = new Diagnostics();

    // default values
    private BlendMode m_defaultBlendMode = BlendMode.DrawAdd;
    private Color m_defaultBrushColor = Color.FromArgb(255, 255, 255, 255);
    private Color m_defaultColor = Color.Black;
    private Color m_defaultColorAlt = Color.White;
    private string m_defaultCanvasImage = @"//colorTest.png";
    private string m_defaultBrush = @"//standard_round.png";


    ////////////////////
    // layer tools
    ////////////////////
    LayerManager m_layers;
    // the image used to store a flattened image that's shown in display_canvas
    private Bitmap m_canvasImage;
    // the image that will be saved to file if saved
    private Bitmap m_finalImage;

    ////////////////////
    // drawing tools
    ////////////////////
    // the position the mouse was last mousemove update
    private Point m_mousePosPrev;
    private bool m_isDrawing;
    // where drawing input is stored bfore it's flattened to m_finalImage
    private Bitmap m_drawBuffer;
    // region where m_drawBuffer has been drawn on
    private Rectangle m_redrawDrawBufferZone;

    private Color m_color;
    private Color m_colorAlt;
    private Brush m_brush;

    Rectangle m_displayRegion;

    public void init(Rectangle displayImageRegion)
    {
        // create layer manager
        m_layers = new LayerManager();

        m_displayRegion = displayImageRegion;

        // load default base image
        Bitmap baseImage;
        string dir = Environment.CurrentDirectory + m_defaultCanvasImage;
        if (System.IO.File.Exists(dir))
        {
            baseImage = new Bitmap(dir);
        }
        else
        {
            Console.Write("\nDefault image not found: " + dir + "\nCreating new image");
            Bitmap img = new Bitmap(800, 600, PixelFormat.Format32bppArgb);
            using (Graphics gr = Graphics.FromImage(img))
            {
                gr.Clear(Color.White);
            }
            baseImage = img;
        }

        m_canvasImage = baseImage;
        m_finalImage = (Bitmap)m_canvasImage.Clone();

        // load default colors
        m_color = m_defaultColor;
        m_colorAlt = m_defaultColorAlt;

        // load default brush image
        dir = Environment.CurrentDirectory + m_defaultBrush;
        Bitmap brushImage;
        if (System.IO.File.Exists(dir))
        {
            // if file exists load and invert rgb values
            brushImage = new Bitmap(dir);
            for (int i = 0; i < brushImage.Size.Height; i++)
                for (int j = 0; j < brushImage.Size.Width; j++)
                {
                    Color c = brushImage.GetPixel(j, i);
                    Color inverted = Color.FromArgb(
                        c.A,
                        byte.MaxValue - c.R,
                        byte.MaxValue - c.G,
                        byte.MaxValue - c.B);
                    brushImage.SetPixel(j, i, inverted);
                }
        }
        else
        {
            // else use hard coded brush
            Console.Write("\nDefault brush not found: " + dir + "\nCreating new brush");
            Bitmap img = new Bitmap(30, 30, PixelFormat.Format32bppArgb);
            using (Graphics gr = Graphics.FromImage(img))
            {
                gr.Clear(Color.Transparent);
            }
            brushImage = img;
            for (int i = 0; i < brushImage.Size.Height; i++)
                for (int j = 0; j < brushImage.Size.Width; j++)
                {
                    if (i < 3 || j < 3)
                        continue;
                    if (i > brushImage.Height - 3 || j > brushImage.Width - 3)
                        continue;

                    if (i - j == 0)
                    {
                        brushImage.SetPixel(j, i, Color.Black);
                        brushImage.SetPixel(j + 1, i, Color.Black);
                        brushImage.SetPixel(j, i + 1, Color.Black);
                        brushImage.SetPixel(j - 1, i, Color.Black);
                        brushImage.SetPixel(j, i - 1, Color.Black);
                    }
                    if (brushImage.Width - j == i)
                    {
                        brushImage.SetPixel(j, i, Color.Black);
                        brushImage.SetPixel(j + 1, i, Color.Black);
                        brushImage.SetPixel(j, i + 1, Color.Black);
                        brushImage.SetPixel(j - 1, i, Color.Black);
                        brushImage.SetPixel(j, i - 1, Color.Black);
                    }
                }
        }
        m_brush = new Brush(brushImage, m_defaultBlendMode);

        // set default program state
        m_isDrawing = false;

        // create redraw zone
        m_redrawDrawBufferZone = new Rectangle(0, 0, 0, 0);

        // create draw buffer
        m_drawBuffer = new Bitmap(displayImageRegion.Size.Width, displayImageRegion.Size.Height, PixelFormat.Format32bppArgb);

        // load layers
        Bitmap lbmp = new Bitmap(400, 200, PixelFormat.Format32bppArgb); for (int y = 0; y < lbmp.Height; y++) for (int x = 0; x < lbmp.Width; x++) lbmp.SetPixel(x, y, Color.Red);
        m_layers.addLayer("Background", new Point(0, 0), (Bitmap)m_finalImage.Clone(), 0);
        m_layers.addLayer("Layer 2", new Point(175, 100), lbmp, 0);
        m_layers.addLayer("Layer 3", new Point(0, 0), new Bitmap(displayImageRegion.Width, displayImageRegion.Height, PixelFormat.Format32bppArgb), 0);

        if (m_finalImage != null)
            m_finalImage.Dispose();

        m_finalImage = m_layers.flattenImage(displayImageRegion.Size, m_drawBuffer, BlendMode.DrawAdd);

        if (m_canvasImage != null)
            m_canvasImage.Dispose();

        m_canvasImage = (Bitmap)m_finalImage.Clone();
        drawUI();
        // make canves the correct size and create image
    }

    public void onMouseDown(int windowId, Point cursorPosition)
    {
        m_isDrawing = true;

        // create draw buffer
        m_mousePosPrev = cursorPosition;
        Console.Write("\nClick position within draw_canvas: " + m_mousePosPrev.X + ", " + m_mousePosPrev.Y);

        Size brushSize = m_brush.getSize();
        m_redrawDrawBufferZone.X = m_mousePosPrev.X - brushSize.Width;
        m_redrawDrawBufferZone.Y = m_mousePosPrev.Y - brushSize.Height;
        m_redrawDrawBufferZone.Width = brushSize.Width * 2;
        m_redrawDrawBufferZone.Height = brushSize.Height * 2;

        drawToBuffer(m_mousePosPrev);

        Bitmap final = new Bitmap(m_displayRegion.Width, m_displayRegion.Height, PixelFormat.Format32bppArgb);

        using (Graphics gr = Graphics.FromImage(final))
        {
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            gr.DrawImage(m_canvasImage,
                new Rectangle(0, 0, m_canvasImage.Width, m_canvasImage.Height),
                new Rectangle(0, 0, m_canvasImage.Width, m_canvasImage.Height),
                GraphicsUnit.Pixel);
            gr.DrawImage(m_drawBuffer,
                m_redrawDrawBufferZone,
                m_redrawDrawBufferZone,
                GraphicsUnit.Pixel);
            gr.Save();
        }

        drawUI();
    }

    public void onMouseMove(int windowId, Point cursorPosition)
    {
        //return;
        if (!m_isDrawing)
            return;

        // current mouse position
        Point mousePos = cursorPosition;

        drawToBuffer(mousePos);

        Size brushSize = m_brush.getSize();
        Point cursorCenter = new Point(
            mousePos.X - brushSize.Width / 2,
            mousePos.Y - brushSize.Height / 2);

        if (m_canvasImage != null)
            m_canvasImage.Dispose();

        m_canvasImage = (Bitmap)m_finalImage.Clone();

        //flattenImage(m_canvasImage, m_redrawDrawufferZone);
        Bitmap final = new Bitmap(m_displayRegion.Width, m_displayRegion.Height, PixelFormat.Format32bppArgb);

        using (Graphics gr = Graphics.FromImage(final))
        {
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            gr.DrawImage(m_canvasImage,
                new Rectangle(0, 0, m_canvasImage.Width, m_canvasImage.Height),
                new Rectangle(0, 0, m_canvasImage.Width, m_canvasImage.Height),
                GraphicsUnit.Pixel);
            gr.DrawImage(m_drawBuffer,
                m_redrawDrawBufferZone,
                m_redrawDrawBufferZone,
                GraphicsUnit.Pixel);
            gr.Save();
        }

        if (m_canvasImage != null)
            m_canvasImage.Dispose();

        m_canvasImage = final;

        drawUI();
        m_mousePosPrev = mousePos;
    }

    public void onMouseUp(int windowId)
    {
        m_isDrawing = false;

        if (m_finalImage != null)
            m_finalImage.Dispose();

        diagnostics.restartTimer();
        m_finalImage = m_layers.flattenImage(m_displayRegion.Size, m_drawBuffer, BlendMode.DrawAdd);
        Console.Write("\nFLATTEN ALL LAYERS TIME: " + diagnostics.getTimeElapsedAndRestart());

        if (m_canvasImage != null)
            m_canvasImage.Dispose();

        m_canvasImage = (Bitmap)m_finalImage.Clone();

        drawUI();
        clearDrawBuffer();
    }

    public void onKeyPress(int windowId, int key)
    {

    }


    public void close()
    {

    }


    //////////////////////////////////////////////////
    //////////////////////////////////////////////////
    //     Program Functions
    //////////////////////////////////////////////////
    //////////////////////////////////////////////////
    
    /// <summary>
    /// Draws the UI.
    /// Currently Draws a box along the edge of m_redrawDrawBufferZone to display_canvas.Image.
    /// </summary>
    private void drawUI()
    {
        // ouline drawbuffer redraw zone
        for (int i = m_redrawDrawBufferZone.X; i < m_redrawDrawBufferZone.X + m_redrawDrawBufferZone.Width; i++)
        {
            int x = i;
            int bottom = m_redrawDrawBufferZone.Y + m_redrawDrawBufferZone.Height - 1;
            int top = m_redrawDrawBufferZone.Y;

            // skip pixels that are out of bounds
            if (x < 0 || x >= m_displayRegion.Width)
                continue;

            if (bottom >= m_displayRegion.Height || bottom < 0) { }
            else
                m_canvasImage.SetPixel(x, bottom, Color.Black);
            if (top < 0 || top >= m_displayRegion.Height) { }
            else
                m_canvasImage.SetPixel(x, top, Color.Black);
        }
        for (int i = m_redrawDrawBufferZone.Y; i < m_redrawDrawBufferZone.Y + m_redrawDrawBufferZone.Height; i++)
        {
            int right = m_redrawDrawBufferZone.X + m_redrawDrawBufferZone.Width - 1;
            int left = m_redrawDrawBufferZone.X;
            int y = i;

            // skip pixels that are out of bounds
            if (y < 0 || y >= m_displayRegion.Height)
                continue;

            if (right >= m_displayRegion.Width || right < 0) { }
            else
                m_canvasImage.SetPixel(right, y, Color.Black);
            if (left < 0 || left >= m_displayRegion.Width) { }
            else
                m_canvasImage.SetPixel(left, y, Color.Black);

        }
        // END outline drawbuffer redraw zone
        // outline current layer
        Rectangle rec = m_layers.getCurrentLayerRegion();
        for (int i = 0; i < rec.Width; i++)
        {
            int x = rec.X + i;
            int bottom = rec.Y + rec.Height - 1;
            int top = rec.Y;

            // skip pixels that are out of bounds
            if (x < 0 || x > m_displayRegion.Width)
                continue;

            // dotted line
            int div = x / 7;
            if (div % 2 == 0)
                continue;

            if (bottom >= m_displayRegion.Height || bottom < 0) { }
            else
                m_canvasImage.SetPixel(x, bottom, Color.Cyan);
            if (top < 0 || top >= m_displayRegion.Height) { }
            else
                m_canvasImage.SetPixel(x, top, Color.Cyan);
        }
        for (int i = 0; i < rec.Height; i++)
        {
            int y = rec.Y + i;
            int right = rec.X + rec.Width - 1;
            int left = rec.X;
            // skip pixels that are out of bounds
            if (y < 0 || y > m_displayRegion.Height)
                continue;

            // dotted line
            int div = y / 7;
            if (div % 2 == 0)
                continue;

            if (right >= m_displayRegion.Width || right < 0) { }
            else
                m_canvasImage.SetPixel(right, y, Color.Cyan);
            if (left < 0 || left >= m_displayRegion.Width) { }
            else
                m_canvasImage.SetPixel(left, y, Color.Cyan);
        }
        // END outline current layer
    }


    /// <summary>
    /// Draws to m_drawBuffer size of brush centered on (mousePosition)
    /// Expands m_redrawDrawBufferZone as needed
    /// </summary>
    /// <param name="mousePosition">The current position of the mouse within draw_canvas</param>
    private void drawToBuffer(Point mousePosition)
    {
        Size brushSize = m_brush.getSize();
        Bitmap brushImage = m_brush.getImage();
        int brushHalfWidth = brushSize.Width / 2;
        int brushHalfHeight = brushSize.Height / 2;

        expandUpdateArea(mousePosition);

        // test drawing and brush orientation
        for (int y = 0; y < brushSize.Height; y++)
            for (int x = 0; x < brushSize.Width; x++)
            {
                Point current = new Point(
                    mousePosition.X + x - brushHalfWidth,
                    mousePosition.Y + y - brushHalfHeight
                    );

                if (current.X < 0 || current.Y < 0)
                    continue;
                if (current.X >= m_redrawDrawBufferZone.X + m_redrawDrawBufferZone.Width ||
                    current.Y >= m_redrawDrawBufferZone.Y + m_redrawDrawBufferZone.Height)
                    continue;
                // check drawbuffer bounds (drawbuffer size is limited to canvas saze atm)
                if (current.X >= m_displayRegion.Size.Width || current.Y >= m_displayRegion.Size.Height)
                    continue;

                Color bp = brushImage.GetPixel(x, y);
                Color ip = m_drawBuffer.GetPixel(current.X, current.Y);

                // alpha multiplier from brush
                float bpAlphacoef = bp.A / (float)byte.MaxValue;

                // colors after brush is considered
                int dbA = (int)((bp.A / (float)byte.MaxValue) * m_color.A);
                int dbR = (int)((bp.R / (float)byte.MaxValue) * m_color.R);
                int dbG = (int)((bp.G / (float)byte.MaxValue) * m_color.G);
                int dbB = (int)((bp.B / (float)byte.MaxValue) * m_color.B);

                // preset final blended pixel values with previous pixel color
                int a = ip.A;
                int r = ip.R;
                int g = ip.G;
                int b = ip.B;

                switch (m_brush.getMode())
                {
                    case BlendMode.DrawAdd:
                        {
                            if (bp.A == 0)
                                break;

                            if (dbA > a)
                                a = dbA;

                            r = dbR;
                            g = dbG;
                            b = dbB;
                            break;
                        }
                    case BlendMode.DrawMix:
                        { // mix
                            if (bp.A == 0)
                                break;

                            float bpAlphaWeight = bp.A / (float)(ip.A + bp.A);
                            float ipAlphaWeight = 1.0f - bpAlphaWeight; // float ipAlphaWeight = ip.A / (float)(ip.A + bp.A);

                            a = ip.A + (int)((byte.MaxValue - ip.A) * bpAlphaWeight * bpAlphacoef);
                            r = (int)(ip.R * ipAlphaWeight + dbR * bpAlphaWeight);
                            g = (int)(ip.G * ipAlphaWeight + dbG * bpAlphaWeight);
                            b = (int)(ip.B * ipAlphaWeight + dbB * bpAlphaWeight);
                            break;
                        }
                    case BlendMode.Erase:
                        {
                            if (dbA > a)
                                a = dbA;

                            r = byte.MaxValue;
                            g = byte.MaxValue;
                            b = byte.MaxValue;

                            break;
                        }
                    default: break;
                } // END switch(m_brushMode)

                // set blended pixel to image
                m_drawBuffer.SetPixel(current.X, current.Y, Color.FromArgb(a, r, g, b));
            }
    }

    /// <summary>
    /// Checks the edge positions of m_redrawDrawBuffer.
    /// If (mousePos) is at the edge of m_redrawDrawBufferZone:
    ///    resize m_redrawDrawBufferZone based on direction and brush size.
    /// </summary>
    /// <param name="mousePos">The current position of the mouse within draw_canvas</param>
    /// <returns></returns>
    private bool expandUpdateArea(Point mousePos)
    {
        bool result = false;

        // number of pixels the redraw area expands by
        int increase = (m_displayRegion.Width + m_displayRegion.Height) / 16;

        Point moveDelta = new Point(
            mousePos.X - m_mousePosPrev.X,
            mousePos.Y - m_mousePosPrev.Y);

        Size brushSize = m_brush.getSize();
        int halfBrushWidth = brushSize.Width;
        int halfBrushHeight = brushSize.Height;

        if (moveDelta.X != 0)
        {
            int rightEdgePosition = m_redrawDrawBufferZone.X + m_redrawDrawBufferZone.Width - halfBrushWidth;
            if (mousePos.X > rightEdgePosition)
            {
                // expand right
                m_redrawDrawBufferZone.Width += increase;
            }
            else if (mousePos.X < m_redrawDrawBufferZone.X + halfBrushWidth)
            {
                // expand left
                m_redrawDrawBufferZone.X -= increase;
                m_redrawDrawBufferZone.Width += increase;
            }
            result = true;
        }

        if (moveDelta.Y != 0)
        {
            int bottomEdgePosition = m_redrawDrawBufferZone.Y + m_redrawDrawBufferZone.Height - halfBrushHeight;
            if (mousePos.Y > bottomEdgePosition)
            {
                // expand down
                m_redrawDrawBufferZone.Height += increase;
            }
            else if (mousePos.Y < m_redrawDrawBufferZone.Y + halfBrushHeight)
            {
                // expand up
                m_redrawDrawBufferZone.Y -= increase;
                m_redrawDrawBufferZone.Height += increase;
            }
            result = true;
        }

        return result;
    }
    
    /// <summary>
    /// Sets all pixels within m_redrawDrawBuffer to Color(0, 0, 0, 0).
    /// Sets all m_redrawDrawBuffer values to 0.
    /// </summary>
    private void clearDrawBuffer()
    {
        using (Graphics gr = Graphics.FromImage(m_drawBuffer))
        {
            gr.Clear(Color.Transparent);
        }

        m_redrawDrawBufferZone.X =
            m_redrawDrawBufferZone.Y =
            m_redrawDrawBufferZone.Width =
            m_redrawDrawBufferZone.Height = 0;
    }




    //////////////////////////////////////////////////
    //////////////////////////////////////////////////
    //     Menu Functions
    //////////////////////////////////////////////////
    //////////////////////////////////////////////////

    public void createNewProject(Size displaySize)
    {
        m_displayRegion.Size = displaySize;

        // create blank image
        Bitmap newImage = new Bitmap(displaySize.Width, displaySize.Height, PixelFormat.Format32bppArgb);


        if (m_finalImage != null)
            m_finalImage.Dispose();
        if (m_canvasImage != null)
            m_canvasImage.Dispose();

        // set default image to new image
        using (Graphics gr = Graphics.FromImage(newImage))
        {
            gr.Clear(Color.White);
            gr.Save();
        }

        // assign image to persisting images
        m_finalImage = newImage;
        m_canvasImage = (Bitmap)newImage.Clone();

        // clear layers
        m_layers.clearAllLayers();

        // set first layer to default image
        Bitmap lbmp = new Bitmap(400, 200, PixelFormat.Format32bppArgb); for (int y = 0; y < lbmp.Height; y++) for (int x = 0; x < lbmp.Width; x++) lbmp.SetPixel(x, y, Color.Red);
        m_layers.addLayer("Background", new Point(0, 0), (Bitmap)newImage.Clone(), 0);
        m_layers.addLayer("Layer 2", new Point(175, 100), lbmp, 0);
        m_layers.addLayer("Layer 3", new Point(0, 0), new Bitmap(displaySize.Width, displaySize.Height, PixelFormat.Format32bppArgb), 0);
    }

    public void changeActiveLayer(int position)
    {
        if (m_canvasImage != null)
            m_canvasImage.Dispose();

        m_canvasImage = (Bitmap)m_finalImage.Clone();
        m_layers.setActiveLayer(position);
        drawUI();
    }

    public bool toggleLayerVisiblity(int position)
    {
        bool result = m_layers.toggleLayerIsVisible(position);

        if (m_finalImage != null)
            m_finalImage.Dispose();

        m_finalImage = m_layers.flattenImage(
            m_displayRegion.Size,
            m_drawBuffer,
            m_brush.getMode());

        if (m_canvasImage != null)
            m_canvasImage.Dispose();

        m_canvasImage = new Bitmap(m_finalImage);
        drawUI();

        return result;
    }

    //////////////////////////////////////////////////
    //////////////////////////////////////////////////
    //      Get/Set Functions
    //////////////////////////////////////////////////
    //////////////////////////////////////////////////

    public void setBrushImage(Bitmap image)
    {
        m_brush.setImage(image);
    }
    public void setBrushMode(BlendMode mode)
    {
        m_brush.setMode(mode);
    }
    public Bitmap getFinalImageCopy()
    {
        return (Bitmap)m_finalImage.Clone();
    }
    public Bitmap getCanvasImageCopy()
    {
        return (Bitmap)m_canvasImage.Clone();
    }
    public Color getSelectedColor()
    {
        return m_color;
    }
    public void setSelectedColor(Color color)
    {
        m_color = color;
    }
    public Color getSelectedColorAlt()
    {
        return m_colorAlt;
    }
    public void setDisplaySize(Size size)
    {
        m_displayRegion = new Rectangle(0, 0, size.Width, size.Height);
    }
}