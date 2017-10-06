using System.Drawing;
using System.Drawing.Imaging;

// stores an image, a location and data about the image
public class CanvasFragment
{
    private BitField m_flags = new BitField();
    private Bitmap m_image = null;

    // distance from left side and top
    private int m_x = int.MinValue;
    private int m_y = int.MinValue;

    public CanvasFragment(Bitmap image, int x, int y, int flags = 0)
    {
        m_flags.set(flags);
        setPosition(x, y);
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
        m_x = int.MinValue;
        m_y = int.MinValue;
    }

    /// <summary>
    /// Stores a copy of the image.
    /// </summary>
    /// <param name="image"></param>
    public void setImage(Bitmap image)
    {
        if (m_image != null)
            m_image.Dispose();

        if (image != null)
            m_image = (Bitmap)image.Clone();
    }
    public Color getPixel(int x, int y)
    {
        if (m_image == null)
            return Color.Transparent;

        int posX = x - m_x;
        int posY = y - m_y;

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

        int posX = x - m_x;
        int posY = y - m_y;

        if (posX < 0 || posY < 0)
            return;
        if (posX >= m_image.Width || posY >= m_image.Height)
            return;

        m_image.SetPixel(posX, posY, color);
    }
    public void resize(int width, int height, Point imageOffset)
    {
        if (m_image == null)
            return;

        Bitmap old = m_image;
        m_image = new Bitmap(width, height, PixelFormat.Format32bppArgb);

        using (Graphics gr = Graphics.FromImage(m_image))
        {
            gr.DrawImageUnscaled(old, imageOffset);
            gr.Save();
        }
        old.Dispose();
    }

    public void setPosition(int x, int y) { m_x = x; m_y = y; }
    public Size getSize() { return m_image.Size; }
    public Point getPosition() { return new Point(m_x, m_y); }
    /// <summary>
    /// Returns a handle to the stored image.
    /// </summary>
    /// <returns></returns>
    public Bitmap getImage() { return m_image; }

    public void addFlag(int flag) { m_flags.add(flag); }
    public void removeFlag(int flag) { m_flags.remove(flag); }
    public bool hasFlag(int flag) { return m_flags.has(flag); }
}
