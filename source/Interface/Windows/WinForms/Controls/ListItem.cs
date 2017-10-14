using System;
using System.Windows.Forms;
using System.Drawing;

public class ListItem : Panel
{
    public int m_id;
    public string m_text;
    private bool m_selected;

    private bool m_buttonOn;

    public event EventHandler ListItemButtonClick;

    public ListItem()
    {
        this.Size = InterfaceSkin.LayerListItemSize;
        this.BackColor = InterfaceSkin.LayerListItemColorBackground;
        this.ForeColor = InterfaceSkin.LayerListItemColorText;

        Label button = new Label();
        button.Size = new Size(15, 15);
        button.Location = new Point(5, 4);
        button.BackColor = Color.White;

        button.MouseUp += (s, e) =>
        {
            m_buttonOn = !m_buttonOn;

            EventHandler handler = ListItemButtonClick;
            if (handler != null)
                handler(this, e);

            if (m_buttonOn == false)
                button.BackColor = Color.White;
            else
                button.BackColor = Color.Black;

        };
        button.MouseEnter += (s, e) =>
        {
            if (m_selected != true)
                this.BackColor = InterfaceSkin.LayerListItemColorBackgroundHiglight;
        };
        button.MouseLeave += (s, e) =>
        {
            if (m_selected != true)
                this.BackColor = InterfaceSkin.LayerListItemColorBackground;
        };

        this.Controls.Add(button);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        int textOffset = 30;
        Color borderColor = InterfaceSkin.LayerListItemColorBorder;

        e.Graphics.Clear(this.BackColor);

        Rectangle rec = this.Bounds;
        rec.X = 0;
        rec.Y = 0;
        rec.Width -= 1;
        rec.Height -= 1;
        e.Graphics.DrawRectangle(new Pen(new SolidBrush(borderColor)), rec);

        SizeF length = e.Graphics.MeasureString(this.m_text, this.Font, this.Width);
        Point location = new Point();
        location.X = textOffset;
        location.Y = (int)((this.Height / 2) - (length.Height / 2));
        e.Graphics.DrawString(this.m_text, Font, new SolidBrush(this.ForeColor), location);

        // draw Button
        Label b = this.Controls[0] as Label;
        e.Graphics.DrawRectangle(new Pen(new SolidBrush(b.BackColor)), new Rectangle(b.Location, b.Size));
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        if(m_selected != true)
            this.BackColor = InterfaceSkin.LayerListItemColorBackgroundHiglight;

        base.OnMouseEnter(e);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        if (m_selected != true)
            this.BackColor = InterfaceSkin.LayerListItemColorBackground;

        base.OnMouseLeave(e);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (this.m_selected == true)
            return;

        select();
        base.OnMouseDown(e);
    }

    /// <summary>
    /// sets selected flag to false and changes colors to normal
    /// </summary>
    public void deselect()
    {
        this.m_selected = false;
        this.BackColor = InterfaceSkin.LayerListItemColorBackground;
        this.ForeColor = InterfaceSkin.LayerListItemColorText;
    }
    
    /// <summary>
    /// sets selecteed tag to true and changes colors to selected colors
    /// </summary>
    private void select()
    {
        this.m_selected = true;
        this.BackColor = InterfaceSkin.LayerListItemColorBackgroundSelected;
        this.ForeColor = InterfaceSkin.LayerListItemColorTextSelected;
    }

}
