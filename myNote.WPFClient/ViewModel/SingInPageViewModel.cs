using myNote.WPFClient.ApiServices;
using myNote.WPFClient.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace myNote.WPFClient.ViewModel
{
    public class SingInPageViewModel : BaseViewModel
    {
        #region Public Properties
        /// <summary>
        /// User's Login from textbox
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// User's Password from textbox
        /// </summary>
        public string Password { get; set; }
        #endregion

        #region Commands
        /// <summary>
        /// Command, which login the user
        /// </summary>
        public ICommand LoginButtonClickCommand { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public SingInPageViewModel()
        {
            LoginButtonClickCommand = new RelayCommand(async(obj) =>
            {
                try
                {
                    UserData.UserDataContent.Token = await new LoginService().LoginAsync(PasswordCrypter.GetCredential(Login, Password));
                    IoC.IoC.Application.GoToPage(DataModels.ApplicationPage.Main);
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
