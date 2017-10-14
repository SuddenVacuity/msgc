using System;
using System.Windows.Forms;
using System.Drawing;

public class DialogNewLayer : SkinnedWindow
{
    TextBoxMarked m_textBoxName;
    TextBoxNumberMarked m_textBoxWidth;
    TextBoxNumberMarked m_textBoxHeight;

    SkinnedLabel m_labelInformation;
    SkinnedLabel m_labelName;
    SkinnedLabel m_labelWidth;
    SkinnedLabel m_labelHeight;

    SkinnedButton m_buttonConfirm;
    SkinnedButton m_buttonCancel;

    public string m_name = "";
    public int m_width = 0;
    public int m_height = 0;

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
        Point pointLabelName     = new Point(0,  155);
        Point pointLabelWidth    = new Point(0,  180);
        Point pointLabelHeight   = new Point(0,  205);
        Point pointTextBoxName   = new Point(60, 155);
        Point pointTextBoxWidth  = new Point(60, 180);
        Point pointTextBoxHeight = new Point(60, 205);
        Point pointButtonConfirm = new Point(80, 300);
        Point pointButtonCancel  = new Point(0,  300);

        // Text data
        string stringTitle = "Add Layer";
        string stringTextBoxEmpty = "Type Here";
        string stringConfirm = "Confirm";
        string stringCancel = "Cancel";
        string stringLabelInformation = "Create a new layer.WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW";
        string stringLabelName = "Name:";
        string stringLabelWidth = "Width:";
        string stringLabelHeight = "Height:";
        
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
        m_textBoxName.TextChanged += (s, e) =>
        {
            Console.Write("\nText Changed in layer name box");
            m_name = m_textBoxName.Text;
        };
        
        // Number input for width
        m_textBoxWidth = new TextBoxNumberMarked(stringTextBoxEmpty);
        m_textBoxWidth.Top  = thisPadding.Height + pointTextBoxWidth.Y;
        m_textBoxWidth.Left = thisPadding.Width + pointTextBoxWidth.X;
        m_textBoxWidth.TextChanged += (s, e) =>
        {
            Console.Write("\nText Changed in layer width box");
            int.TryParse(m_textBoxWidth.Text, out m_width);
        };

        // Number input for height
        m_textBoxHeight = new TextBoxNumberMarked(stringTextBoxEmpty);
        m_textBoxHeight.Top  = thisPadding.Height + pointTextBoxHeight.Y;
        m_textBoxHeight.Left = thisPadding.Width + pointTextBoxHeight.X;
        m_textBoxHeight.TextChanged += (s, e) =>
        {
            Console.Write("\nText Changed in layer height box");
            int.TryParse(m_textBoxHeight.Text, out m_height);
        };

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
            if (m_width > 0 && m_height > 0)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                Console.Write("\nCreate new project failed." +
                    "\n  values must be greater than 0 - width: " + m_width +
                    "\n  values must be greater than 0 - height: " + m_height);
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
