using System;
using System.Windows.Forms;
using System.Drawing;

public class SkinnedLabel : Label
{
    public SkinnedLabel()
    {
        this.Text = "Label Text";
        this.Name = "Label";
        this.AutoSize = false;
        this.Size = InterfaceSkin.LabelSizeNormal;
        this.BackColor = InterfaceSkin.LabelColorBackground;
        this.ForeColor = InterfaceSkin.LabelColorText;
        this.Margin = InterfaceSkin.LabelMargin;
        this.TextAlign = ContentAlignment.TopLeft;
        this.Padding = new Padding(10);
    }
    
}
