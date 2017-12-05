using Microsoft.Win32;
using myNote.ClientService;
using myNote.Model;
using myNote.WPFClient.ViewModel.Base;
using System.Drawing;
using System.IO;
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
         /// <summary>
        /// User's avatar bytes
        /// </summary>
        public byte[] Avatar { get; set; }
        #endregion

        #region Commands
        /// <summary>
        /// Execute when we want to save user changes
        /// </summary>
        public ICommand SaveCommand { get; set; }
        /// <summary>
        /// Change the user avatar command
        /// </summary>
        public ICommand ChangeAvatarCommand { get; set; }
        #endregion

        #region Default Constructor
        public UserContentViewModel(User user)
        {
            User = user;
            IsEnabled = User.Id == UserData.UserDataContent.Token.UserId;
            var api = ApiClient.CreateInstance(IoC.IoC.ConnectionString);
            InitializeProperty(api);
            if (IsEnabled)
            {
                SaveCommand = new RelayCommand(async (obj) =>
                {
                    await api.UserService.UpdateUserAsync(User, UserData.UserDataContent.Token);
                });
                ChangeAvatarCommand = new RelayCommand(async(obj) =>
                {
                    var dialog = new OpenFileDialog();
                    if(dialog.ShowDialog() == true)
                    {
                        var imageBytes = File.ReadAllBytes(dialog.FileName);
                        api.ImageService.PostImage(imageBytes, User.Id);                       
                        await api.UserService.UpdateUserAsync(User, UserData.UserDataContent.Token);
                        InitializeProperty(api);
                    }
                });
            }
        }
        #endregion


        #region Helping method
        private async void InitializeProperty(ApiClient api)
        {
            Avatar = await api.ImageService.GetImageBytes(User.Id);
        } 
        #endregion
    }
}
