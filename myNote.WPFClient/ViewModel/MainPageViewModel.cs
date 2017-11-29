using myNote.Model;
using myNote.WPFClient.ApiServices;
using myNote.WPFClient.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace myNote.WPFClient.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        #region Public Properties
        /// <summary>
        /// User's notes
        /// </summary>
        public ObservableCollection<Note> Notes { get; set; }
        /// <summary>
        /// Title note, which add
        /// </summary>
        public string NewNoteTitle { get; set; }
        /// <summary>
        /// Content note, which add
        /// </summary>
        public string NewNoteContent { get; set; }

        #endregion

        #region Commands
        /// <summary>
        /// Add new note command
        /// </summary>
        public ICommand AddNoteCommand { get; set; }
        /// <summary>
        /// This command execute when page loaded
        /// </summary>
        public ICommand LoadPageCommand { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainPageViewModel()
        {
            AddNoteCommand = new RelayCommand(async(obj) =>
            {
                try
                {
                    var noteClient = new NoteService();
                    var note = await noteClient.CreateNoteAsync(new Note { Title = NewNoteTitle, Content = NewNoteContent, UserId = UserData.UserDataContent.Token.UserId }, UserData.UserDataContent.Token);
                    Notes.Add(note);
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show("Something went wrong...\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            LoadPageCommand = new RelayCommand(async(obj) =>
            {
                try
                {
                    var noteClient = new NoteService();
                    var notes = await noteClient.GetUserNotesAsync(UserData.UserDataContent.Token.UserId);
                    Notes = new ObservableCollection<Note>(notes);
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show("Something went wrong...\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
        #endregion
    }
}
