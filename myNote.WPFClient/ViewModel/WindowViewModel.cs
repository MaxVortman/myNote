using myNote.WPFClient.View.DataModels;
using myNote.WPFClient.View.ViewModel.Base;

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
