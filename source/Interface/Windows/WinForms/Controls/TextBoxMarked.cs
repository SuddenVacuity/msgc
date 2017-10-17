using System;
using System.Windows.Forms;
using System.Drawing;

/// <summary>
/// A textbox that displays text when inactive and empty. 
/// Will store/return empty strings in place of null strings
/// </summary>
public class TextBoxMarked : SkinnedTextBox
{
    // this is the text that displays when no valid text is input
    private string m_watermark = " Type here...";

    // bool for if the onTextChanged event should do anything
    public bool m_allowChangeText = true;

    public TextBoxMarked()
    {
        this.BackColor = InterfaceSkin.TextBoxColorBackground;
        this.ForeColor = InterfaceSkin.TextBoxColorTextFaded;

        this.Text = m_watermark;
        m_allowChangeText = false;
    }
    public TextBoxMarked(string watermark)
    {
        setWatermark(watermark);

        this.Text = m_watermark;
        
        this.BackColor = InterfaceSkin.TextBoxColorBackground;
        this.ForeColor = InterfaceSkin.TextBoxColorTextFaded;
        
        m_allowChangeText = false;
    }

    /// <summary>
    /// Creates a copy of watermark string and set it to the control's Text
    /// </summary>
    private void setWatermark(string watermark)
    {
        if (watermark == null)
            m_watermark = "";
        else
            m_watermark = watermark;
    }

    /// <summary>
    /// Sets input state to allow input and removes watermark
    /// </summary>
    protected override void OnGotFocus(EventArgs e)
    {
        m_allowChangeText = true;

        if (this.Text == m_watermark)
        {
            this.Text = "";
            this.ForeColor = InterfaceSkin.TextBoxColorText;
        }

        base.OnGotFocus(e);
    }
    /// <summary>
    /// Sets input state to don't allow input and adds watermark if input string is empty
    /// </summary>
    protected override void OnLostFocus(EventArgs e)
    {
        if (this.Text.Length == 0)
        {
            // setting to false must happen before changing text
            m_allowChangeText = false;
            this.Text = m_watermark;
            this.ForeColor = InterfaceSkin.TextBoxColorTextFaded;
        }
        
        base.OnLostFocus(e);
    }
    protected override void OnTextChanged(EventArgs e)
    {
        // prevent onchanged event from running when 
        // the text is changed to the watermark
        if (m_allowChangeText == false)
            return;

        base.OnTextChanged(e);
    }

    public override string Text
    {
        get
        {
            // don't return a null string
            if (base.Text == m_watermark && m_allowChangeText == false)
                return "";
    
            return base.Text;
        }
    
        set
        {
            // don't accept a null string
            if (value == null)
                base.Text = "";
            else
                base.Text = value;
        }
    }
}
