using myNote.WPFClient.DataModels;
using myNote.WPFClient.ViewModel.Base;

namespace myNote.WPFClient.ViewModel
{
    public class WindowViewModel : BaseViewModel
    {
        /// <summary>
        /// The current page of the application
        /// </summary>
        public ApplicationPage CurrentPage { get; set; } = ApplicationPage.Register;
    }
}
