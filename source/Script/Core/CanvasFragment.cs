using System.Drawing;
using System.Drawing.Imaging;

// stores an image, a location and data about the image
public class CanvasFragment
{
    private BitField m_flags = new BitField();
    private Bitmap m_image = null;
    private Rectangle m_region = Rectangle.Empty;

    public CanvasFragment(Bitmap image, Rectangle region, int flags = 0)
    {
        m_flags.set(flags);
        if (setRegion(region) == false || setImage(image) == false)
        {
            clear();
            throw new System.ArgumentNullException();
        }
    }
    public CanvasFragment(Bitmap image, int flags = 0)
    {
        m_flags.set(flags);
        if (setImage(image) == false)
        {
            clear();
            throw new System.ArgumentNullException();
        }
    }

    ~CanvasFragment()
    {
        clear();
    }

    public void clear()
    {
        m_flags = new BitField();

        if (m_image != null)
            m_image.Dispose();

        m_image = null;
        m_region = Rectangle.Empty;
    }

    /// <summary>
    /// Stores a copy of the image and sets region size to the image size. 
    /// Returns true if image storage was successful.
    /// </summary>
    public bool setImage(Bitmap image)
    {
        if (image == null)
            return false;

        if (m_image != null)
            m_image.Dispose();

        m_image = (Bitmap)image.Clone();
        m_region = new Rectangle(m_region.Location, m_image.Size);

        return true;
    }
    /// <summary>
    /// Resource Intensive - AVOID USING. Returns the color of the pixel at x, y coordinates. 
    /// Returns Color.Empty if failed.
    /// </summary>
    public Color getPixel(int x, int y)
    {
        if (m_image == null)
            return Color.Empty;

        int posX = x;
        int posY = y;

        if (posX < 0 || posY < 0)
            return Color.Empty;
        if (posX >= m_image.Width || posY >= m_image.Height)
            return Color.Empty;

        return m_image.GetPixel(posX, posY);
    }
    /// <summary>
    /// Resource Intensive - AVOID USING. Returns true if the pixel was successfully set. 
    /// </summary>
    public bool setPixel(int x, int y, Color color)
    {
        if (color == Color.Empty)
            return false;

        int posX = x;
        int posY = y;

        if (posX < 0 || posY < 0)
            return false;
        if (posX >= m_image.Width || posY >= m_image.Height)
            return false;

        m_image.SetPixel(posX, posY, color);
        return true;
    }
    
    /// <summary>
    /// Sets the canvas region. region must have width and height great than 0. 
    /// Returns true on success.
    /// </summary>
    /// <param name="region"></param>
    /// <returns></returns>
    public bool setRegion(Rectangle region)
    {
        if (region == Rectangle.Empty)
            return false;
        if (region.Width <= 0 || region.Height <= 0)
            return false;

        m_region = new Rectangle(region.Location, region.Size);
        return true;
    }
    /// <summary>
    /// Returns a copy of the size of the image contained within the canvas. 
    /// Returns Size.Empty if failed.
    /// </summary>
    public Size getSize()
    {
        if (m_region == Rectangle.Empty)
            return new Size(m_region.Size.Width, m_region.Size.Height);
        else
            return Size.Empty;
    }

    /// <summary>
    /// Returns a handle to the stored image.
    /// </summary>
    /// <returns></returns>
    public Bitmap getImage() { return m_image; }
    /// <summary>
    /// Returns a copy of the region the image in the fragment covers. 
    /// Returns Rectangle.Empty if failed.
    /// </summary>
    /// <returns></returns>
    public Rectangle getRegion()
    {
        if (m_region == Rectangle.Empty)
            return Rectangle.Empty;

        return new Rectangle(m_region.Location, m_region.Size);
    }
    public void addFlag(int flag) { m_flags.add(flag); }
    public void removeFlag(int flag) { m_flags.remove(flag); }
    public bool hasFlag(int flag) { return m_flags.has(flag); }
    
}
