using System.Windows.Forms;
using System.Drawing;

public class CustomColorTable : ProfessionalColorTable
{
    Color unknownElement = Color.Yellow;
    // the background color of the 
    // menu strip's dropdown menu
    public override Color ToolStripDropDownBackground
    {
        get
        {
            return InterfaceSkin.ToolStripItemColorBackground;
        }
    }

    // the gradient in the menu strips dropdown menu background
    // on the left side 22 pixels wide in total
    public override Color ImageMarginGradientBegin
    {
        get
        {
            return InterfaceSkin.ToolStripItemColorBackgroundGradientLeft;
        }
    }
    public override Color ImageMarginGradientMiddle
    {
        get
        {
            return InterfaceSkin.ToolStripItemColorBackgroundGradientCenter;
        }
    }
    public override Color ImageMarginGradientEnd
    {
        get
        {
            return InterfaceSkin.ToolStripItemColorBackgroundGradientRight;
        }
    }

    // the color of the border of the selected menu strip item in the menu strip
    // along with the border of the menu strip's drop down menu
    // when selected
    public override Color MenuBorder
    {
        get
        {
            return InterfaceSkin.MenuStripColorDropDownBorderSelected;
        }
    }

    // the color of the border of menu strip items
    // when the cursor is hovering over them
    public override Color MenuItemBorder
    {
        get
        {
            return InterfaceSkin.MenuStripColorDropDownBorderHover;
        }
    }

    // the color of menu strip item background in drop down menus
    // when the cursor is hovering over them
    public override Color MenuItemSelected
    {
        get
        {
            return InterfaceSkin.ToolStripItemColorHighlight;
        }
    }

    // the gradient of the menu strip background from left to right
    public override Color MenuStripGradientBegin
    {
        get
        {
            return InterfaceSkin.MenuStripColorBackgroundGradientLeft;
        }
    }
    public override Color MenuStripGradientEnd
    {
        get
        {
            return InterfaceSkin.MenuStripColorBackgroundGradientRight;
        }
    }


    // Gradient shown on menu items in the menu strip
    // from top to bottom when the cursor is hovering over them
    public override Color MenuItemSelectedGradientBegin
    {
        get
        {
            return InterfaceSkin.MenuStripColorItemBackgroundGradientTopHover;
        }
    }
    public override Color MenuItemSelectedGradientEnd
    {
        get
        {
            return InterfaceSkin.MenuStripColorItemBackgroundGradientBottomHover;
        }
    }


    // Gradient shown on menu items in the menu strip
    // from top to bottom when they are selected
    public override Color MenuItemPressedGradientBegin
    {
        get
        {
            return InterfaceSkin.MenuStripColorItemBackgroundGradientTopSelected;
        }
    }
    public override Color MenuItemPressedGradientEnd
    {
        get
        {
            return InterfaceSkin.MenuStripColorItemBackgroundGradientBottomSelected;
        }
    }

    // drop down item seperators
    public override Color SeparatorDark
    {
        get
        {
            return InterfaceSkin.MenuStripColorDropDownSeperator;
        }
    }
    public override Color SeparatorLight
    {
        get
        {
            return InterfaceSkin.MenuStripColorDropDownSeperator;
        }
    }
    ///////////////////////////////
    ///    UNKNOWN ELEMENTS     ///
    ///////////////////////////////

    public override Color MenuItemPressedGradientMiddle
    {
        get
        {
            return unknownElement;
        }
    }
    public override Color GripLight
    {
        get
        {
            return unknownElement;
        }
    }
    public override Color GripDark
    {
        get
        {
            return unknownElement;
        }
    }
    public override Color ToolStripBorder
    {
        get
        {
            return unknownElement;
        }
    }
    public override Color ToolStripContentPanelGradientBegin
    {
        get
        {
            return unknownElement;
        }
    }
    public override Color ToolStripContentPanelGradientEnd
    {
        get
        {
            return unknownElement;
        }
    }
    public override Color ToolStripGradientBegin
    {
        get
        {
            return unknownElement;
        }
    }
    public override Color ToolStripGradientEnd
    {
        get
        {
            return unknownElement;
        }
    }
    public override Color ToolStripGradientMiddle
    {
        get
        {
            return unknownElement;
        }
    }
    public override Color ToolStripPanelGradientBegin
    {
        get
        {
            return unknownElement;
        }
    }
    public override Color ToolStripPanelGradientEnd
    {
        get
        {
            return unknownElement;
        }
    }
}
