using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

public class SkinnedButton : Button
{
    public SkinnedButton()
    {
        this.Text = "Button Text";
        this.Name = "Button";

        this.Size = InterfaceSkin.ButtonSizeNormal;
        this.BackColor = InterfaceSkin.ButtonColorBackground;
        this.ForeColor = InterfaceSkin.ButtonColorText;
        this.Margin = InterfaceSkin.ButtonMargin;

        this.MouseEnter += new EventHandler(button_mouseEnter);
        this.MouseLeave += new EventHandler(button_mouseLeave);
        this.MouseDown += new MouseEventHandler(button_mouseDown);
        this.MouseUp += new MouseEventHandler(button_mouseUp);
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        // the color of the border
        Color borderColor = InterfaceSkin.ButtonColorBorder;

        // the width of the line used to draw the border
        int borderWidth = InterfaceSkin.ButtonBorderWidth;

        // the diameter of each corner circle
        int cornerDiameter = InterfaceSkin.ButtonBorderCornerRadius * 2;

        //
        // padding distance needed to display the whole border
        int offset = borderWidth / 2;
        pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        //
        // create border path
        Rectangle bounds = new Rectangle(
            offset, 
            offset, 
            this.Size.Width  - borderWidth, 
            this.Size.Height - borderWidth);

        // size of the corner circles
        Size size = new Size(cornerDiameter, cornerDiameter);
        Rectangle arc = new Rectangle(
            offset,
            offset,
            size.Width  - borderWidth, 
            size.Height - borderWidth);

        GraphicsPath path = new GraphicsPath();
        if (cornerDiameter == 0)
        {
            path.AddRectangle(bounds);
        }
        else
        {
            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - cornerDiameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - cornerDiameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
        }

        //
        // clear background
        pevent.Graphics.Clear(Parent.BackColor);

        //
        // fill border
        Pen pen = new Pen(this.BackColor, borderWidth);
        pevent.Graphics.FillPath(pen.Brush, path);

        //
        // draw Border
        pen.Color = borderColor;
        pevent.Graphics.DrawPath(pen, path);

        //
        // draw text
        SizeF length = pevent.Graphics.MeasureString(this.Text, this.Font, this.Width);
        Point location = new Point();
        location.X = (int)((this.Width / 2) - (length.Width / 2));
        location.Y = (int)((this.Height / 2) - (length.Height / 2));
        pevent.Graphics.DrawString(this.Text, Font, new SolidBrush(this.ForeColor), location);
    }

    private void button_mouseEnter(object sender, EventArgs e)
    {
        Console.Write("\nButton Mouse Enter");
        this.BackColor = InterfaceSkin.ButtonColorBackgroundHighlight;
    }

    private void button_mouseLeave(object sender, EventArgs e)
    {
        Console.Write("\nButton Mouse Leave");
        this.BackColor = InterfaceSkin.ButtonColorBackground;
    }

    private void button_mouseDown(object sender, MouseEventArgs e)
    {
        Console.Write("\nButton Mouse Down");

        if(e.Button == MouseButtons.Left)
            this.BackColor = InterfaceSkin.ButtonColorBackgroundClick;
    }
    
    private void button_mouseUp(object sender, MouseEventArgs e)
    {
        Console.Write("\nButton Mouse Up");

        if (e.Button == MouseButtons.Left)
            this.BackColor = InterfaceSkin.ButtonColorBackgroundHighlight;
    }
    
}
