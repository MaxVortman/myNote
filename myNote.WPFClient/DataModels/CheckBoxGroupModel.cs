using myNote.Model;
using myNote.WPFClient.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.WPFClient.DataModels
{
    public class CheckBoxGroupModel : BaseViewModel
    {
        /// <summary>
        /// Group
        /// </summary>
        public Group Group { get; set; }
        /// <summary>
        /// Checked status of checkbox
        /// </summary>
        public bool IsChecked { get; set; }
        /// <summary>
        /// Enable status of checkbox
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
