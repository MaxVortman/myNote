using myNote.WPFClient.DataModels;
using myNote.WPFClient.View.Pages.MainFrameContent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.WPFClient.ValueConverters
{
    public class ContentPageValueConverter : BaseValueConverter<ContentPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ContentPage)value)
            {
                case ContentPage.NoteContent:
                    return new NoteContentPage();
                case ContentPage.UserContent:
                    return new UserContentPage();
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
