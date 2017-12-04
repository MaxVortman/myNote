using myNote.ClientService;
using myNote.Model;
using myNote.WPFClient.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace myNote.WPFClient.ViewModel
{
    public class UserContentViewModel : BaseViewModel
    {
        #region Public properties
        /// <summary>
        /// Viewable user
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// Show can we change user or not
        /// </summary>
        public bool IsEnabled { get; set; }
        #endregion

        #region Commands
        /// <summary>
        /// Execute when we want to save user changes
        /// </summary>
        public ICommand SaveCommand { get; set; }
        #endregion

        #region Default Constructor
        public UserContentViewModel(User user)
        {
            User = user;
            IsEnabled = User.Id == UserData.UserDataContent.Token.UserId;
            var api = ApiClient.CreateInstance(IoC.IoC.ConnectionString);

            if (IsEnabled)
                SaveCommand = new RelayCommand(async(obj) =>
                {
                    await api.UserService.UpdateUserAsync(User, UserData.UserDataContent.Token);
                });
        }
        #endregion
    }
}
