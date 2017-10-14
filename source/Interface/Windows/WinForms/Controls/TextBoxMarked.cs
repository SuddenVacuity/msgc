using System;
using System.Windows.Forms;
using System.Drawing;

/// <summary>
/// A textbox that displays text when inactive and empty.
/// </summary>
public class TextBoxMarked : SkinnedTextBox
{
    // this is the text that displays when no valid text is input
    public string m_watermark = " Type here...";
    // bool for wether or not the text displayed in the box is allowed to change
    public bool m_allowChangeText = true;

    public TextBoxMarked(string watermark)
    {
        m_watermark = watermark;
        this.BackColor = InterfaceSkin.TextBoxColorBackground;
        this.ForeColor = InterfaceSkin.TextBoxColorTextFaded;

        //Font font = new Font(this.Font, FontStyle.Italic);
        //this.Font = font;

        this.Text = m_watermark;
    }
    public TextBoxMarked()
    {
        this.BackColor = InterfaceSkin.TextBoxColorBackground;
        this.ForeColor = InterfaceSkin.TextBoxColorTextFaded;

        //Font font = new Font(this.Font, FontStyle.Italic);
        //this.Font = font;

        this.Text = m_watermark;
    }

    protected override void OnGotFocus(EventArgs e)
    {
        if(m_allowChangeText == true &&
            this.Text == m_watermark)
            this.Text = "";

        this.ForeColor = InterfaceSkin.TextBoxColorText;

        //Font font = new Font(this.Font, FontStyle.Regular);
        //this.Font = font;

        base.OnGotFocus(e);
    }
    protected override void OnLostFocus(EventArgs e)
    {
        if (this.Text.Length == 0)
        {
            m_allowChangeText = true;
            this.ForeColor = InterfaceSkin.TextBoxColorTextFaded;

            //Font font = new Font(this.Font, FontStyle.Italic);
            //this.Font = font;

            this.Text = m_watermark;
        }
        
        base.OnLostFocus(e);
    }
    protected override void OnTextChanged(EventArgs e)
    {
        if (m_allowChangeText == true)
            base.OnTextChanged(e);
    }
}
