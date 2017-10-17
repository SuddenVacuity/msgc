using System;
using System.Windows.Forms;
using System.Drawing;

public class DialogMessage : SkinnedWindow
{
    SkinnedLabel m_labelText;
    SkinnedButton m_buttonClose;

    public DialogMessage(string title, string text)
    {
        ////////////////////////////
        // Define the design data //
        ////////////////////////////
        // Text Data
        string stringTitle = title;
        string stringLabelText = text;
        string stringButtonText = "Close";

        /////////////////////////////
        //   Set the form values   //
        /////////////////////////////
        this.ShowInTaskbar = false;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        this.Text = stringTitle;
        this.Padding = new Padding(
            this.Padding.Left + 10,
            this.Padding.Top,
            this.Padding.Right + 10,
            this.Padding.Bottom);

        ////////////////////////////
        // Apply data to controls //
        ////////////////////////////
        // Text label
        m_labelText = new SkinnedLabel();
        m_labelText.AutoSize = true;
        m_labelText.Top  = this.Padding.Top;
        m_labelText.Left = this.Padding.Left;
        m_labelText.Text = stringLabelText;
        
        // Close button
        m_buttonClose = new SkinnedButton();
        m_buttonClose.Text = stringButtonText;
        m_buttonClose.Click += (vvv, bbb) =>
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        };

        //////////////////////////////////
        // Add the controls to the form //
        //////////////////////////////////
        this.Controls.Add(m_labelText);
        this.Controls.Add(m_buttonClose);
    }

    protected override void OnLoad(EventArgs e)
    {
        this.ActiveControl = m_buttonClose;
        base.OnLoad(e);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);

        // place the button under the label 
        // and in the center of the form
        if (m_buttonClose != null)
        {
            m_buttonClose.Top = m_labelText.Top + m_labelText.Height + 10;
            m_buttonClose.Left = (this.ClientSize.Width - m_buttonClose.Width) / 2;
        }
    }
}
