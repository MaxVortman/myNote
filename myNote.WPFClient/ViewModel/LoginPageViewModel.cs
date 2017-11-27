using myNote.WPFClient.ViewModel.Base;
using System.Windows.Input;

namespace myNote.WPFClient.ViewModel
{
    public class LoginPageViewModel : BaseViewModel
    {
        /// <summary>
        /// Next (Sing Up) Button Click Command
        /// </summary>
        public ICommand SingUpButtonClickCommand { get; set; } = new RelayCommand((obj)=>{ });

        /// <summary>
        /// Command of button, which change page for sing in
        /// </summary>
        public ICommand LoginButtonClickCommand { get; set; } = new RelayCommand((obj) => { });
    }
}
