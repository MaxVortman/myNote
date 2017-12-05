using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace myNote.WPFClient.ValueConverters
{
    public class ImageByteValueConverter : BaseValueConverter<ImageByteValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            var imageData = value as byte[];
            return ByteToImage(imageData);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Convert image bytes to <see cref="ImageSource"/>
        /// </summary>
        /// <param name="imageData">image bytes</param>
        /// <returns></returns>
        private BitmapImage ByteToImage(byte[] imageData)
        {
            using (var ms = new MemoryStream(imageData))
            {
                BitmapImage biImg = new BitmapImage();                
                biImg.BeginInit();
                biImg.CacheOption = BitmapCacheOption.OnLoad;
                biImg.StreamSource = ms;
                biImg.EndInit();

                return biImg; 
            }
        }
    }
}
