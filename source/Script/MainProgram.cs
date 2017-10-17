using System;
using System.Drawing;
using System.Drawing.Imaging;

using static EnumList;

public class MainProgram
{
    Diagnostics diagnostics = new Diagnostics("MainProgram ", ConsoleColor.Cyan);
    public EnvironmentData u_environment;

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
    // holds all image data to be stacked into the final image
    LayerManager m_layers;
    // holds precombined ranges of m_layers to limit max layers to merge when drawing
    LayerManager m_imageCache;
    // where drawing input is stored bfore it's flattened to m_finalImage
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

        u_environment = new EnvironmentData();

        diagnostics.printToConsole("INITIALIZE START");

        //////////////////////////////////////
        //////////////////////////////////////
        //                                  //
        //         DEFAULT LAYERS           //
        //                                  //
        //////////////////////////////////////
        //////////////////////////////////////

        // create layer manager
        m_layers = new LayerManager(255);
        m_imageCache = new LayerManager((int)ImageCacheId.Count);

        m_displaySize = displayImageRegion;

        // load default base image
        Bitmap baseImage;
        string dir = u_environment.getImageDirectory() + m_defaultCanvasImage;
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

        diagnostics.restartTimer();

        // load layers
        Bitmap lbmp = new Bitmap(400, 200, PixelFormat.Format32bppArgb); for (int y = 0; y < lbmp.Height; y++) for (int x = 0; x < lbmp.Width; x++) lbmp.SetPixel(x, y, Color.Red);
        Bitmap bbmp = new Bitmap(baseImage.Size.Width, baseImage.Size.Height, PixelFormat.Format32bppArgb);
        m_layers.addLayer("Background", new Rectangle(new Point(0, 0), baseImage.Size), baseImage, 0);
        m_layers.addLayer("Layer 2", new Rectangle(new Point(175, 100), lbmp.Size), lbmp, 0);
        m_layers.addLayer("Layer 3", new Rectangle(new Point(0, 0), bbmp.Size), bbmp, 0);

        baseImage.Dispose();
        lbmp.Dispose();
        bbmp.Dispose();

        diagnostics.printTimeElapsedAndRestart("ADD LAYERS TIME");

        // create redraw zone
        m_redrawDrawBufferZone = new Rectangle(0, 0, 0, 0);
        m_drawBufferMode = m_defaultBufferMode;
        createImageCache();
        
        drawUI();

        //////////////////////////////////////
        //////////////////////////////////////
        //                                  //
        //         DEFAULT BRUSHES          //
        //                                  //
        //////////////////////////////////////
        //////////////////////////////////////

        diagnostics.restartTimer();

        // set default program state
        m_isDrawing = false;

        // load default colors
        m_color = m_defaultColor;
        m_colorAlt = m_defaultColorAlt;

        // load default brush image
        dir = u_environment.getBrushDirectory() + m_defaultBrush;
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
        diagnostics.printTimeElapsedAndRestart("LOAD BRUSHES TIME");
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

        applyDrawBufferToImageCache();

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

        applyDrawBufferToImageCache();

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


        Bitmap buffer = m_imageCache.getLayerImage((int)ImageCacheId.DrawBuffer);
        if (buffer != null)
        {
            diagnostics.printToConsole("APPLYING DRAW BUFFER");
            diagnostics.restartTimer();

            m_layers.applyImageOnLayer(
                m_layers.getActiveLayer(),
                buffer,
                m_redrawDrawBufferZone,
                m_drawBufferMode);

            diagnostics.printTimeElapsedAndRestart("APPLY DRAW BUFFER TIME");
        }

        updateImageCache(m_redrawDrawBufferZone);
        
        drawUI();
        clearDrawBuffer();

        //m_layers.debugSaveLayerImagesToFile("m_layers", 0, m_layers.getLayerCount());
        //m_imageCache.debugSaveLayerImagesToFile("m_imageCache", 0, m_imageCache.getLayerCount());
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
                    Console.Write("\nCreating " + imageSize.Width + "x" + imageSize.Height + "layer with transparent white image");
                    createNewProject(Color.FromArgb(0, 255, 255, 255), imageSize);
                    m_imageCache.clearAllLayers();
                    createImageCache();
                    result = true;
                    break;
                }
            case 'E':
                {
                    m_layers.clearAllLayers();
                    Console.Write("\nCreating " + imageSize.Width + "x" + imageSize.Height + "layer with semi-transparent white image");
                    createNewProject(Color.FromArgb(0, 0, 0, 0), imageSize);
                    m_imageCache.clearAllLayers();
                    createImageCache();
                    result = true;
                    break;
                }
            case 'R':
                {
                    m_layers.clearAllLayers();
                    Console.Write("\nCreating " + imageSize.Width + "x" + imageSize.Height + "layer solid white image");
                    createNewProject(Color.FromArgb(255, 255, 255, 255), imageSize);
                    m_imageCache.clearAllLayers();
                    createImageCache();
                    result = true;
                    break;
                }
            default: break;
        }
        return result;
    }

    /// <summary>
    /// Returns true if the program should be allowed to close.
    /// </summary>
    /// <returns></returns>
    public bool close()
    {
        // do stuff before closing here
        return true;
    }


    //////////////////////////////////////////////////
    //////////////////////////////////////////////////
    //     Program Functions
    //////////////////////////////////////////////////
    //////////////////////////////////////////////////

    /// <summary>
    /// Initializes the image caches. 
    /// Requires m_layers to contain layers with images.
    /// </summary>
    private void createImageCache()
    {
        diagnostics.printToConsole("CREATING IMAGE CACHE");
        diagnostics.restartTimer();

        // use total size of all layers for each cache image for now
        m_displaySize = m_layers.getTotalSize();

        // the size the final and canvas image caches will be
        Size imageSize = m_displaySize;

        int activeLayerId = m_layers.getActiveLayer();
        int layerCount = m_layers.getLayerCount();

        // flatten images all image under the current layer
        Rectangle backgroundRegion = m_layers.getLayerRegion(0, activeLayerId);
        Bitmap background = m_imageCache.getLayerImage((int)ImageCacheId.Background);
        if (backgroundRegion.Width > 0 && backgroundRegion.Height > 0)
            background = m_layers.flattenImage(
            m_displaySize,
            new Rectangle(new Point(0, 0), backgroundRegion.Size),
            0, activeLayerId,
            new Bitmap(backgroundRegion.Width, backgroundRegion.Height, PixelFormat.Format32bppArgb));

        // get the current layer image
        // Bitmap currentLayer is a handle to the current layer image in m_layers, do not dispose
        Bitmap currentLayer = null;
        Rectangle currentRegion = m_layers.getLayerRegion(activeLayerId);
        if (m_layers.getLayerIsVisible(activeLayerId))
            currentLayer = m_layers.getLayerImage(activeLayerId);

        // create a drawbuffer
        Bitmap drawBuffer = new Bitmap(m_displaySize.Width, m_displaySize.Height, PixelFormat.Format32bppArgb);

        // flatten all images after the current layer
        Rectangle foregroundRegion = m_layers.getLayerRegion(activeLayerId + 1, layerCount);
        Bitmap foreground = m_imageCache.getLayerImage((int)ImageCacheId.Foreground);
        if (foregroundRegion.Width > 0 && foregroundRegion.Height > 0)
            foreground = m_layers.flattenImage(
            m_displaySize,
            new Rectangle(new Point(0, 0), foregroundRegion.Size),
            activeLayerId + 1, layerCount,
            new Bitmap(foregroundRegion.Width, foregroundRegion.Height, PixelFormat.Format32bppArgb));
        
        // clear any existing cached images
        m_imageCache.clearAllLayers();

        // add the flattened images, the drawbuffer and the current image
        m_imageCache.addLayer("Background", backgroundRegion, background);
        m_imageCache.addLayer("CurrentLayer", currentRegion, currentLayer);
        m_imageCache.addLayer("Foreground", foregroundRegion, foreground);
        m_imageCache.addLayer("DrawBuffer", new Rectangle(new Point(0, 0), m_displaySize), drawBuffer);

        if (background != null)
            background.Dispose();
        if (drawBuffer != null)
            drawBuffer.Dispose();
        if (foreground != null)
            foreground.Dispose();

        // flatten cached images into a single image
        Rectangle finalRegion = m_imageCache.getLayerRegion(0, (int)ImageCacheId.Foreground + 1);
        Bitmap final = m_imageCache.flattenImage(
            m_displaySize,
            finalRegion,
            0, (int)ImageCacheId.Foreground + 1,
            new Bitmap(imageSize.Width, imageSize.Height, PixelFormat.Format32bppArgb));

        // add the flattened cached images to the cache
        m_imageCache.addLayer("CanvasImage", finalRegion, final);
        m_imageCache.addLayer("FinalImage", finalRegion, final);

        if (final != null)
            final.Dispose();

        diagnostics.printTimeElapsedAndRestart("CREATE IMAGE CACHE TIME");

        //m_layers.debugSaveLayerImagesToFile("m_layers", 0, m_layers.getLayerCount());
        //m_imageCache.debugSaveLayerImagesToFile("m_imageCache", 0, m_imageCache.getLayerCount());
    }

    /// <summary>
    /// Sets the current layer image cache to the stored m_layers image for the current layer. 
    /// Flattens m_imageCache and sets copies of the result to final and canvas image caches.
    /// </summary>
    /// <param name="redrawRegion">The region that will be updated</param>
    private void updateImageCache(Rectangle redrawRegion)
    {
        diagnostics.printToConsole("UPDATING IMAGE CACHE");
        // set display size
        m_displaySize = m_layers.getTotalSize();

        // get the current layer and the layer count from project layers
        int activeLayerId = m_layers.getActiveLayer();
        int layerCount = m_layers.getLayerCount();

        // get handles to image cache images
        Bitmap currentLayerBmp = m_imageCache.getLayerImage((int)ImageCacheId.CurrentLayer);
        Bitmap finalImageBmp   = m_imageCache.getLayerImage((int)ImageCacheId.FinalImage);
        
        if (finalImageBmp != null)
            finalImageBmp = (Bitmap)m_imageCache.getLayerImage((int)ImageCacheId.FinalImage).Clone();
        
        // get current layer region
        Rectangle region = m_layers.getLayerRegion(activeLayerId);

        // reset the currentLayer cache image to the m_layer stored image
        if (m_layers.getLayerIsVisible(activeLayerId))
            currentLayerBmp = m_layers.getLayerImage(activeLayerId);

        // add the created images to image cache
        m_imageCache.setLayerImage((int)ImageCacheId.CurrentLayer, currentLayerBmp, region.Location);

        // flatten layers in image cache to create final image
        region = m_imageCache.getLayerRegion(0, (int)ImageCacheId.FinalImage + 1);
        region.Intersect(redrawRegion);
        Bitmap final = m_imageCache.flattenImage(
            m_displaySize,
            region,
            0, (int)ImageCacheId.Foreground + 1,
            finalImageBmp);

        finalImageBmp.Dispose();

        // add the final image as both the final and canvas image
        m_imageCache.setLayerImage((int)ImageCacheId.CanvasImage, final);
        m_imageCache.setLayerImage((int)ImageCacheId.FinalImage, final);

        if (final != null)
            final.Dispose();

        diagnostics.printTimeElapsedAndRestart("UPDATE IMAGE CACHE TIME");

        //m_layers.debugSaveLayerImagesToFile("m_layers", 0, m_layers.getLayerCount());
        //m_imageCache.debugSaveLayerImagesToFile("m_imageCache", 0, m_imageCache.getLayerCount());
    }



    /// <summary>
    /// Draws information about the current layer size and redraw region directly to the display image.
    /// </summary>
    private void drawUI()
    {
        Bitmap canvasImage = m_imageCache.getLayerImage((int)ImageCacheId.CanvasImage);
        // outline drawbuffer redraw zone
        // horizontal lines
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
                canvasImage.SetPixel(x, bottom, Color.Black);
            if (top < 0 || top >= m_displaySize.Height) { }
            else
                canvasImage.SetPixel(x, top, Color.Black);
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
                canvasImage.SetPixel(right, y, Color.Black);
            if (!(left < 0 || left >= m_displaySize.Width))
                canvasImage.SetPixel(left, y, Color.Black);

        }
        // END outline drawbuffer redraw zone
        // outline current layer

        Rectangle rec = m_layers.getCurrentLayerRegion();
        Color c = Color.Black;

        if (true)

            // horizontal lines
            for (int i = 0; i < rec.Width; i++)
            {
                int x = rec.X + i;
                int bottom = rec.Y + rec.Height - 1;
                int top = rec.Y;

                // skip pixels that are out of bounds
                if (x < 0 || x > m_displaySize.Width)
                    continue;

                // dotted line
                int div = x / 7;
                if (div % 2 == 0)
                    c = Color.Yellow;
                else
                    c = Color.Black;

                // draw lines
                if (!(bottom >= m_displaySize.Height || bottom < 0))
                    canvasImage.SetPixel(x, bottom, c);
                if (!(top < 0 || top >= m_displaySize.Height))
                    canvasImage.SetPixel(x, top, c);
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

            // dotted line
            int div = y / 7;
            if (div % 2 == 0)
                c = Color.Yellow;
            else
                c = Color.Black;

            // draw lines
            if (!(right >= m_displaySize.Width || right < 0))
                canvasImage.SetPixel(right, y, c);
            if (!(left < 0 || left >= m_displaySize.Width))
                canvasImage.SetPixel(left, y, c);
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

        Bitmap drawBuffer = m_imageCache.getLayerImage((int)ImageCacheId.DrawBuffer);

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
                Color ip = drawBuffer.GetPixel(current.X, current.Y);

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
                drawBuffer.SetPixel(current.X, current.Y, Color.FromArgb(a, r, g, b));
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
        Bitmap drawBuffer = m_imageCache.getLayerImage((int)ImageCacheId.DrawBuffer);

        if (drawBuffer == null)
            return;

        using (Graphics gr = Graphics.FromImage(drawBuffer))
        {
            gr.Clear(Color.Transparent);
        }

        m_redrawDrawBufferZone.X =
            m_redrawDrawBufferZone.Y =
            m_redrawDrawBufferZone.Width =
            m_redrawDrawBufferZone.Height = 0;
    }

    /// <summary>
    /// Applies the drawbuffer to the current layer image and cached current layer image.
    /// </summary>
    public void applyDrawBufferToImageCache()
    {
        // get a handle on drawbuffer and the cached current layer image
        Bitmap drawBuffer = m_imageCache.getLayerImage((int)ImageCacheId.DrawBuffer);
        Bitmap currentLayer = m_layers.getLayerImage(m_layers.getActiveLayer());

        // create copy of cached final image to set to cacged canvas image
        Bitmap finalImage = (Bitmap)m_imageCache.getLayerImage((int)ImageCacheId.FinalImage).Clone();

        // reset the cached canvas image
        m_imageCache.setLayerImage((int)ImageCacheId.CanvasImage, finalImage);

        // reset the cached current layer image
        m_imageCache.setLayerImage((int)ImageCacheId.CurrentLayer, currentLayer);

        // apply the drawbuffer to the cached current layer image
        m_imageCache.applyImageOnLayer((int)ImageCacheId.CurrentLayer, drawBuffer, m_redrawDrawBufferZone, m_drawBufferMode);

        // flatten the cached images
        Bitmap canvasImage = m_imageCache.flattenImage(
            m_displaySize,
            m_redrawDrawBufferZone,
            0, (int)ImageCacheId.Foreground + 1,
            finalImage);

        if (finalImage != null)
            finalImage.Dispose();

        // set the display image to the flattened cache images
        m_imageCache.setLayerImage((int)ImageCacheId.CanvasImage, canvasImage);

        if (canvasImage != null)
            canvasImage.Dispose();
    }

    //////////////////////////////////////////////////
    //////////////////////////////////////////////////
    //     Menu Functions
    //////////////////////////////////////////////////
    //////////////////////////////////////////////////

    /// <summary>
    /// Create a new project using an image as the starting layer.
    /// </summary>
    public void createNewProject(Bitmap image)
    {
        m_displaySize = image.Size;

        // clear layers
        m_layers.clearAllLayers();
        m_imageCache.clearAllLayers();

        // set first layer to input image
        m_layers.addLayer("Layer 1", new Rectangle(new Point(0, 0), image.Size), image, 0);

        createImageCache();
        drawUI();
    }
    /// <summary>
    /// Create a new project using a solid color as the starting layer.
    /// </summary>
    public void createNewProject(Color color, Size size)
    {
        Bitmap image = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);

        using (Graphics gr = Graphics.FromImage(image))
        {
            gr.Clear(color);
        }

        m_displaySize = image.Size;

        // clear layers
        m_layers.clearAllLayers();
        m_imageCache.clearAllLayers();

        // set first layer to input image
        m_layers.addLayer("Layer 1", new Rectangle(new Point(0, 0), image.Size), image, 0);

        createImageCache();
        drawUI();
    }

    /// <summary>
    /// Changes the active layer to the specified layer.
    /// </summary>
    /// <param name="position">The id of the layer to be made active.</param>
    public void changeActiveLayer(int position)
    {
        m_layers.setActiveLayer(position);
        createImageCache();

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

        Rectangle region = new Rectangle(new Point(0, 0), m_layers.getTotalSize());

        string str = "";
        if (result)
            str = "TOGGLE LAYER " + position + " ON";
        else
            str = "TOGGLE LAYER " + position + " OFF";


        diagnostics.printToConsole("START " + str);

        //updateFinalImage(m_displaySize, region);
        createImageCache();

        drawUI();
        diagnostics.printToConsole("END " + str);

        //m_layers.debugSaveLayerImagesToFile("m_layers", 0, m_layers.getLayerCount());
        //m_imageCache.debugSaveLayerImagesToFile("m_imageCache", 0, m_imageCache.getLayerCount());

        return result;
    }

    public bool addLayer(string name, Size size, Point position)
    {
        Bitmap bmp = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
        bool result = m_layers.addLayer(name, new Rectangle(position, size), bmp);
        bmp.Dispose();

        if (result == true)
            createImageCache();

        Console.Write("\n number of layers: " + m_layers.getLayerCount());

        return result;
    }

    //////////////////////////////////////////////////
    //////////////////////////////////////////////////
    //      Get/Set Functions
    //////////////////////////////////////////////////
    //////////////////////////////////////////////////

    public bool setBrushImage(Bitmap image)
    {
        return m_brush.setImage(image);
    }
    public void setBrushMode(BlendMode brushMode, BlendMode bufferMode)
    {
        m_brush.setMode(brushMode);
        m_drawBufferMode = bufferMode;
    }
    /// <summary>
    /// Returns a copy of the current final image.
    /// </summary>
    /// <returns></returns>
    public Bitmap getFinalImageCopy()
    {
        return (Bitmap)m_imageCache.getLayerImage((int)ImageCacheId.FinalImage).Clone();
    }
    /// <summary>
    /// Returns a copy of the current display image.
    /// </summary>
    /// <returns></returns>
    public Bitmap getCanvasImageCopy()
    {
        return (Bitmap)m_imageCache.getLayerImage((int)ImageCacheId.CanvasImage).Clone();
    }
    public Color getSelectedColor()
    {
        return m_color;
    }
    /// <summary>
    /// Sets the primary color to the input color. 
    /// Returns true on success.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public bool setSelectedColor(Color color)
    {
        if (color == Color.Empty)
            return false;

        m_color = color;
        return true;
    }
    /// <summary>
    /// Sets the secondary color to the input color. 
    /// Returns true on success.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public bool setSelectedColorAlt(Color color)
    {
        if (color == Color.Empty)
            return false;
        
        m_colorAlt = color;
        return true;
    }
    public Color getSelectedColorAlt()
    {
        return m_colorAlt;
    }
    public void swapSelectedColors()
    {
        Color c = m_color;

        m_color = m_colorAlt;
        m_colorAlt = c;
    }
    public bool setDisplaySize(Size size)
    {
        if (size.Width > 0 ||
            size.Height > 0 ||
            size == Size.Empty)
            return false;

        m_displaySize = size;
        return true;
    }
    public int getLayerCount()
    {
        return m_layers.getLayerCount();
    }
    /// <summary>
    /// Get the name of the layer at position. 
    /// Returns null on fail.
    /// </summary>
    public string getLayerText(int position)
    {
        return m_layers.getName(position);
    }
    /// <summary>
    /// Gets the region the current layer covers. 
    /// Returns Rectagle.Empty on fail.
    /// </summary>
    /// <returns></returns>
    public Rectangle getCurrentLayerRegion()
    {
        return m_layers.getCurrentLayerRegion();
    }
}
