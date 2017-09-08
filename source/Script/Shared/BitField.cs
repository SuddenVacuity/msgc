
public class BitField
{
    private int m_flags;

    public BitField()
    {
        m_flags = 0;
    }
    public BitField(int flags)
    {
        m_flags = flags;
    }

    public int get() { return m_flags; }
    public void set(int flags) { m_flags = flags; }
    public void add(int flags) { m_flags |= flags; }
    public void remove(int flags) { m_flags -= (m_flags & flags); }
    public bool has(int flags) { return (m_flags & flags) == flags; }
}
