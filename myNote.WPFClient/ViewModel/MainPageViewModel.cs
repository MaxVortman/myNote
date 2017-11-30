using myNote.ClientService;
using myNote.Model;
using myNote.WPFClient.DataModels;
using myNote.WPFClient.View.Pages.MainFrameContent;
using myNote.WPFClient.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace myNote.WPFClient.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        #region Public Properties

        #region ListViews properties
        /// <summary>
        /// User's notes
        /// </summary>
        public ObservableCollection<Note> Notes { get; set; }
        /// <summary>
        /// Shared notes
        /// </summary>
        public ObservableCollection<Note> SharedNotes { get; set; }
        /// <summary>
        /// Selected listview note
        /// </summary>
        public Note SelectedNote { get; set; }
        #endregion

        #region Frame properties
        /// <summary>
        /// Frame content page
        /// </summary>
        public Page CurrentContentPage { get; set; }

        #endregion

        #endregion

        #region Commands
        /// <summary>
        /// Select note command
        /// </summary>
        public ICommand SelectedNoteCommand { get; set; }
        /// <summary>
        /// This command execute when page loaded
        /// </summary>
        public ICommand LoadPageCommand { get; set; }
        /// <summary>
        /// Add note button click command
        /// </summary>
        public ICommand AddNoteCommand { get; set; }
        /// <summary>
        /// The command execute when shared tab item is selected
        /// </summary>
        public ICommand SharedNoteEnterCommand { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainPageViewModel()
        {
            AddNoteCommand = new RelayCommand((obj) =>
            {
                CurrentContentPage = new NoteContentPage(new Note { UserId = UserData.UserDataContent.Token.UserId });
            });

            SelectedNoteCommand = new RelayCommand((obj) =>
            {
                CurrentContentPage = new NoteContentPage(SelectedNote);
            });

            LoadPageCommand = new RelayCommand(async(obj) =>
            {
                try
                {
                    var noteClient = new NoteService(IoC.IoC.ConnectionString);
                    var notes = await noteClient.GetUserNotesAsync(UserData.UserDataContent.Token.UserId);
                    Notes = new ObservableCollection<Note>(notes);
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show("Something went wrong...\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });

            SharedNoteEnterCommand = new RelayCommand(async (obj) =>
            {
                //TO DO
            });
        }
        #endregion
    }
}
