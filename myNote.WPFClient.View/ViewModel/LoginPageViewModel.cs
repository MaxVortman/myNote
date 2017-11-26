using myNote.WPFClient.View.ViewModel.Base;
using myNote.WPFClient.View.ViewModel.Comands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace myNote.WPFClient.View.ViewModel
{
    public class LoginPageViewModel : BaseViewModel
    {
        /// <summary>
        /// Next (Sing Up) Button Click Command
        /// </summary>
        public ClickCommand SingUpButtonClickCommand { get; set; } = new ClickCommand(()=>{ });

        /// <summary>
        /// Command of button, which change page for sing in
        /// </summary>
        public ClickCommand LoginButtonClickCommand { get; set; } = new ClickCommand(() => { });
    }
}
