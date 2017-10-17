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
    /// <summary>
    /// Sets values to default and disposes of any images the layer contains.
    /// </summary>
    public void clearLayer()
    {
        m_id = int.MinValue;
        m_name = "";

        if (m_fragment != null)
        {
            m_fragment.clear();
            m_fragment = null;
        }
    }

    /// <summary>
    /// Stores a copy of the image. 
    /// Returns truw on success.
    /// </summary>
    /// <param name="image"></param>
    public bool setImage(Bitmap image) { return m_fragment.setImage(image); }
    /// <summary>
    /// Returns a handle to the stored image. 
    /// Returns null if layer has no fragment to hold the image.
    /// </summary>
    /// <returns></returns>
    public Bitmap getImage()
    {
        if (m_fragment == null)
            return null;

        return m_fragment.getImage();
    }
    public int getId() { return m_id; }
    public void setId(int id) { m_id = id; }
    public void setIsVisible(bool visible) { m_isVisible = visible; }
    public bool getIsVisible() { return m_isVisible; }

    /// <summary>
    /// Resource Intensive - AVOID USING. Returns the color of the pixel at x, y coordinates. 
    /// Returns Color.Empty if failed.
    /// </summary>
    public Color getPixel(int x, int y) { return m_fragment.getPixel(x, y); }
    /// <summary>
    /// Resource Intensive - AVOID USING. Returns the color of the pixel at x, y coordinates. 
    /// Returns Color.Empty if failed.
    /// </summary>
    public Color getPixel(Point point) { return m_fragment.getPixel(point.X, point.Y); }
    /// <summary>
    /// Resource Intensive - AVOID USING. Returns true if the pixel was successfully set. 
    /// </summary>
    public bool setPixel(int x, int y, Color color) { return m_fragment.setPixel(x, y, color); }
    /// <summary>
    /// Resource Intensive - AVOID USING. Returns true if the pixel was successfully set. 
    /// </summary>
    public bool setPixel(Point point, Color color) { return m_fragment.setPixel(point.X, point.Y, color); }
    /// <summary>
    /// Returns a copy of the region the image in the fragment covers. 
    /// Returns Rectangle.Empty if failed.
    /// </summary>
    /// <returns></returns>
    public Rectangle getLayerRegion() { return m_fragment.getRegion(); }
    /// <summary>
    /// Returns a copy of the size of the image contained within the canvas. 
    /// Returns Size.Empty if failed.
    /// </summary>
    public Size getImageSize() { return m_fragment.getSize(); }
    public bool hasFragment() { return (m_fragment != null); }

    public void addFlag(int flag) { m_flags.add(flag); }
    public void removeFlag(int flag) { m_flags.remove(flag); }
    public bool hasFlag(int flag) { return m_flags.has(flag); }
    public string getName() { return m_name; }
    /// <summary>
    /// Sets the layer region. region must have width and height great than 0. 
    /// Returns true on success.
    /// </summary>
    public bool setRegion(Rectangle region) { return m_fragment.setRegion(region); }
}