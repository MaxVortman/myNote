using myNote.Model;
using myNote.WPFClient.ApiServices;
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
        #region Credential properties
        /// <summary>
        /// User's login, which entered in textbox
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// User's password, which entered in textbox
        /// </summary>
        public string Password { get; set; }

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
            IoC.IoC.Application.GoToPage(DataModels.ApplicationPage.Login);
        });

        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SingUpPageViewModel()
        {
            //Initialize Sing Up Command
            SingUpButtonClickCommand = new RelayCommand((obj) =>
            {
                try
                {
                    new LoginService(IoC.IoC.ConnectionString).Register(PasswordCrypter.GetCredential(Login, Password));
                    IoC.IoC.Application.GoToPage(DataModels.ApplicationPage.Login);
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show("Something went wrong...\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        #endregion
    }
}
