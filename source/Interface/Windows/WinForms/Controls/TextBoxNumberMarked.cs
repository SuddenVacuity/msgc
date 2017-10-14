using System;
using System.Windows.Forms;
using System.Drawing;


public class TextBoxNumberMarked : TextBoxMarked
{
    public TextBoxNumberMarked(string watermark)
    {
        m_watermark = watermark;
        this.Text = watermark;
    }
    public TextBoxNumberMarked()
    {
        this.Text = m_watermark;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        int key = e.KeyValue;

        if (key > '9' || key < '0')
            e.SuppressKeyPress = true;
    }
}
