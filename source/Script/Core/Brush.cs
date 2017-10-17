using System.Drawing;

using static EnumList;

public class Brush
{
    private Bitmap m_image;
    private BlendMode m_mode;

    public Brush(Bitmap image, BlendMode mode)
    {
        m_mode = mode;
        if (setImage(image) == false)
        {
            m_image = null;
            m_mode = BlendMode.None;
            throw new System.ArgumentNullException();
        }
    }

    ~Brush()
    {
        m_mode = BlendMode.None;
        if (m_image != null)
            m_image.Dispose();
    }

    public bool setImage(Bitmap image)
    {
        if (image == null)
            return false;

        if (m_image != null)
            m_image.Dispose();

        m_image = (Bitmap)image.Clone();

        invertImageColor();

        return true;
    }

    // brush image files are black to represent the brush
    // invert them to white to more easily create a multipier
    // only supports r==g==b brushes - Will need to be changed later
    private void invertImageColor()
    {
        for (int i = 0; i < m_image.Size.Height; i++)
        {
            for (int j = 0; j < m_image.Size.Width; j++)
            {
                Color c = m_image.GetPixel(j, i);
                Color inverted = Color.FromArgb(
                    c.A,
                    byte.MaxValue - c.R,
                    byte.MaxValue - c.G,
                    byte.MaxValue - c.B);
                m_image.SetPixel(j, i, inverted);
            }
        }
    }

    public Bitmap getImage()            { return m_image; }
    public void setMode(BlendMode mode) { m_mode = mode; }
    public BlendMode getMode()          { return m_mode; }
    public Size getSize()               { return new Size(m_image.Width, m_image.Height); }
}
