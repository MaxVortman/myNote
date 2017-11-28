using myNote.WPFClient.DataModels;
using myNote.WPFClient.View.Pages;
using System;
using System.Diagnostics;
using System.Globalization;

namespace myNote.WPFClient.ValueConverters
{
    public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Find the appropriate page
            switch ((ApplicationPage)value)
            {
                case ApplicationPage.Register:
                    return new SingUpPage();

                case ApplicationPage.Login:
                    return new SingInPage();

                case ApplicationPage.Main:
                    return new MainPage();
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
