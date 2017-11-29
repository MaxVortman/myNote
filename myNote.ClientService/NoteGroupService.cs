using myNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using myNote.ClientService.Base;

namespace myNote.ClientService
{
    public class NoteGroupService : BaseService
    {
        public NoteGroupService(string connectionString) : base(connectionString) { }
        /// <summary>
        /// Create group of note
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="groupId"></param>
        /// <param name="accessToken">user's access token</param>
        /// <returns></returns>
        public async Task<NoteGroup> CreateNoteGroupAsync(NoteGroup noteGroup,Token accessToken)
        {
            var response = await client.PostAsJsonAsync($@"notegroup", new { noteGroup, accessToken });
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
            return await response.Content.ReadAsAsync<NoteGroup>();
        }
        /// <summary>
        /// Get note group
        /// </summary>
        /// <param name="id">note id</param>
        /// <returns>note group</returns>
        public async Task<Group> GetGroupAsync(Guid id)
        {
            var response = await client.GetAsync($@"notegroups/notes/{id}");
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
            return await response.Content.ReadAsAsync<Group>();
        }
        /// <summary>
        /// Get all note contains in group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>notes enumarable</returns>
        public async Task<IEnumerable<Note>> GetNotesInGroupAsync(Guid groupId, Token accessToken)
        {
            var response = await client.PutAsJsonAsync($@"notegroups/group/{groupId}", accessToken);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
            return await response.Content.ReadAsAsync<IEnumerable<Note>>();
        }
        /// <summary>
        /// Get all note contains in group
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name">group name</param>
        /// <returns>notes enumerable</returns>
        public async Task<IEnumerable<Note>> GetNotesInGroupAsync(string name, Token accessToken)
        {
            var response = await client.PutAsJsonAsync($@"notegroups/user/group/{name}", accessToken);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
            return await response.Content.ReadAsAsync<IEnumerable<Note>>();
        }
    }
}
