using myNote.WPFClient.ViewModel;
using System.Windows.Controls;

namespace myNote.WPFClient.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для SingInPage.xaml
    /// </summary>
    public partial class SingInPage : Page
    {
        public SingInPage()
        {
            InitializeComponent();

            DataContext = new SingInPageViewModel();
        }
    }
}
