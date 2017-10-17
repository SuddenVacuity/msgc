using System.Drawing;
using System.Windows.Forms;
using System;

/// <summary>
/// Contains style values for the application.
/// </summary>
public static class InterfaceSkin
{
    private static Color backgroundDark = Color.FromArgb(255, 30, 30, 35);

    private static Color mainBackground = Color.FromArgb(255, 45, 45, 50);
    private static Color mainBackgroundHighlight = Color.FromArgb(255, 60, 60, 80);
    private static Color mainBackgroundSelect = Color.FromArgb(255, 80, 80, 120);
    private static Color mainText = Color.White;
    private static Color mainTextSelect = Color.FromArgb(255, 255, 255, 255);
    private static Color mainTextFaded = Color.Gray;

    private static Color childBackground = Color.FromArgb(255, 60, 60, 65);
    private static Color childBackgroundHighlight = Color.FromArgb(255, 120, 120, 140);
    private static Color childBackgroundSelect = Color.FromArgb(255, 160, 160, 200);
    private static Color childText = Color.FromArgb(255, 255, 255, 255);
    private static Color childTextHighlight = Color.FromArgb(255, 0, 0, 0);
    private static Color childTextFaded = Color.FromArgb(255, 180, 180, 180);

    private static Color objectBackground = Color.FromArgb(255, 120, 120, 120);
    private static Color objectBorder = Color.FromArgb(255, 180, 180, 180);
    private static Color objectBackgroundHighlight = Color.FromArgb(255, 160, 160, 180);
    private static Color objectBackgroundSelect = Color.FromArgb(255, 200, 200, 240);
    private static Color objectText = Color.FromArgb(255, 0, 0, 0);
    private static Color objectTextFaded = Color.FromArgb(255, 120, 120, 120);

    ////////////////////////////
    ////      Windows       ////
    ////////////////////////////
    public static Color WindowColorBackground = mainBackground;
    public static Color WindowColorText = mainText;
    public static Padding WindowPadding = new Padding(10, 10, 0, 0); // right and down padding stack with control margin

    ////////////////////////////
    ////     MenuStrip      ////
    ////////////////////////////
    public static Size MenuStripSizeNormal = new Size(200, 26);
    public static Color MenuStripColorBackground = mainBackground;
    public static Color MenuStripColorBackgroundGradientLeft = mainBackground;
    public static Color MenuStripColorBackgroundGradientRight = mainBackground;
    public static Color MenuStripColorDropDownBorderSelected = mainBackgroundHighlight;
    public static Color MenuStripColorDropDownBorderHover = mainBackgroundHighlight;
    public static Color MenuStripColorDropDownSeperator = mainBackgroundHighlight;
    public static Color MenuStripColorItemBackgroundGradientTopHover = mainBackgroundSelect;
    public static Color MenuStripColorItemBackgroundGradientBottomHover = mainBackgroundSelect;
    public static Color MenuStripColorItemBackgroundGradientTopSelected = mainBackgroundSelect;
    public static Color MenuStripColorItemBackgroundGradientBottomSelected = mainBackgroundSelect;


    ////////////////////////////
    ////   ToolStripItem    ////
    ////////////////////////////
    public static Size ToolStripItemSizeNormal = new Size(45, 20);
    public static Color ToolStripItemColorBackground = backgroundDark;
    public static Color ToolStripItemColorBackgroundGradientLeft = mainBackgroundHighlight;
    public static Color ToolStripItemColorBackgroundGradientCenter = mainBackground;
    public static Color ToolStripItemColorBackgroundGradientRight = backgroundDark;
    public static Color ToolStripItemColorText = mainText;
    public static Color ToolStripItemColorHighlight = mainBackgroundSelect;

    ////////////////////////////
    ////       Panel        ////
    ////////////////////////////
    public static Size PanelSizeNormal = new Size(200, 200);
    public static Color PanelColorBackground = mainBackground;
    public static Color PanelColorText = mainText;

    ////////////////////////////
    ////      TextBox       ////
    ////////////////////////////
    public static Size TextBoxSizeNormal = new Size(80, 20);
    public static Color TextBoxColorBackground = childBackground;
    public static Color TextBoxColorText = childText;
    public static Color TextBoxColorTextFaded = childTextFaded;
    public static Padding TextBoxMargin = new Padding(10);

    ////////////////////////////
    ////       Label        ////
    ////////////////////////////
    public static Size LabelSizeNormal = new Size(60, 20);
    public static Color LabelColorBackground = childBackground;
    public static Color LabelColorText = childText;
    public static Padding LabelMargin = new Padding(10);

    ////////////////////////////
    ////       Button       ////
    ////////////////////////////
    public static Size ButtonSizeNormal = new Size(80, 25);
    public static Color ButtonColorBorder = objectBorder;
    public static Color ButtonColorBackground = objectBackground;
    public static Color ButtonColorBackgroundHighlight = objectBackgroundHighlight;
    public static Color ButtonColorBackgroundClick = objectBackgroundSelect;
    public static Color ButtonColorText = objectText;
    public static Color ButtonColorTextFaded = objectTextFaded;
    public static Padding ButtonMargin = new Padding(10);
    public static int ButtonBorderWidth = 1;
    public static int ButtonBorderCornerRadius = 4;


    ////////////////////////////
    ////     LayerList      ////
    ////////////////////////////
    public static Size LayerListSize = new Size(250, 200);
    public static int LayerListHeaderHeight = 30;
    public static int LayerListFooterHeight = 30;
    public static Color LayerListHeaderColorBackground = mainBackground;
    public static Color LayerListHeaderColorText = mainText;
    public static Color LayerListBodyColorBackground = mainBackground;
    public static Color LayerListBodyColorText = mainText;
    public static Color LayerListFooterColorBackground = mainBackground;
    public static Color LayerListButtonColorBackground = objectBackground;
    public static Color LayerListButtonColorText = objectText;
    public static Color LayerListButtonColorTextFaded = objectTextFaded;


    public static Size LayerListItemSize = new Size(150, 25);
    public static Color LayerListItemColorBorder = mainBackgroundSelect;
    public static Color LayerListItemColorBackground = mainBackground;
    public static Color LayerListItemColorBackgroundHiglight = mainBackgroundHighlight;
    public static Color LayerListItemColorBackgroundSelected = mainBackgroundSelect;
    public static Color LayerListItemColorText = mainText;
    public static Color LayerListItemColorTextSelected = mainTextSelect;


    //
    //////////////////////////////
    //////      ListView      ////
    //////////////////////////////
    //public static Size ListViewSizeNormal = new Size(150, 200);
    //public static Color ListViewColorBackground = background;
    //public static Color ListViewColorText = text;
    //public static Color ListViewColorHeaderBackground = backgroundSubBodySelect;
    //public static Color ListViewColorHeaderText = text;
    //public static int ListViewItemHeight = 30;
    //public static Color ListViewItemColorBackground = background;
    //public static Color ListViewItemColorBackGroundHighlight = backgroundLight;
    //public static Color ListViewItemColorBackgroundSelected = backgroundSubBodyHighlight;
    //public static Color ListViewItemColorText = text;
    
}
