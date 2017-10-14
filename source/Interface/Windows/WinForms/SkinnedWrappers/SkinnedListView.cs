using System;
using System.Windows.Forms;
using System.Drawing;

public class SkinnedListView : ListView
{
    /*
    public SkinnedListView()
    {
        this.Text = "List View Text";
        this.Name = "List View";

        this.Size = InterfaceSkin.ListViewSizeNormal;

        this.BackColor = InterfaceSkin.ListViewColorBackground;
        this.ForeColor = InterfaceSkin.ListViewColorText;

        this.ShowGroups = true;
        this.HeaderStyle = ColumnHeaderStyle.Nonclickable;
        this.AllowColumnReorder = false;
        this.View = View.Details;
        this.MultiSelect = false;
        this.FullRowSelect = true;
        
        this.OwnerDraw = true;

        // hacky but the only reasonable way I could find 
        // to control height without using an third party wrapper
        ImageList imglst = new ImageList();
        imglst.ImageSize = new Size(1, InterfaceSkin.ListViewItemHeight);
        this.SmallImageList = imglst;

    }

    protected override void OnColumnWidthChanging(ColumnWidthChangingEventArgs e)
    {
        // prevent column resizing
        // TODO: fix issue - keyboard shortcuts for resize still work
        e.Cancel = true;
        e.NewWidth = this.Columns[e.ColumnIndex].Width;
        //base.OnColumnWidthChanging(e);
    }
    
    // this runs once per column in the listview's header
    protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
    {
        //Color backgroundColor = InterfaceSkin.ListViewColorHeaderBackground;
        //Color textColor = InterfaceSkin.ListViewColorHeaderText;
        Color backgroundColor = Color.Blue;
        Color textColor = Color.White;
        Font font = SystemFonts.DefaultFont;
        int textWidth = 2;
    
        //e.Graphics.FillRectangle(new SolidBrush(backgroundColor), this.Bounds);
    
        e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
    
        string s = e.Header.Text;
        Rectangle rec = this.Bounds;
    
        // switch from relative to parents position
        // to relative to its own position
        rec.X = 0;
        rec.Y = 0;

        // add up the width of all previous columns
        int id = e.ColumnIndex;
        for (int i = 0; i < id; i++)
            rec.X += this.Columns[i].Width;
    
        e.Graphics.FillRectangle(new SolidBrush(backgroundColor), rec);
    
        // offset for text position
        rec.X += 3;
        rec.Y += 5;
    
        SizeF length = e.Graphics.MeasureString(s, font, textWidth);
        e.Graphics.DrawString(s, font, new SolidBrush(textColor), rec.Location);
    }

    // this runs once per item in the listview
    protected override void OnDrawItem(DrawListViewItemEventArgs e)
    {
        //Color backgroundColor = InterfaceSkin.ListViewItemColorBackground;
        //Color textColor = InterfaceSkin.ListViewItemColorText;
        //Font font = SystemFonts.DefaultFont;
        //int textWidth = 2;
        //
        ////e.Graphics.FillRectangle(new SolidBrush(backgroundColor), this.Bounds);
        //e.Graphics.Clear(backgroundColor);
        //e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
        //
        //Point position = new Point(0, 0);
        //
        //string s = e.Item.Text;
        //
        //SizeF length = e.Graphics.MeasureString(s, font, textWidth);
        //Point location = new Point();
        //location.X = position.X + 3;
        //location.Y = position.Y + 5;
        //e.Graphics.DrawString(s, font, new SolidBrush(textColor), location);

        Color backgroundColor = Color.Green;
        Color textColor = Color.White;
        Font font = SystemFonts.DefaultFont;
        int textWidth = 2;


        e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

        int id = e.ItemIndex;
        string s = e.Item.Text;

        Rectangle rec = this.Bounds;

        // switch from relative to parents position
        // to relative to its own position
        rec.X = e.Item.Position.X;
        rec.Y = e.Item.Position.Y;
        rec.Width = this.Columns[0].Width;
        rec.Height = InterfaceSkin.ListViewItemHeight;

        e.Graphics.FillRectangle(new SolidBrush(backgroundColor), rec);

        rec.X += 3;
        rec.Y += 5;

        //SizeF length = e.Graphics.MeasureString(s, font, textWidth);
        e.Graphics.DrawString(s, font, new SolidBrush(textColor), rec.Location);

        // draw the sting contained in each subitem
        // add up the width of all previous columns
        int count = e.Item.SubItems.Count;
        for (int i = 0; i < count; i++)
        {
            if(i > 0)
                rec.X += this.Columns[i - 1].Width;

            e.Graphics.FillRectangle(new SolidBrush(backgroundColor), rec);

            string str = e.Item.Text;
            //SizeF length = e.Graphics.MeasureString(str, font, textWidth);
            e.Graphics.DrawString(str, font, new SolidBrush(textColor), rec.Location);
        }
        //this.OwnerDraw = false;
        //base.OnDrawItem(e);
        //this.OwnerDraw = true;
    }
    // */
}
