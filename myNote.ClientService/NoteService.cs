using myNote.ClientService.Base;
using myNote.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace myNote.ClientService
{
    public class NoteService : BaseService
    {
        internal NoteService(string connectionString) : base(connectionString) { }
        /// <summary>
        /// Creating new note
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public async Task<Note> CreateNoteAsync(Note note, Token accessToken)
        {
            var response = await client.PostAsJsonAsync("notes", new { note, accessToken });
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
            return await response.Content.ReadAsAsync<Note>();
        }

        /// <summary>
        /// Getting all user's notes
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Note>> GetUserNotesAsync(Guid userId)
        {
            var response = await client.GetAsync($@"users/{userId}/notes");
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
            return await response.Content.ReadAsAsync<IEnumerable<Note>>();
        }

        /// <summary>
        /// Updating the note
        /// </summary>
        /// <param name="newNote">new, updated note</param>
        /// <param name="accessToken">User's token</param>
        /// <returns></returns>
        public async Task<Note> UpdateNoteAsync(Note newNote, Token accessToken)
        {           
            var response = await client.PutAsJsonAsync("notes/update", new { note = newNote, accessToken});
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
            return await response.Content.ReadAsAsync<Note>();
        }

        /// <summary>
        /// Get Note by id
        /// </summary>
        /// <param name="noteId">note id</param>
        /// <returns></returns>
        public async Task<Note> GetUserNoteAsync(Guid noteId)
        {
            var response = await client.GetAsync($@"notes/{noteId}");
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
            return await response.Content.ReadAsAsync<Note>();
        }
    }
}
