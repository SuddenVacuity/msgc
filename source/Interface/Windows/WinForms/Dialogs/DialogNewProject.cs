using System;
using System.Windows.Forms;
using System.Drawing;

public class DialogNewProject : SkinnedWindow
{
    // The data returned by the dialog
    public int m_width = 0;
    public int m_height = 0;

    TextBoxMarked m_textBoxWidth;
    TextBoxMarked m_textBoxHeight;

    SkinnedLabel m_labelInformation;
    SkinnedLabel m_labelWidth;
    SkinnedLabel m_labelHeight;

    SkinnedButton m_buttonConfirm;
    SkinnedButton m_buttonCancel;

    public DialogNewProject()
    {
        ////////////////////////////
        // Define the design data //
        ////////////////////////////

        // Main Window
        Size thisSize = new Size(500, 300);
        Size thisPadding = this.Padding.Size;

        // custom-sized elements
        Rectangle rectLabelInformation = new Rectangle(
            0, 
            0, 
            thisSize.Width - (this.Padding.Left + this.Padding.Right), 
            100);

        // standard-sized elements
        Point pointTextBoxWidth  = new Point(225, 120);                                                             
        Point pointTextBoxHeight = new Point(225, 150);
        Point pointLabelWidth    = new Point(175, 120);
        Point pointLabelHeight   = new Point(175, 150);
        Point pointButtonConfirm = new Point(150, 220);
        Point pointButtonCancel  = new Point(250, 220);

        // Text data
        string stringTitle = "New Project";
        string stringTextBoxEmpty = "Type Here";
        string stringConfirm = "Confirm";
        string stringCancel = "Cancel";
        string stringLabelInformation = "Enter the size of the new project in pixels.";
        string stringLabelWidth = "Width:";
        string stringLabelHeight = "Height:";
        string stringErrorInvalidInputTitle  = "Invalid Input";
        string stringErrorInvalidInput       = "Error creating new project:";
        string stringErrorInvalidInputWidth  = "\n   - Width must be greater than 0\n      and only contain characters 0~9";
        string stringErrorInvalidInputHeight = "\n   - Height must be greater than 0\n      and only contain characters 0~9";

        /////////////////////////////
        //   Set the form values   //
        /////////////////////////////
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

        ////////////////////////////
        // Apply data to controls //
        ////////////////////////////
        // Number input for width
        m_textBoxWidth = new TextBoxMarked(stringTextBoxEmpty);
        m_textBoxWidth.Top  = thisPadding.Height + pointTextBoxWidth.Y;
        m_textBoxWidth.Left = thisPadding.Width + pointTextBoxWidth.X;

        // Number input for height
        m_textBoxHeight = new TextBoxMarked(stringTextBoxEmpty);
        m_textBoxHeight.Top  = thisPadding.Height + pointTextBoxHeight.Y;
        m_textBoxHeight.Left = thisPadding.Width + pointTextBoxHeight.X;

        // Informational text
        m_labelInformation = new SkinnedLabel();
        m_labelInformation.Top  = thisPadding.Height + rectLabelInformation.Y;
        m_labelInformation.Left = thisPadding.Width + rectLabelInformation.X;
        m_labelInformation.Width = rectLabelInformation.Width;
        m_labelInformation.Height = rectLabelInformation.Height;
        m_labelInformation.Text = stringLabelInformation;

        // Label for width input box
        m_labelWidth = new SkinnedLabel();
        m_labelWidth.Top  = thisPadding.Height + pointLabelWidth.Y;
        m_labelWidth.Left = thisPadding.Width + pointLabelWidth.X;
        m_labelWidth.Text = stringLabelWidth;

        // Label for height input box
        m_labelHeight = new SkinnedLabel();
        m_labelHeight.Top  = thisPadding.Height + pointLabelHeight.Y;
        m_labelHeight.Left = thisPadding.Width + pointLabelHeight.X;
        m_labelHeight.Text = stringLabelHeight;
        
        // Confirm button
        m_buttonConfirm = new SkinnedButton();
        m_buttonConfirm.Top  = thisPadding.Height + pointButtonConfirm.Y;
        m_buttonConfirm.Left = thisPadding.Width + pointButtonConfirm.X;
        m_buttonConfirm.Text = stringConfirm;
        m_buttonConfirm.Click += (vvv, bbb) =>
        {
            bool validWidth = false;
            bool validHeight = false;
            
            if (int.TryParse(m_textBoxWidth.Text, out m_width))
                if (m_width > 0)
                    validWidth = true;
            if (int.TryParse(m_textBoxHeight.Text, out m_height))
                if (m_height > 0)
                    validHeight = true;

            if (validWidth == true &&
                validHeight == true)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                string message = stringErrorInvalidInput;
                
                if (validWidth == false)
                    message += stringErrorInvalidInputWidth;
                if (validHeight == false)
                    message += stringErrorInvalidInputHeight;

                DialogMessage error = new DialogMessage(stringErrorInvalidInputTitle, message);
                error.ShowDialog();
            }
        };

        // Cancel Button
        m_buttonCancel = new SkinnedButton();
        m_buttonCancel.Top  = thisPadding.Height + pointButtonCancel.Y;
        m_buttonCancel.Left = thisPadding.Width + pointButtonCancel.X;
        m_buttonCancel.Text = stringCancel;
        m_buttonCancel.Click += (vvv, bbb) =>
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        };

        //////////////////////////////////
        // Add the controls to the form //
        //////////////////////////////////

        this.Controls.Add(m_labelInformation);
        this.Controls.Add(m_textBoxWidth);
        this.Controls.Add(m_textBoxHeight);
        this.Controls.Add(m_labelWidth);
        this.Controls.Add(m_labelHeight);
        this.Controls.Add(m_buttonConfirm);
        this.Controls.Add(m_buttonCancel);
    } // END public DialogNewProject()

    protected override void OnLoad(EventArgs e)
    {
        this.ActiveControl = m_labelInformation;
        base.OnLoad(e);
    }
}
