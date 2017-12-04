using myNote.Model;
using myNote.WPFClient.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.WPFClient.ViewModel
{
    public class UserContentViewModel : BaseViewModel
    {
        #region Public properties
        /// <summary>
        /// Viewable user
        /// </summary>
        public User User { get; set; }
        #endregion

        #region Default Constructor
        public UserContentViewModel(User user)
        {
            User = user;
        }
        #endregion
    }
}
