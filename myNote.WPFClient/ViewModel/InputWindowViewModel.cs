using myNote.WPFClient.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace myNote.WPFClient.ViewModel
{
    public class InputWindowViewModel : BaseViewModel
    {
        #region Property

        public string InputText { get; set; }

        #endregion

        #region Command

        public ICommand AcceptCommand { get; set; }

        #endregion

        #region Default Constructor

        public InputWindowViewModel(string oldNameString, Window window)
        {
            InputText = oldNameString;

            AcceptCommand = new RelayCommand((obj) =>
            {
                window.DialogResult = true;
            });
        }

        #endregion
    }
}
