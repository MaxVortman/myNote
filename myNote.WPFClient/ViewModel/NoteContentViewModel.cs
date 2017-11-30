using myNote.ClientService;
using myNote.Model;
using myNote.WPFClient.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace myNote.WPFClient.ViewModel
{
    public class NoteContentViewModel
    {

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="note"></param>
        public NoteContentViewModel(Note note)
        {
            Note = note;
            try
            {

                User = new UserService(IoC.IoC.ConnectionString).GetUserAsync(note.UserId, UserData.UserDataContent.Token).Result;
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show("Something went wrong...\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            UnloadPageCommand = new RelayCommand(async (obj) =>
            {
                try
                {
                    await new NoteService(IoC.IoC.ConnectionString).UpdateNoteAsync(Note, UserData.UserDataContent.Token);
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show("Something went wrong...\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Current note
        /// View it
        /// </summary>
        public Note Note { get; set; }
        /// <summary>
        /// User, which own the note
        /// </summary>
        public User User { get; set; }

        #endregion

        #region Command
        /// <summary>
        /// Command, which execute when page unloaded
        /// </summary>
        public ICommand UnloadPageCommand { get; set; }

        #endregion
    }
}
