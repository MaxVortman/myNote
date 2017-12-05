using myNote.WPFClient.IoC;
using myNote.WPFClient.ViewModel;
using System.Windows.Controls;

namespace myNote.WPFClient.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для SingInPage.xaml
    /// </summary>
    public partial class SingInPage : Page, IPasswordSupplier
    {
        public SingInPage()
        {
            InitializeComponent();

            DataContext = new SingInPageViewModel(this);
        }

        public string GetPassword()
        {
            return LoginPassword.Password;
        }
    }
}
