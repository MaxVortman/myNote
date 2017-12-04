using myNote.ClientService;
using myNote.Model;
using myNote.WPFClient.DataModels;
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
    public class NoteContentViewModel : BaseViewModel
    {

        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="note"></param>
        public NoteContentViewModel(Note note)
        {
            InitializeProperty(note);
            var api = ClientService.ApiClient.CreateInstance(IoC.IoC.ConnectionString);
            if (note.UserId == UserData.UserDataContent.Token.UserId)
            {
                //don't need update, when its not our note
                UnloadPageCommand = new RelayCommand(async (obj) =>
                {
                    try
                    {
                        await api.NoteService.UpdateNoteAsync(Note, UserData.UserDataContent.Token);
                        foreach (var group in Groups)
                        {
                            if (group.IsChecked)
                            {
                                await api.NoteGroupService.CreateNoteGroupAsync(new NoteGroup { NoteId = Note.Id, GroupId = group.Group.Id }, UserData.UserDataContent.Token);
                            }
                        }
                    }
                    catch (HttpRequestException e)
                    {
                        MessageBox.Show("Something went wrong...\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                });

                CheckCommand = new RelayCommand((isChecked) =>
                {
                    //one checkbox enable need whole time
                    foreach (var group in Groups)
                    {
                        group.IsEnabled = !(bool)isChecked || group.IsChecked;
                    }
                });
            }
            else
            {
                //if its not our note
                //we don't need enable it
                foreach (var group in Groups)
                {
                    group.IsEnabled = false;
                }
            }
        }

        private async void InitializeProperty(Note note)
        {
            var api = ClientService.ApiClient.CreateInstance(IoC.IoC.ConnectionString);
            try
            {
                if (note.Id == default(Guid))
                    Note = await api.NoteService.CreateNoteAsync(note, UserData.UserDataContent.Token);
                else
                    Note = note;
                User = await api.UserService.GetUserAsync(Note.UserId, UserData.UserDataContent.Token);
                var groups = await api.UserService.GetUserGroupsAsync(UserData.UserDataContent.Token.UserId, UserData.UserDataContent.Token);
                //group of this note
                var group = await api.NoteGroupService.GetGroupAsync(Note.Id);
                Groups = new ObservableCollection<CheckBoxGroupModel>(from g in groups.AsParallel()
                                                                      let isChecked = @group != null ? (g.Id == @group.Id) : false
                                                                      let isEnabled = @group != null ? (g.Id == @group.Id) : true
                                                                      select new CheckBoxGroupModel { Group = g, IsChecked = isChecked, IsEnabled = isEnabled });
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show("Something went wrong...\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
        /// <summary>
        /// Collection of user's groups
        /// </summary>
        public ObservableCollection<CheckBoxGroupModel> Groups { get; set; }
        
        #endregion

        #region Command
        /// <summary>
        /// Command, which execute when page unloaded
        /// </summary>
        public ICommand UnloadPageCommand { get; set; }
        /// <summary>
        /// Execute when checkboxs status change
        /// </summary>
        public ICommand CheckCommand { get; set; }

        #endregion
    }
}
