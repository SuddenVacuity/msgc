using System.Windows.Forms;

public class SkinnedMenuStrip : MenuStrip
{
    public SkinnedMenuStrip()
    {
        this.Text = "Menu Strip Text";
        this.Name = "Menu Strip";
        this.Size = InterfaceSkin.MenuStripSizeNormal;
        this.Renderer = new CustomToolStripRenderer(new CustomColorTable());
        this.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

                        
        

        //this.menuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
        //this.fileToolStripMenuItem,
        //this.editToolStripMenuItem,
        //this.viewToolStripMenuItem,
        //this.projectToolStripMenuItem,
        //this.selectionToolStripMenuItem,
        //this.layerToolStripMenuItem,
        //this.toolsToolStripMenuItem,
        //this.windowToolStripMenuItem,
        //this.helpToolStripMenuItem});
        //this.menuStrip1.Location = new System.Drawing.Point(10, 10);
        //this.menuStrip1.Name = "menuStrip1";
        //this.menuStrip1.Size = new System.Drawing.Size(1037, 24);
        //this.menuStrip1.TabIndex = 0;
        //this.menuStrip1.Text = "menuStrip1"; 
    }
}