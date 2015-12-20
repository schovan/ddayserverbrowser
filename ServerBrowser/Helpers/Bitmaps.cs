using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ServerBrowser.Helpers
{
    public class Bitmaps
    {
        private const int ColorStep = 10;
        private const int BitmapSize = 16;

        public static List<BitmapSource> GetBitmaps()
        {
            var refBitmapImage = new BitmapImage(new Uri(@"pack://application:,,,/Resources/circle.png"));
            var refColor = Color.FromRgb(255, 200, 0);
            int stride = refBitmapImage.PixelWidth * (refBitmapImage.Format.BitsPerPixel / 8);
            int bitmapLength = stride * refBitmapImage.PixelHeight;
            var refBitmap = new byte[bitmapLength];
            refBitmapImage.CopyPixels(refBitmap, stride, 0);
            var bitmapSources = new List<BitmapSource> { refBitmapImage };

            for (int i = 10; i < 200; i += ColorStep)
            {
                var bitmap = new byte[bitmapLength];
                refBitmapImage.CopyPixels(bitmap, stride, 0);
                Color setColor = Color.FromRgb(255, (byte)(200 - i), 0);
                for (int x = 0; x < BitmapSize; x++)
                {
                    for (int y = 0; y < BitmapSize; y++)
                    {
                        if (bitmap[4 * (x * BitmapSize + y) + 0] == refColor.B && bitmap[4 * (x * BitmapSize + y) + 1] == refColor.G && bitmap[4 * (x * BitmapSize + y) + 2] == refColor.R && bitmap[4 * (x * BitmapSize + y) + 3] == refColor.A)
                        {
                            bitmap[4 * (x * BitmapSize + y) + 0] = setColor.B;
                            bitmap[4 * (x * BitmapSize + y) + 1] = setColor.G;
                            bitmap[4 * (x * BitmapSize + y) + 2] = setColor.R;
                            bitmap[4 * (x * BitmapSize + y) + 3] = setColor.A;
                        }
                    }
                }
                bitmapSources.Add(BitmapSource.Create(refBitmapImage.PixelWidth, refBitmapImage.PixelHeight, refBitmapImage.DpiX, refBitmapImage.DpiY, refBitmapImage.Format, refBitmapImage.Palette, bitmap, stride));
            }
            for (int i = 0; i < 256; i += ColorStep)
            {
                var bitmap = new byte[bitmapLength];
                refBitmapImage.CopyPixels(bitmap, stride, 0);
                Color setColor = Color.FromRgb((byte)(255 - i), 0, 0);
                for (int x = 0; x < BitmapSize; x++)
                {
                    for (int y = 0; y < BitmapSize; y++)
                    {
                        if (bitmap[4 * (x * BitmapSize + y) + 0] == refColor.B && bitmap[4 * (x * BitmapSize + y) + 1] == refColor.G && bitmap[4 * (x * BitmapSize + y) + 2] == refColor.R && bitmap[4 * (x * BitmapSize + y) + 3] == refColor.A)
                        {
                            bitmap[4 * (x * BitmapSize + y) + 0] = setColor.B;
                            bitmap[4 * (x * BitmapSize + y) + 1] = setColor.G;
                            bitmap[4 * (x * BitmapSize + y) + 2] = setColor.R;
                            bitmap[4 * (x * BitmapSize + y) + 3] = setColor.A;
                        }
                    }
                }
                bitmapSources.Add(BitmapSource.Create(refBitmapImage.PixelWidth, refBitmapImage.PixelHeight, refBitmapImage.DpiX, refBitmapImage.DpiY, refBitmapImage.Format, refBitmapImage.Palette, bitmap, stride));
            }
            return bitmapSources;
        }
    }
}
