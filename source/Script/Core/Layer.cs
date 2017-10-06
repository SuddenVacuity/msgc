using System.Drawing;



// stores canvas fragments and blending data
public class Layer
{
    private BitField m_flags = new BitField();
    private int m_id = int.MinValue;
    private string m_name = "";
    private CanvasFragment m_fragment;
    private bool m_isVisible = true;
    //Vector
    //Filter

    public Layer(int id, string name, CanvasFragment fragment, int flags = 0)
    {
        m_flags.set(flags);
        m_id = id;
        m_name = name;
        m_fragment = fragment;
        m_isVisible = true;
    }
    public void clearLayer()
    {
        m_id = int.MinValue;
        m_name = "";
        m_fragment.clear();
    }

    public void resizeImage(Point location, Size size)
    {
        Point currentLocation = m_fragment.getPosition();
        Point offset = new Point(0, 0);

        if (location.X < currentLocation.X)
            offset.X = currentLocation.X - location.X;
        if (location.Y < currentLocation.Y)
            offset.Y = currentLocation.Y - location.Y;

        m_fragment.setPosition(location.X, location.Y);
        m_fragment.resize(size.Width, size.Height, offset);
    }

    /// <summary>
    /// Stores a copy of the image.
    /// </summary>
    /// <param name="image"></param>
    public void setImage(Bitmap image) { m_fragment.setImage(image); }
    /// <summary>
    /// Returns a handle to the stored image.
    /// </summary>
    /// <returns></returns>
    public Bitmap getImage() { return m_fragment.getImage(); }
    public int getId() { return m_id; }
    public void setId(int id) { m_id = id; }
    public void setIsVisible(bool visible) { m_isVisible = visible; }
    public bool getIsVisible() { return m_isVisible; }
    public Color getPixel(int x, int y) { return m_fragment.getPixel(x, y); }
    public Color getPixel(Point point) { return m_fragment.getPixel(point.X, point.Y); }
    public void setPixel(int x, int y, Color color) { m_fragment.setPixel(x, y, color); }
    public void setPixel(Point point, Color color) { m_fragment.setPixel(point.X, point.Y, color); }
    public Point getImagePosition() { return m_fragment.getPosition(); }
    public Size getImageSize() { return m_fragment.getSize(); }
    public Rectangle getRectangle() { return new Rectangle(m_fragment.getPosition(), m_fragment.getSize()); }

    public void addFlag(int flag) { m_flags.add(flag); }
    public void removeFlag(int flag) { m_flags.remove(flag); }
    public bool hasFlag(int flag) { return m_flags.has(flag); }
}