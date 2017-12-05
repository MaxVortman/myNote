using myNote.ClientService;
using myNote.Model;
using myNote.WPFClient.IoC;
using myNote.WPFClient.ViewModel.Base;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace myNote.WPFClient.ViewModel
{
    public class SingUpPageViewModel : BaseViewModel
    {
        #region Public properties
        /// <summary>
        /// User's login, which entered in textbox
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Show enable buttons or not
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        #endregion

        #region Button Commands

        /// <summary>
        /// Next (Sing Up) Button Click Command
        /// </summary>
        public ICommand SingUpButtonClickCommand { get; set; }

        /// <summary>
        /// Command of button, which change page for sing in
        /// </summary>
        public ICommand LoginButtonClickCommand { get; set; } = new RelayCommand((obj) =>
        {
            IoC.IoC.WindowViewModel.GoToPage(DataModels.ApplicationPage.Login);
        });

        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SingUpPageViewModel(IPasswordSupplier passwordSupplier)
        {
            var api = ApiClient.CreateInstance(IoC.IoC.ConnectionString);
            //Initialize Sing Up Command
            SingUpButtonClickCommand = new RelayCommand((obj) =>
            {
                try
                {
                    IsEnabled = false;
                    api.LoginService.Register(PasswordCrypter.GetCredential(Login, passwordSupplier.GetPassword()));
                    IoC.IoC.WindowViewModel.GoToPage(DataModels.ApplicationPage.Login);
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show("Something went wrong...\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    IsEnabled = true;
                }
            });
        }

        #endregion
    }
}
