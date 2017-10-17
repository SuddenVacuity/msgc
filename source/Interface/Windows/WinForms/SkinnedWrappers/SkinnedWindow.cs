using System.Windows.Forms;

public class SkinnedWindow : Form
{
    public SkinnedWindow()
    {
        this.Name = "Window";
        this.Text = "Window Text";
        this.BackColor = InterfaceSkin.WindowColorBackground;
        this.ForeColor = InterfaceSkin.WindowColorText;
        this.Padding = InterfaceSkin.WindowPadding;
        this.FormBorderStyle = FormBorderStyle;
        this.AutoSize = true;
    }
}
