


public class EnumList
{
    // used by Brush and Layer to determine Blendmode in RasterBlend functions
    public enum BlendMode
    {
        None,
        Over,
        Add,
        Mix,
        Replace,
        ReplaceClampAlpha,
        Erase,
    }

    // id values of the image caches in MainProgram
    public enum ImageCacheId
    {
        Background,
        CurrentLayer,
        Foreground,
        DrawBuffer,
        CanvasImage,
        FinalImage,

        Count
    }
}
