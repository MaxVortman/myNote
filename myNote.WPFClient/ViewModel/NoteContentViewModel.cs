﻿using myNote.ClientService;
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
            UnloadPageCommand = new RelayCommand(async (obj) =>
            {
                try
                {
                    await api.NoteService.UpdateNoteAsync(Note, UserData.UserDataContent.Token);
                }
                catch (HttpRequestException e)
                {
                    MessageBox.Show("Something went wrong...\n" + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
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

        #endregion

        #region Command
        /// <summary>
        /// Command, which execute when page unloaded
        /// </summary>
        public ICommand UnloadPageCommand { get; set; }

        #endregion
    }
}