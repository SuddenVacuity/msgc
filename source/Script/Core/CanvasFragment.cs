using System.Drawing;
using System.Drawing.Imaging;

// stores an image, a location and data about the image
public class CanvasFragment
{
    private BitField m_flags = new BitField();
    private Bitmap m_image = null;

    private Rectangle m_region = new Rectangle(int.MinValue, int.MinValue, int.MinValue, int.MinValue);

    public CanvasFragment(Bitmap image, Rectangle region, int flags = 0)
    {
        m_flags.set(flags);
        setRegion(region);
        setImage(image);
    }
    public CanvasFragment(Bitmap image, int flags = 0)
    {
        m_flags.set(flags);
        setImage(image);
    }

    ~CanvasFragment()
    {
        if (m_image != null)
            m_image.Dispose();
    }

    public void clear()
    {
        m_flags = new BitField();

        if (m_image != null)
            m_image.Dispose();

        m_region.X = int.MinValue;
        m_region.Y = int.MinValue;
        m_region.Width = int.MinValue;
        m_region.Height = int.MinValue;
    }

    /// <summary>
    /// Stores a copy of the image.
    /// </summary>
    /// <param name="image"></param>
    public void setImage(Bitmap image)
    {
        if (m_image != null)
            m_image.Dispose();

        // this keeps getting argument exception on .Clone()
        //if (image != null)
        //    m_image = (Bitmap)image.Clone();

        try
        {
            m_image = (Bitmap)image.Clone();
            m_region = new Rectangle(m_region.Location, m_image.Size);
        }
        catch
        {
            m_flags = null;
            m_region = new Rectangle(m_region.X, m_region.Y, 0, 0);
        }
    }
    public Color getPixel(int x, int y)
    {
        if (m_image == null)
            return Color.Transparent;

        int posX = x - m_region.X;
        int posY = y - m_region.Y;

        if (posX < 0 || posY < 0)
            return Color.FromArgb(0, 0, 0, 0);
        if (posX >= m_image.Width || posY >= m_image.Height)
            return Color.FromArgb(0, 0, 0, 0);

        return m_image.GetPixel(posX, posY);
    }
    public void setPixel(int x, int y, Color color)
    {
        if (m_image == null)
            return;

        int posX = x - m_region.X;
        int posY = y - m_region.Y;

        if (posX < 0 || posY < 0)
            return;
        if (posX >= m_image.Width || posY >= m_image.Height)
            return;

        m_image.SetPixel(posX, posY, color);
    }

    public void setRegion(Rectangle region) { m_region = region; }

    public Size getSize()
    {
        if (m_image != null)
            return m_image.Size;
        else
            return new Size(0, 0);
    }

    public Rectangle getRegion() { return m_region; }
    /// <summary>
    /// Returns a handle to the stored image.
    /// </summary>
    /// <returns></returns>
    public Bitmap getImage() { return m_image; }

    public void addFlag(int flag) { m_flags.add(flag); }
    public void removeFlag(int flag) { m_flags.remove(flag); }
    public bool hasFlag(int flag) { return m_flags.has(flag); }
}
