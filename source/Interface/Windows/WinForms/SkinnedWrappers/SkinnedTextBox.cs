using System.Windows.Forms;

public class SkinnedTextBox : TextBox
{
    public SkinnedTextBox()
    {
        this.Text = "Text Box Text";
        this.Name = "Text Box";
        this.Size = InterfaceSkin.TextBoxSizeNormal;
        this.BorderStyle = BorderStyle.FixedSingle;
        this.BackColor = InterfaceSkin.TextBoxColorBackground;
        this.ForeColor = InterfaceSkin.TextBoxColorText;
        this.Margin = InterfaceSkin.TextBoxMargin;
    }
}
