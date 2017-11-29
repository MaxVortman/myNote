using myNote.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace myNote.WPFClient.ApiServices
{
    public class NoteService
    {
        private readonly HttpClient client;
        #region Default Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="connectionString">Lint of connection to server</param>
        public NoteService(string connectionString = IoC.IoC.ConnectionString)
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(connectionString)
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        #endregion

        /// <summary>
        /// Creating new note
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public async Task<Note> CreateNoteAsync(Note note, Token accessToken)
        {
            var jobj = JObject.FromObject(new
            {
                note,
                accessToken
            });
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
    }
}
