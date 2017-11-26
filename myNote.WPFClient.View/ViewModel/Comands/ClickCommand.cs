using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace myNote.WPFClient.View.ViewModel.Comands
{
    public class ClickCommand : ICommand
    {
        #region Private members

        private readonly Action action;

        #endregion

        #region Public Event

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        #endregion

        #region Constructor

        public ClickCommand(Action action)
        {
            this.action = action;
        }

        #endregion

        #region Command Methods

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action();
        }

        #endregion
    }
}
