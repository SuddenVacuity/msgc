using System.Windows.Forms;

public class SkinnedPanel : Panel
{
    public SkinnedPanel()
    {
        this.Text = "Panel Text";
        this.Name = "Panel";
        this.Size = InterfaceSkin.PanelSizeNormal;
        this.BackColor = InterfaceSkin.PanelColorBackground;
        this.ForeColor = InterfaceSkin.PanelColorText;
        this.BorderStyle = BorderStyle.FixedSingle;
        this.RightToLeft = RightToLeft.No;
    }
}
