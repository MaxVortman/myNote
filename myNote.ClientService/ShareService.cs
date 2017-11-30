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
    public class ShareService : BaseService
    {
        public ShareService(string connectionString) : base(connectionString) { }
        /// <summary>
        /// Create note share
        /// </summary>
        /// <param name="note">user's note, which he want share</param>
        /// <param name="accessToken">user's access token</param>
        /// <returns>share object</returns>
        public async Task<Share> CreateShareAsync(Note note, Token accessToken)
        {
            var response = await client.PostAsJsonAsync($@"shares", new { note, accessToken});
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
            return await response.Content.ReadAsAsync<Share>();
        }
        /// <summary>
        /// Get all user's share note
        /// </summary>
        /// <param name="id">user's id</param>
        /// <returns>shared notes enumerable</returns>
        public async Task<IEnumerable<Note>> GetUserSharesAsync(Guid id)
        {
            var response = await client.GetAsync($@"shares/user/{id}");
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
            return await response.Content.ReadAsAsync<IEnumerable<Note>>();
        }

        /// <summary>
        /// Get some shared note
        /// </summary>
        /// <param name="count">that much</param>
        /// <returns>shared notes enumerable</returns>
        public async Task<IEnumerable<Note>> GetSomeShares(int count)
        {
            var response = await client.GetAsync($@"shares/{count}");
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
            return await response.Content.ReadAsAsync<IEnumerable<Note>>();
        }
    }
}
