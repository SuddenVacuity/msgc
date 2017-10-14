using System;
using System.Windows.Forms;
using System.Drawing;

public class CustomToolStripRenderer : ToolStripProfessionalRenderer
{
    public CustomToolStripRenderer(CustomColorTable customColorTable):
        base(customColorTable)
    {
    }

    protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
    {
        var tsMenuItem = e.Item as ToolStripMenuItem;

        if (tsMenuItem != null)
            e.ArrowColor = InterfaceSkin.ToolStripItemColorText;

        base.OnRenderArrow(e);
    }
}