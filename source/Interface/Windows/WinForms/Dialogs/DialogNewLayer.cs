using System;
using System.Windows.Forms;
using System.Drawing;

public class DialogNewLayer : SkinnedWindow
{
    // The data returned by the dialog
    public string m_name = "";
    public int m_width = 0;
    public int m_height = 0;

    TextBoxMarked m_textBoxName;
    TextBoxMarked m_textBoxWidth;
    TextBoxMarked m_textBoxHeight;

    SkinnedLabel m_labelInformation;
    SkinnedLabel m_labelName;
    SkinnedLabel m_labelWidth;
    SkinnedLabel m_labelHeight;

    SkinnedButton m_buttonConfirm;
    SkinnedButton m_buttonCancel;
    
    public DialogNewLayer()
    {
        ////////////////////////////
        // Define the design data //
        ////////////////////////////

        // Main Window
        Size thisSize = new Size(10, 20);
        Size thisPadding = this.Padding.Size;

        // custom-sized elements
        Rectangle rectLabelInformation = new Rectangle(
            0,
            0, 
            160, 
            150);

        // standard-sized elements
        Point pointLabelName     = new Point(10,  155);
        Point pointLabelWidth    = new Point(10,  180);
        Point pointLabelHeight   = new Point(10,  205);
        Point pointTextBoxName   = new Point(70, 155);
        Point pointTextBoxWidth  = new Point(70, 180);
        Point pointTextBoxHeight = new Point(70, 205);
        Point pointButtonConfirm = new Point(80, 300);
        Point pointButtonCancel  = new Point(0,  300);

        // Text data
        string stringTitle = "Add Layer";
        string stringLabelInformation = "Create a new layer.";
        string stringLabelName = "Name:";
        string stringLabelWidth = "Width:";
        string stringLabelHeight = "Height:";
        string stringTextBoxEmpty = "Type Here...";
        string stringConfirm = "Confirm";
        string stringCancel = "Cancel";
        string stringErrorInvalidInputTitle = "Invalid Input";
        string stringErrorInvalidInput  = "Error creating new layer:";
        string stringErrorInvalidInputName   = "\n   - Name is not valid <-- THIS SHOULD NOT APPEAR";
        string stringErrorInvalidInputWidth  = "\n   - Width must be greater than 0\n      and only contain characters 0~9";
        string stringErrorInvalidInputHeight = "\n   - Height must be greater than 0\n      and only contain characters 0~9";

        ////////////////////////////////////////
        // create controls and apply the data //
        ////////////////////////////////////////

        // Main Window
        this.ShowInTaskbar = false;
        this.Text = stringTitle;
        this.Width = thisSize.Width;
        this.Height = thisSize.Height;
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        this.ShowInTaskbar = false;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        this.AutoSize = true;

        // Text input for name
        m_textBoxName = new TextBoxMarked(stringTextBoxEmpty);
        m_textBoxName.Top  = thisPadding.Height + pointTextBoxName.Y;
        m_textBoxName.Left = thisPadding.Width + pointTextBoxName.X;
        
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

        // Label for name input
        m_labelName = new SkinnedLabel();
        m_labelName.Top  = thisPadding.Height + pointLabelName.Y;
        m_labelName.Left = thisPadding.Width + pointLabelName.X;
        m_labelName.Text = stringLabelName;
        m_labelName.Padding = new Padding(2);

        // Label for width input box
        m_labelWidth = new SkinnedLabel();
        m_labelWidth.Top  = thisPadding.Height + pointLabelWidth.Y;
        m_labelWidth.Left = thisPadding.Width + pointLabelWidth.X;
        m_labelWidth.Text = stringLabelWidth;
        m_labelWidth.Padding = new Padding(2);

        // Label for height input box
        m_labelHeight = new SkinnedLabel();
        m_labelHeight.Top  = thisPadding.Height + pointLabelHeight.Y;
        m_labelHeight.Left = thisPadding.Width + pointLabelHeight.X;
        m_labelHeight.Text = stringLabelHeight;
        m_labelHeight.Padding = new Padding(2);

        // Confirm button
        m_buttonConfirm = new SkinnedButton();
        m_buttonConfirm.Top  = thisPadding.Height + pointButtonConfirm.Y;
        m_buttonConfirm.Left = thisPadding.Width + pointButtonConfirm.X;
        m_buttonConfirm.Text = stringConfirm;
        m_buttonConfirm.Click += (vvv, bbb) =>
        {
            bool validName = false;
            bool validWidth = false;
            bool validHeight = false;

            if (m_textBoxName.Text != null)
            {
                m_name = m_textBoxName.Text;
                validName = true;
            }
            if (int.TryParse(m_textBoxWidth.Text, out m_width))
                if(m_width > 0)
                    validWidth = true;
            if (int.TryParse(m_textBoxHeight.Text, out m_height))
                if(m_height > 0)
                    validHeight = true;

            if (validName == true &&
                validWidth == true &&
                validHeight == true)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                string message = stringErrorInvalidInput;

                if (validName == false)
                    message += stringErrorInvalidInputName;
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
        this.Controls.Add(m_labelName);
        this.Controls.Add(m_textBoxName);
        this.Controls.Add(m_labelWidth);
        this.Controls.Add(m_textBoxWidth);
        this.Controls.Add(m_labelHeight);
        this.Controls.Add(m_textBoxHeight);
        this.Controls.Add(m_buttonCancel);
        this.Controls.Add(m_buttonConfirm);
    }

    protected override void OnLoad(EventArgs e)
    {
        this.ActiveControl = m_labelInformation;
        base.OnLoad(e);
    }

}
