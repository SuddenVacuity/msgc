using System;
using System.Drawing;
using System.Drawing.Imaging;

using static EnumList;

public class MainProgram
{
    public string version = "v0.0.0.5";

    Diagnostics diagnostics = new Diagnostics("MainProgram ", ConsoleColor.Cyan);
    public EnvironmentData m_environment = new EnvironmentData();

    // default values
    private BlendMode m_defaultBrushMode = BlendMode.ReplaceClampAlpha;
    private BlendMode m_defaultBufferMode = BlendMode.Add;
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
    // where drawing input is stored bfore it's flattened to m_finalImage
    private Bitmap m_drawBuffer;
    private BlendMode m_drawBufferMode;

    ////////////////////
    // drawing tools
    ////////////////////
    // the position the mouse was last mousemove update
    private Point m_mousePosPrev;
    private bool m_isDrawing;
    // region where m_drawBuffer has been drawn on
    private Rectangle m_redrawDrawBufferZone;

    private Color m_color;
    private Color m_colorAlt;
    private Brush m_brush;

    Size m_displaySize;

    /// <summary>
    /// loads images into the program, creates layers and brushes.
    /// </summary>
    /// <param name="displayImageRegion">The size in pizels of the display canvas</param>
    public void init(Size displayImageRegion)
    {
        diagnostics.setActive(true);
        m_environment.init();

        //////////////////////////////////////
        //////////////////////////////////////
        //                                  //
        //         DEFAULT LAYERS           //
        //                                  //
        //////////////////////////////////////
        //////////////////////////////////////

        // create layer manager
        m_layers = new LayerManager();

        m_displaySize = displayImageRegion;

        // load default base image
        Bitmap baseImage;
        string dir = m_environment.getImageDirectory() + m_defaultCanvasImage;
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

        // create redraw zone
        m_redrawDrawBufferZone = new Rectangle(0, 0, 0, 0);
        
        // load layers
        Bitmap lbmp = new Bitmap(400, 200, PixelFormat.Format32bppArgb); for (int y = 0; y < lbmp.Height; y++) for (int x = 0; x < lbmp.Width; x++) lbmp.SetPixel(x, y, Color.Red);
        m_layers.addLayer("Background", new Point(0, 0), (Bitmap)baseImage.Clone(), 0);
        m_layers.addLayer("Layer 2", new Point(175, 100), lbmp, 0);
        m_layers.addLayer("Layer 3", new Point(0, 0), new Bitmap(1000, 1000, PixelFormat.Format32bppArgb), 0);

        // set display size
        m_displaySize = m_layers.getTotalSize();
        
        m_finalImage = new Bitmap(m_displaySize.Width, m_displaySize.Height, PixelFormat.Format32bppArgb);

        // create draw buffer
        m_drawBuffer = new Bitmap(m_displaySize.Width, m_displaySize.Height, PixelFormat.Format32bppArgb);
        m_drawBufferMode = m_defaultBufferMode;

        Bitmap final = m_layers.flattenImage(
            m_displaySize,
            new Rectangle(new Point(0, 0), m_displaySize),
            0, m_layers.getLayerCount(),
            m_finalImage);

        if (m_finalImage != null)
            m_finalImage.Dispose();

        m_finalImage = final;

        if (m_canvasImage != null)
            m_canvasImage.Dispose();

        m_canvasImage = (Bitmap)m_finalImage.Clone();
        drawUI();

        //////////////////////////////////////
        //////////////////////////////////////
        //                                  //
        //         DEFAULT BRUSHES          //
        //                                  //
        //////////////////////////////////////
        //////////////////////////////////////
        
        // set default program state
        m_isDrawing = false;

        // load default colors
        m_color = m_defaultColor;
        m_colorAlt = m_defaultColorAlt;

        // load default brush image
        dir = m_environment.getBrushDirectory() + m_defaultBrush;
        Bitmap brushImage;
        if (System.IO.File.Exists(dir))
        {
            // if file exists load and invert rgb values
            brushImage = new Bitmap(dir);
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
        m_brush = new Brush(brushImage, m_defaultBrushMode);
    }

    /// <summary>
    /// Runs when the mouse is clicked. 
    /// Sets application state. 
    /// Sets initial redraw region. 
    /// Draws the brush to the drawbuffer. 
    /// Applies the drawbuffer to the display image. 
    /// Draws UI feedback to the display image. 
    /// </summary>
    /// <param name="windowId">The id of the window the event was triggered on - UNUSED</param>
    /// <param name="cursorPosition">The position of the cursor within the element of the window</param>
    public void onMouseDown(int windowId, Point cursorPosition)
    {
        m_isDrawing = true;
        
        // store current mouse position
        m_mousePosPrev = cursorPosition;

        Console.Write("\nClick position within draw_canvas: " + m_mousePosPrev.X + ", " + m_mousePosPrev.Y);

        // set redrawregion to the size of the brush
        // and centered on the cursor position
        Size brushSize = m_brush.getSize();
        m_redrawDrawBufferZone.X = m_mousePosPrev.X - brushSize.Width;
        m_redrawDrawBufferZone.Y = m_mousePosPrev.Y - brushSize.Height;
        m_redrawDrawBufferZone.Width = brushSize.Width * 2;
        m_redrawDrawBufferZone.Height = brushSize.Height * 2;

        // draw to buffer
        drawToBuffer(cursorPosition);
        applyDrawBufferToCanvasImage();

        // draw UI feedback
        drawUI();
    }

    /// <summary>
    /// Runs when the mouse moves. 
    /// Checks application state to determine actions. 
    /// Draws the brush to the drawbuffer. 
    /// Applies the drawbuffer to the display image. 
    /// Draws UI feedback to the display image. 
    /// </summary>
    /// <param name="windowId">The id of the window the event was triggered on - UNUSED</param>
    /// <param name="cursorPosition">The position of the cursor within the element of the window</param>
    public void onMouseMove(int windowId, Point cursorPosition)
    {
        if (!m_isDrawing)
            return;

        // current mouse position
        Point mousePos = cursorPosition;

        drawToBuffer(mousePos);
        applyDrawBufferToCanvasImage();

        drawUI();
        m_mousePosPrev = mousePos;
    }

    /// <summary>
    /// Runs when a mouse button is released. 
    /// Checks application state to determine actions. 
    /// Applies the drawbuffer to the current layer's image. 
    /// Flattens all visible layers together and set m_finalImage to the result. 
    /// Draws UI feedback to the display image. 
    /// Clears the drawbuffer and resets the redraw region.
    /// </summary>
    /// <param name="windowId">The id of the window the event was triggered on - UNUSED</param>
    public void onMouseUp(int windowId)
    {
        if (!m_isDrawing)
            return;

        m_isDrawing = false;
        
        diagnostics.restartTimer();
        m_layers.applyDrawBuffer(m_drawBuffer, m_redrawDrawBufferZone, m_drawBufferMode);
        diagnostics.printTimeElapsedAndRestart("APPLY DRAW BUFFER TIME");

        updateFinalImage(m_displaySize, m_redrawDrawBufferZone, m_finalImage);

        drawUI();
        clearDrawBuffer();
    }

    /// <summary>
    /// Runs when a key is pressed.
    /// </summary>
    /// <param name="windowId">The window that recived the  key input</param>
    /// <param name="key">The Id of the key that was pressed.</param>
    /// <param name="keyModifiers">Key modifiers. (shift, ctrl, alt, etc)</param>
    public bool onKeyPress(int windowId, int key, int keyModifiers)
    {
        // result returns true if the window need to refresh its image
        bool result = false;
        Size imageSize = new Size(4000, 4000);

        switch (key)
        {
            case 'W':
                {
                    m_layers.clearAllLayers();
                    Bitmap bmp = new Bitmap(imageSize.Width, imageSize.Height, PixelFormat.Format32bppArgb);
                    Console.Write("\nCreating " + imageSize.Width + "x" + imageSize.Height + " layers with transparent image");
                    createNewProject(bmp, 3);
                    result = true;
                    break;
                }
            case 'E':
                {
                    m_layers.clearAllLayers();
                    Bitmap bmp;
                    Bitmap img = new Bitmap(imageSize.Width, imageSize.Height, PixelFormat.Format32bppArgb);
                    using (Graphics gr = Graphics.FromImage(img))
                    {
                        gr.Clear(Color.FromArgb(100, 255, 255, 255));
                    }
                    bmp = img;
                    Console.Write("\nCreating " + imageSize.Width + "x" + imageSize.Height + "layers with semi-transparent white image");
                    createNewProject(bmp, 3);
                    result = true;
                    break;
                }
            case 'R':
                {
                    m_layers.clearAllLayers();
                    Bitmap bmp;
                    Bitmap img = new Bitmap(imageSize.Width, imageSize.Height, PixelFormat.Format32bppArgb);
                    using (Graphics gr = Graphics.FromImage(img))
                    {
                        gr.Clear(Color.White);
                    }
                    bmp = img;
                    Console.Write("\nCreating " + imageSize.Width + "x" + imageSize.Height + "layers solid white image");
                    createNewProject(bmp, 3);
                    result = true;
                    break;
                }
            default: break;
        }
        return result;
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
    /// Draws information about the current layer size and redraw region directly to the display image.
    /// </summary>
    private void drawUI()
    {
        // ouline drawbuffer redraw zone
        // horizaontal lines
        for (int i = m_redrawDrawBufferZone.X; i < m_redrawDrawBufferZone.X + m_redrawDrawBufferZone.Width; i++)
        {
            int x = i;
            int bottom = m_redrawDrawBufferZone.Y + m_redrawDrawBufferZone.Height - 1;
            int top = m_redrawDrawBufferZone.Y;

            // skip pixels that are out of bounds
            if (x < 0 || x >= m_displaySize.Width)
                continue;

            if (bottom >= m_displaySize.Height || bottom < 0) { }
            else
                m_canvasImage.SetPixel(x, bottom, Color.Black);
            if (top < 0 || top >= m_displaySize.Height) { }
            else
                m_canvasImage.SetPixel(x, top, Color.Black);
        }
        // vertical lines
        for (int i = m_redrawDrawBufferZone.Y; i < m_redrawDrawBufferZone.Y + m_redrawDrawBufferZone.Height; i++)
        {
            int right = m_redrawDrawBufferZone.X + m_redrawDrawBufferZone.Width - 1;
            int left = m_redrawDrawBufferZone.X;
            int y = i;

            // skip pixels that are out of bounds
            if (y < 0 || y >= m_displaySize.Height)
                continue;

            if (!(right >= m_displaySize.Width || right < 0))
                m_canvasImage.SetPixel(right, y, Color.Black);
            if (!(left < 0 || left >= m_displaySize.Width))
                m_canvasImage.SetPixel(left, y, Color.Black);
        }
        // END outline drawbuffer redraw zone
        // outline current layer
        // horizontal lines
        Rectangle rec = m_layers.getCurrentLayerRegion();
        for (int i = 0; i < rec.Width; i++)
        {
            int x = rec.X + i;
            int bottom = rec.Y + rec.Height - 1;
            int top = rec.Y;

            // skip pixels that are out of bounds
            if (x < 0 || x > m_displaySize.Width)
                continue;

            Color c = Color.Cyan;

            // dotted line
            int div = x / 7;
            if (div % 2 == 0)
                c = Color.Black;

            if (!(bottom >= m_displaySize.Height || bottom < 0))
                m_canvasImage.SetPixel(x, bottom, c);
            if (!(top < 0 || top >= m_displaySize.Height))
                m_canvasImage.SetPixel(x, top, c);
        }
        // vertical lines
        for (int i = 0; i < rec.Height; i++)
        {
            int y = rec.Y + i;
            int right = rec.X + rec.Width - 1;
            int left = rec.X;
            // skip pixels that are out of bounds
            if (y < 0 || y > m_displaySize.Height)
                continue;

            Color c = Color.Cyan;

            // dotted line
            int div = y / 7;
            if (div % 2 == 0)
                c = Color.Black;

            if (!(right >= m_displaySize.Width || right < 0))
                m_canvasImage.SetPixel(right, y, c);
            if (!(left < 0 || left >= m_displaySize.Width))
                m_canvasImage.SetPixel(left, y, c);
        }
        // END outline current layer
    }


    /// <summary>
    /// Draws to m_drawBuffer with the brush centered on (mousePosition). 
    /// Expands m_redrawDrawBufferZone as needed.
    /// </summary>
    /// <param name="mousePosition">The current position of the mouse within draw_canvas</param>
    private void drawToBuffer(Point mousePosition)
    {
        Size brushSize = m_brush.getSize();
        Bitmap brushImage = m_brush.getImage();
        int brushHalfWidth = brushSize.Width / 2;
        int brushHalfHeight = brushSize.Height / 2;
        BlendMode mode = m_brush.getMode();

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
                if (current.X >= m_displaySize.Width || current.Y >= m_displaySize.Height)
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

                switch (mode)
                {
                    case BlendMode.ReplaceClampAlpha:
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
                    case BlendMode.Mix:
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
    /// Returns true if the redraw zone was expanded to contain the region of drawbuffer that was drawn to. 
    /// </summary>
    /// <param name="mousePos">The current position of the mouse within draw_canvas</param>
    /// <returns></returns>
    private bool expandUpdateArea(Point mousePos)
    {
        bool result = false;

        Point moveDelta = new Point(
            mousePos.X - m_mousePosPrev.X,
            mousePos.Y - m_mousePosPrev.Y);

        moveDelta.X = Math.Abs(moveDelta.X);
        moveDelta.Y = Math.Abs(moveDelta.Y);

        Size brushSize = m_brush.getSize();
        int halfBrushWidth = brushSize.Width;
        int halfBrushHeight = brushSize.Height;

        if (moveDelta.X != 0)
        {
            int rightEdgePosition = m_redrawDrawBufferZone.X + m_redrawDrawBufferZone.Width - halfBrushWidth;
            if (mousePos.X > rightEdgePosition)
            {
                // expand right
                m_redrawDrawBufferZone.Width += moveDelta.X;
            }
            else if (mousePos.X < m_redrawDrawBufferZone.X + halfBrushWidth)
            {
                // expand left
                m_redrawDrawBufferZone.X -= moveDelta.X;
                m_redrawDrawBufferZone.Width += moveDelta.X;
            }
            result = true;
        }

        if (moveDelta.Y != 0)
        {
            int bottomEdgePosition = m_redrawDrawBufferZone.Y + m_redrawDrawBufferZone.Height - halfBrushHeight;
            if (mousePos.Y > bottomEdgePosition)
            {
                // expand down
                m_redrawDrawBufferZone.Height += moveDelta.Y;
            }
            else if (mousePos.Y < m_redrawDrawBufferZone.Y + halfBrushHeight)
            {
                // expand up
                m_redrawDrawBufferZone.Y -= moveDelta.Y;
                m_redrawDrawBufferZone.Height += moveDelta.Y;
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

    /// <summary>
    /// Applies the drawbuffer to the canvas image.
    /// </summary>
    public void applyDrawBufferToCanvasImage()
    {
        if (m_canvasImage != null)
            m_canvasImage.Dispose();

        // reset the canvas image to the final image
        m_canvasImage = (Bitmap)m_finalImage.Clone();

        // apply the drawbuffer to the display image only
        Bitmap final = new Bitmap(m_displaySize.Width, m_displaySize.Height, PixelFormat.Format32bppArgb);

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
    }

    /// <summary>
    /// Flattens all visible layers and sets m_finalImage and m_canvasImage to the result.
    /// </summary>
    /// <param name="displaySize">The size of the display_canvas in the main window</param>
    /// <param name="redrawZone">The region that has data to be redrawn.</param>
    /// <param name="cachedImage">The previous image to reuse old data from.</param>
    private void updateFinalImage(Size displaySize, Rectangle redrawZone, Bitmap cachedImage)
    {
        diagnostics.restartTimer();
        Bitmap final = m_layers.flattenImage(
            displaySize,
            redrawZone,
            0, m_layers.getLayerCount(),
            cachedImage);

        if (m_finalImage != null)
            m_finalImage.Dispose();

        m_finalImage = final;

        if (m_canvasImage != null)
            m_canvasImage.Dispose();

        m_canvasImage = (Bitmap)m_finalImage.Clone();

        diagnostics.printTimeElapsedAndRestart("FLATTEN LAYERS TIME");
    }


    //////////////////////////////////////////////////
    //////////////////////////////////////////////////
    //     Menu Functions
    //////////////////////////////////////////////////
    //////////////////////////////////////////////////

    /// <summary>
    /// Quick and dirty function to reset the application's image
    /// </summary>
    /// <param name="displaySize"></param>
    public void createNewProject(Bitmap image, int layerCount)
    {
        m_displaySize = image.Size;
        
        // clear layers
        m_layers.clearAllLayers();

        // set first layer to default image
        for(int i = 0; i < layerCount; i++)
            m_layers.addLayer("Layer " + i, new Point(0, 0), image, 0);

        // create blank image
        Bitmap newImage = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);

        // set default image to new image
        using (Graphics gr = Graphics.FromImage(newImage))
        {
            gr.Clear(Color.Transparent);
            gr.Save();
        }

        if (m_finalImage != null)
            m_finalImage.Dispose();
        if (m_canvasImage != null)
            m_canvasImage.Dispose();

        // assign image to persisting images
        m_finalImage = (Bitmap)newImage.Clone();
        m_canvasImage = (Bitmap)newImage.Clone();

        drawUI();

        newImage.Dispose();
    }

    /// <summary>
    /// Changes the active layer to the specified layer.
    /// </summary>
    /// <param name="position">The id of the layer to be made active.</param>
    public void changeActiveLayer(int position)
    {
        if (m_canvasImage != null)
            m_canvasImage.Dispose();

        m_canvasImage = (Bitmap)m_finalImage.Clone();
        m_layers.setActiveLayer(position);
        drawUI();
    }

    /// <summary>
    /// Toggles the visibility of the specified layer.
    /// </summary>
    /// <param name="position">The id of the layer to be toggled.</param>
    /// <returns></returns>
    public bool toggleLayerVisiblity(int position)
    {
        bool result = m_layers.toggleLayerIsVisible(position);

        Rectangle region = new Rectangle(new Point(0, 0),  m_layers.getTotalSize());

        Bitmap bmp = new Bitmap(region.Width, region.Height);
        updateFinalImage(m_displaySize, region, bmp);
        bmp.Dispose();

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
    public void setBrushMode(BlendMode brushMode, BlendMode bufferMode)
    {
        m_brush.setMode(brushMode);
        m_drawBufferMode = bufferMode;
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
        m_displaySize = size;
    }
}