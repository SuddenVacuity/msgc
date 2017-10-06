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
    /// <param name="offset">Offset of the overlay relative to the base image</param>
    /// <param name="size">The size of the area of the overlay to read from</param>
    /// <param name="mode">The method used to blend images together</param>
    /// <returns></returns>
    public static Bitmap mergeImages(Bitmap source, Bitmap overlay, Point offset, Size size, BlendMode mode = BlendMode.None)
    {
        ////////////////////////////
        // Prepare Image Data
        ////////////////////////////

        // size of pixels in bytes
        int pixelSize = 4;
        PixelFormat sourcePxf = PixelFormat.Format32bppArgb;
        PixelFormat overlayPxf = PixelFormat.Format32bppArgb;

        // prepare source data for modification
        Bitmap sourceBmp = (Bitmap)source.Clone();
        Rectangle sourceRect = new Rectangle(0, 0, sourceBmp.Width, sourceBmp.Height);
        BitmapData sourceBmpData = sourceBmp.LockBits(sourceRect, ImageLockMode.ReadWrite, sourcePxf);

        // prepare overlay data for modification
        Bitmap overlayBmp = (Bitmap)overlay.Clone();
        Rectangle overlayRect = new Rectangle(0, 0, overlayBmp.Width, overlayBmp.Height);
        BitmapData overlayBmpData = overlayBmp.LockBits(overlayRect, ImageLockMode.ReadWrite, overlayPxf);

        IntPtr sourcePtr = sourceBmpData.Scan0;
        IntPtr overlayPtr = overlayBmpData.Scan0;

        int sourceNumBytes = sourceBmp.Width * sourceBmp.Height * pixelSize;

        // the number of bytes in each line for source/overlay image
        int sourceStride = source.Width * pixelSize;
        int overlayStride = overlay.Width * pixelSize;
        
        ////////////////////////////
        // Read/Write Points
        ////////////////////////////

        // the first and last line in source image to write to
        int byteStartLine = offset.Y * sourceStride;
        int byteEndLine = (overlay.Size.Height * sourceStride) + byteStartLine;
        
        // check source byte start/stop points against the
        // overlay offset and size
        // check specified region bounds
        if (byteStartLine < offset.Y * sourceStride)
            byteStartLine = offset.Y * sourceStride;
        if (byteEndLine > (offset.Y + size.Height) * sourceStride)
            byteEndLine = (offset.Y + size.Height) * sourceStride;
        if (byteStartLine < 0)
            byteStartLine = 0;
        if (byteEndLine > sourceNumBytes)
            byteEndLine = sourceNumBytes;

        // get start/stop position within each line
        // in the source image
        int lineWriteStart = offset.X * pixelSize;
        int lineWriteEnd = (overlay.Width * pixelSize) + lineWriteStart;

        // check image bounds against the line write start/end points
        // check specified region bounds
        if (lineWriteEnd > (offset.X + size.Width) * pixelSize)
            lineWriteEnd = (offset.X + size.Width) * pixelSize;
        if (lineWriteStart < 0)
            lineWriteStart = 0;
        if (lineWriteEnd > source.Width * pixelSize)
            lineWriteEnd = source.Width * pixelSize;

        // the (x, y) pixel position in overlay to read from
        int overlayReadStartX = offset.X;
        int overlayReadStartY = offset.Y;

        // negative offset means overlay is left/up relative to source
        // and the start position needs to be offset
        if (offset.X < 0)
            overlayReadStartX = Math.Abs(overlayReadStartX);
        else
            overlayReadStartX = 0;
        if (offset.Y < 0)
            overlayReadStartY = Math.Abs(overlayReadStartY);
        else
            overlayReadStartY = 0;
        
        // convert pixel positions to byte positions
        overlayReadStartX *= pixelSize;
        overlayReadStartY *= overlayStride;

        // position within overlay currently being read from
        int overlayBytePos = overlayReadStartX + overlayReadStartY;
        int sourceBytePos = byteStartLine + lineWriteStart;

        // number of bytes to read/write for each line
        int lineWriteLength = lineWriteEnd - lineWriteStart;
        
        // the number of lines in teh image to be merged
        int lineWriteHeight = ((byteEndLine + lineWriteStart) - sourceBytePos) / sourceStride;

        ////////////////////////////
        // Move Data to Source Image
        ////////////////////////////

        if (lineWriteHeight > 0 && lineWriteLength > 0)
        {
            // create data buffers to hold the data from each line
            byte[] sourceLine = new byte[lineWriteLength];
            byte[] overlayLine = new byte[lineWriteLength];

            // move the bmpdata pointer to the start position
            sourcePtr = IntPtr.Add(sourcePtr, sourceBytePos);
            overlayPtr = IntPtr.Add(overlayPtr, overlayBytePos);

            for (int i = 0; i < lineWriteHeight; i++)
            {
                // copy data from the bmp data to the buffers
                System.Runtime.InteropServices.Marshal.Copy(sourcePtr, sourceLine, 0, lineWriteLength);
                System.Runtime.InteropServices.Marshal.Copy(overlayPtr, overlayLine, 0, lineWriteLength);

                // merge the overlay pixels to the source pixels
                scanLine(ref sourceLine, ref overlayLine, pixelSize, mode);

                // copy the result data to the result image
                System.Runtime.InteropServices.Marshal.Copy(sourceLine, 0, sourcePtr, lineWriteLength);

                // repoint the bmp data pointer to the next line
                sourcePtr = IntPtr.Add(sourcePtr, sourceStride);
                overlayPtr = IntPtr.Add(overlayPtr, overlayStride);
            }
        }

        sourceBmp.UnlockBits(sourceBmpData);
        overlayBmp.UnlockBits(overlayBmpData);

        overlayBmp.Dispose();

        return sourceBmp;
    }

    /// <summary>
    /// Merges every pixel from two same-length arrays of bytes.
    /// </summary>
    /// <param name="source">The array of pixels used as the base.</param>
    /// <param name="overlay">The array of pixels applied to the base.</param>
    /// <param name="pixelSize">The number of bytes in each pixel</param>
    /// <param name="mode">The blend mode used to merge the pixels</param>
    private static void scanLine(ref byte[] source, ref byte[] overlay, int pixelSize, BlendMode mode)
    {
        // cycle though each pixel in the arrays
        for (int i = 0; i < source.Length; i += pixelSize)
        {
            // apply the overlay pixel to the source pixel
            mergePixel(ref source, ref overlay, i, mode);
        }
    }

    /// <summary>
    /// Merges one pixel within an array with another pixel in the same position in another array.
    /// </summary>
    /// <param name="source">The array containing the pixels used as a base.</param>
    /// <param name="overlay">The array with the pixels apllied to the base</param>
    /// <param name="position">The position within the arrays the target pixels are.</param>
    /// <param name="mode">The blend mode used to merge the pixels.</param>
    public static void mergePixel(ref byte[] source, ref byte[] overlay, int position, BlendMode mode)
    {
        // argbValues are in format BGRA (Blue, Green, Red, Alpha)
        int b = position + 0;
        int g = position + 1;
        int r = position + 2;
        int a = position + 3;

        if (position >= source.Length &&
            position >= overlay.Length)
            return;

        switch (mode)
        {
            case BlendMode.Add:
                {
                    ////////////////////////////////////////////
                    // 
                    ////////////////////////////////////////////

                    // skip if overlay has 0% opacity
                    if (overlay[a] == 0)
                        break;

                    float overAcoef = overlay[a] / (float)byte.MaxValue;
                    float sourAcoef = source[a] / (float)byte.MaxValue;

                    float A = 1.0f - (1.0f - overAcoef) * (1.0f - sourAcoef);
                    source[a] = (byte)(byte.MaxValue * A);

                    if (A < 1.0e-6)
                        break;

                    source[r] = (byte)(overlay[r] * overAcoef / A + source[r] * sourAcoef * (1.0f - overAcoef) / A);
                    source[g] = (byte)(overlay[g] * overAcoef / A + source[g] * sourAcoef * (1.0f - overAcoef) / A);
                    source[b] = (byte)(overlay[b] * overAcoef / A + source[b] * sourAcoef * (1.0f - overAcoef) / A);

                    break;
                } // END case BlendMode.Add
            case BlendMode.Mix:
                {
                    ////////////////////////////////////////////
                    // Averages the source pixels argb with the
                    //   overlay pixels argb weighted by the 
                    //   alpha weights of both pixels respectively
                    ////////////////////////////////////////////

                    // skip if overlay has 0% opacity
                    if (overlay[a] == 0)
                        break;

                    float overAcoef = overlay[a] / (float)byte.MaxValue;
                    float sourAweight = source[a] / (float)(source[a] + overlay[a]);
                    float overAweight = 1.0f - sourAweight;

                    source[a] = (byte)((overlay[a] * overAweight + source[a] * sourAweight));
                    source[r] = (byte)((overlay[r] * overAweight + source[r] * sourAweight));
                    source[g] = (byte)((overlay[g] * overAweight + source[g] * sourAweight));
                    source[b] = (byte)((overlay[b] * overAweight + source[b] * sourAweight));

                    break;
                } // END case BlendMode.Mix
            case BlendMode.Replace:
                {
                    ////////////////////////////////////////////
                    // Replaces each pixel with new pixel
                    ////////////////////////////////////////////

                    source[a] = overlay[a];
                    source[r] = overlay[r];
                    source[g] = overlay[g];
                    source[b] = overlay[b];
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
                    if (overlay[a] == 0)
                        break;
                    if (overlay[a] >= source[a])
                        source[a] = 0;
                    else
                        source[a] = (byte)(source[a] - overlay[a]);

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
        } // END switch(mode)
    }
    
}
