using myNote.WPFClient.IoC;
using myNote.WPFClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace myNote.WPFClient.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class SingUpPage : Page, IPasswordSupplier
    {
        public SingUpPage()
        {
            InitializeComponent();

            this.DataContext = new SingUpPageViewModel(this);
        }

        public string GetPassword()
        {
            return RegisterPassword.Password;
        }
    }
}
