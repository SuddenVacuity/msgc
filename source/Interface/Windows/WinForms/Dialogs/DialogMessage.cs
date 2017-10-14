using System;
using System.Windows.Forms;
using System.Drawing;

public class DialogMessage : SkinnedWindow
{
    SkinnedLabel m_labelText;
    SkinnedButton m_buttonClose;

    public DialogMessage(string title, string text, int width = 300, int height = 200)
    {
        ////////////////////////////
        // Define the design data //
        ////////////////////////////

        // Main Window
        Size thisSize = new Size(width + 35, height + 100);
        Size thisPadding = this.Padding.Size;

        // custom-sized elements
        Rectangle rectLabelText = new Rectangle(
            thisPadding.Width,
            thisPadding.Height,
            width, 
            height);

        // standard-sized elements
        Point pointButtonClose = new Point(
            thisPadding.Width + (width / 2) - 50,
            thisPadding.Height + height + 20);

        // Text Data
        string stringTitle = title;
        string stringLabelText = text;
        string stringButtonClose = "Close";

        ////////////////////////////////////////
        // create controls and apply the data //
        ////////////////////////////////////////

        // Main Window
        this.ShowInTaskbar = false;
        this.Text = stringTitle;
        this.Width = thisSize.Width;
        this.Height = thisSize.Height;
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        this.AutoSize = true;

        // Text label
        m_labelText = new SkinnedLabel();
        m_labelText.Top = rectLabelText.Y;
        m_labelText.Left = rectLabelText.X;
        m_labelText.Width = rectLabelText.Width;
        m_labelText.Height = rectLabelText.Height;
        m_labelText.Text = stringLabelText;
        
        // Close button
        m_buttonClose = new SkinnedButton();
        m_buttonClose.Top = pointButtonClose.Y;
        m_buttonClose.Left = pointButtonClose.X;
        m_buttonClose.Text = stringButtonClose;
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
}
