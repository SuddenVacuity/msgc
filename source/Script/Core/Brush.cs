using System.Drawing;

using static EnumList;

public class Brush
{
    private Bitmap m_image;
    private BlendMode m_mode;

    public Brush(Bitmap image, BlendMode mode)
    {
        m_image = image;
        m_mode = mode;
    }

    ~Brush()
    {
        if (m_image != null)
            m_image.Dispose();
    }

    public void setImage(Bitmap image)
    {
        if (m_image != null)
            m_image.Dispose();

        m_image = image;
    }
    public Bitmap getImage()            { return m_image; }
    public void setMode(BlendMode mode) { m_mode = mode; }
    public BlendMode getMode()          { return m_mode; }
    public Size getSize()               { return new Size(m_image.Width, m_image.Height); }
}
