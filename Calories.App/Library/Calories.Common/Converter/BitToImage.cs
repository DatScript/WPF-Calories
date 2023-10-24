using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Common.Converter;
public class BitmapToImage : IValueConverter
{
    public BitmapImage ConvertBitmapToBitMapImage(Bitmap BitmapFrame)
    {
        BitmapImage bitmapimage = new BitmapImage();
        try
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BitmapFrame.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                memoryStream.Position = 0;
                bitmapimage.BeginInit();
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.StreamSource = memoryStream;
                bitmapimage.EndInit();
            }
            bitmapimage.Freeze();
        }
        catch
        {
            ;
        }
        return bitmapimage;
    }


    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BitmapImage img = new BitmapImage();
        if (value != null)
        {
            img = this.ConvertBitmapToBitMapImage((Bitmap)value);
        }
        return img;
    }

    public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}