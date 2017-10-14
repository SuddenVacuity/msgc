using System.Windows.Forms;

public class SkinnedToolStripItem : ToolStripMenuItem
{
    public SkinnedToolStripItem()
    {
        this.Text = "Tool Text";
        this.Name = "Tool";
        this.Size = InterfaceSkin.ToolStripItemSizeNormal;

        // more detailed color options are in CustomColorTable.cs
        this.ForeColor = InterfaceSkin.ToolStripItemColorText;
    }
}