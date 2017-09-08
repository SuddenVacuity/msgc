using System;
using System.Drawing;
using System.Drawing.Imaging;

using static EnumList;


public static class RasterBlend
{

    
    /// <summary>
    /// Merges two images together based on blend mode.
    /// </summary>
    /// <param name="source">The image used as a base.</param>
    /// <param name="overlay">The image applied to the base.</param>
    /// <param name="updateRegion">Position and size of the area to overlay</param>
    /// <param name="mode">The method used to blend images together</param>
    /// <returns></returns>
    public static Bitmap mergeImages(Bitmap source, Bitmap overlay, Rectangle updateRegion, BlendMode mode = BlendMode.None)
    {
        ////////////////////////////
        // Prepare Image Data
        ////////////////////////////

        // size of pixels in bytes
        int pixelSize = 4;

        // prepare source data for modification
        Bitmap sourceBmp = (Bitmap)source.Clone();
        PixelFormat sourcePxf = PixelFormat.Format32bppArgb;
        Rectangle sourceRect = new Rectangle(0, 0, sourceBmp.Width, sourceBmp.Height);
        BitmapData sourceBmpData = sourceBmp.LockBits(sourceRect, ImageLockMode.ReadWrite, sourcePxf);

        // prepare overlay data for modification
        Bitmap overlayBmp = (Bitmap)overlay.Clone();
        PixelFormat overlayPxf = PixelFormat.Format32bppArgb;
        Rectangle overlayRect = new Rectangle(0, 0, overlayBmp.Width, overlayBmp.Height);
        BitmapData overlayBmpData = overlayBmp.LockBits(overlayRect, ImageLockMode.ReadWrite, overlayPxf);

        IntPtr sourcePtr = sourceBmpData.Scan0;
        IntPtr overlayPtr = overlayBmpData.Scan0;

        int sourceNumBytes = sourceBmp.Width * sourceBmp.Height * pixelSize;
        byte[] sourceArgbValues = new byte[sourceNumBytes];

        int overlayNumBytes = overlayBmp.Width * overlayBmp.Height * pixelSize;
        byte[] overlayArgbValues = new byte[overlayNumBytes];

        ////////////////////////////
        // Read/Write Points
        ////////////////////////////

        int sourceStride = source.Width * pixelSize;
        int overlayStride = overlay.Width * pixelSize;

        // the first and last line in source image to write to
        int byteStartLine = updateRegion.Y * sourceStride;
        int byteEndLine = (overlay.Size.Height * sourceStride) + byteStartLine;

        // position in each line to start and stop
        int lineWriteStart = updateRegion.X * pixelSize;
        int lineWriteEnd = (overlay.Width * pixelSize) + lineWriteStart;

        // get start/end lines
        // check image bounds
        if (byteStartLine < 0)
            byteStartLine = 0;
        if (byteEndLine > sourceArgbValues.Length)
            byteEndLine = sourceArgbValues.Length;
        // check specified region bounds
        if (byteStartLine < updateRegion.Y * sourceStride)
            byteStartLine = updateRegion.Y * sourceStride;
        if (byteEndLine > (updateRegion.Y + overlayBmp.Height) * sourceStride)
            byteEndLine = (updateRegion.Y + overlayBmp.Height) * sourceStride;

        // get start/stop position within each line
        // check image bounds
        if (lineWriteStart < 0)
            lineWriteStart = 0;
        if (lineWriteEnd > source.Width * pixelSize)
            lineWriteEnd = source.Width * pixelSize;
        // check specified region bounds
        if (lineWriteStart < updateRegion.X * pixelSize)
            lineWriteStart = updateRegion.X * pixelSize;
        if (lineWriteEnd > (updateRegion.X + overlayBmp.Width) * pixelSize)
            lineWriteEnd = (updateRegion.X + overlayBmp.Width) * pixelSize;

        // start position for x/y in bytes within overlay to copy data from
        int overlayReadStartX = updateRegion.X;
        int overlayReadStartY = updateRegion.Y;

        // set offset within overlay if negative - 
        // negative means overlay read start position needs to move
        if (overlayReadStartX < 0) overlayReadStartX = Math.Abs(overlayReadStartX); else overlayReadStartX = 0;
        if (overlayReadStartY < 0) overlayReadStartY = Math.Abs(overlayReadStartY); else overlayReadStartY = 0;

        //
        // convert pixels to byte positions
        overlayReadStartX *= pixelSize;
        overlayReadStartY *= overlayStride;

        // position within overlay currently being read from
        int overlayBytePos = overlayReadStartX + overlayReadStartY;

        // number of btye to read/write for each line
        int lineWriteLength = lineWriteEnd - lineWriteStart;

        ////////////////////////////
        // Move Data to Source Image
        ////////////////////////////

        System.Runtime.InteropServices.Marshal.Copy(sourcePtr, sourceArgbValues, 0, sourceNumBytes);
        System.Runtime.InteropServices.Marshal.Copy(overlayPtr, overlayArgbValues, 0, overlayNumBytes);

        for (int linePos = byteStartLine + lineWriteStart; linePos < byteEndLine;)
        {
            for (int b = 0; b < lineWriteLength; b += pixelSize)
            {
                mergePixels(
                    ref sourceArgbValues,
                    ref overlayArgbValues,
                    linePos + b,
                    overlayBytePos + b,
                    mode);
            }

            // go to next line in overlay
            overlayBytePos += overlayStride;
            // go to next line in source
            linePos += sourceStride;
        }

        System.Runtime.InteropServices.Marshal.Copy(sourceArgbValues, 0, sourcePtr, sourceNumBytes);
        sourceBmp.UnlockBits(sourceBmpData);
        overlayBmp.UnlockBits(overlayBmpData);

        return sourceBmp;
    }
    
    /// <summary>
    /// Blends pixels together based on Blend Mode.
    /// </summary>
    /// <param name="source">The pixel array to be used as a base.</param>
    /// <param name="overlay">The pixel array to be applied to the base.</param>
    /// <param name ="sourcePosition">The position of the target pixel within the source array</param>
    /// <param name ="overlayPosition">The position of the target pixel within the overlay array</param>
    /// <param name ="mode">The blending mode used to combine the pixels</param>
    /// <returns></returns>
    private static void mergePixels(ref byte[] source, ref byte[] overlay, int sourcePosition, int overlayPosition, BlendMode mode)
    {
        // argbValues are in format BGRA (Blue, Green, Red, Alpha)
        // position in sourcebytes
        int b = sourcePosition;
        int g = sourcePosition + 1;
        int r = sourcePosition + 2;
        int a = sourcePosition + 3;
        // position in overlaybytes
        int bo = overlayPosition;
        int go = overlayPosition + 1;
        int ro = overlayPosition + 2;
        int ao = overlayPosition + 3;

        switch (mode)
        {
            case BlendMode.DrawAdd:
                {
                    ////////////////////////////////////////////
                    // 
                    ////////////////////////////////////////////

                    // skip if overlay has 0% opacity
                    if (overlay[ao] == 0)
                        break;

                    float overAcoef = overlay[ao] / (float)byte.MaxValue;
                    float sourAcoef = source[a] / (float)byte.MaxValue;

                    float A = 1.0f - (1.0f - overAcoef) * (1.0f - sourAcoef);
                    source[a] = (byte)(byte.MaxValue * A);

                    if (A < 1.0e-6)
                        break;

                    source[r] = (byte)(overlay[ro] * overAcoef / A + source[r] * sourAcoef * (1.0f - overAcoef) / A);
                    source[g] = (byte)(overlay[go] * overAcoef / A + source[g] * sourAcoef * (1.0f - overAcoef) / A);
                    source[b] = (byte)(overlay[bo] * overAcoef / A + source[b] * sourAcoef * (1.0f - overAcoef) / A);

                    break;
                } // END case BlendMode.DrawAdd
            case BlendMode.DrawMix:
                {
                    ////////////////////////////////////////////
                    // Averages the source pixels argb with the
                    //   overlay pixels argb weighted by the 
                    //   alpha weights of both pixels respectively
                    ////////////////////////////////////////////

                    // skip if overlay has 0% opacity
                    if (overlay[ao] == 0)
                        break;

                    float overAcoef = overlay[ao] / (float)byte.MaxValue;
                    float sourAweight = source[a] / (float)(source[a] + overlay[ao]);
                    float overAweight = 1.0f - sourAweight;

                    source[a] = (byte)((overlay[ao] * overAweight + source[a] * sourAweight));
                    source[r] = (byte)((overlay[ro] * overAweight + source[r] * sourAweight));
                    source[g] = (byte)((overlay[go] * overAweight + source[g] * sourAweight));
                    source[b] = (byte)((overlay[bo] * overAweight + source[b] * sourAweight));

                    break;
                } // END case BlendMode.DrawMix
            case BlendMode.Replace:
                {
                    ////////////////////////////////////////////
                    // Replaces each pixel with new pixel
                    ////////////////////////////////////////////

                    source[a] = overlay[ao];
                    source[r] = overlay[ro];
                    source[g] = overlay[go];
                    source[b] = overlay[bo];
                    break;
                }
            case BlendMode.Erase:
                {
                    ////////////////////////////////////////////
                    // Lowers the source pixel alpha value by
                    //   the overlay pixels alph value
                    // If the ruselting pixel has 0 alpha its
                    //   color is changes to white
                    ////////////////////////////////////////////

                    // skip if overlay has 0% opacity
                    if (overlay[ao] == 0)
                        break;
                    if (overlay[ao] >= source[a])
                        source[a] = 0;
                    else
                        source[a] = (byte)(source[a] - overlay[ao]);

                    // If 0% transparent change colors to white
                    if (source[a] > 0)
                        break;

                    // turn 0% opacity pixels white
                    source[r] = byte.MaxValue;
                    source[g] = byte.MaxValue;
                    source[b] = byte.MaxValue;
                    break;
                } // END case BlendMode.Erase
            case BlendMode.None: break;
            default: Console.Write("\nUnknown Blend Mode"); break;
        } // END switch(br)
    }

}
