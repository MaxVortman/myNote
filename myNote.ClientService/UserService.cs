using myNote.ClientService.Base;
using myNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace myNote.ClientService
{
    public class UserService : BaseService
    {
        internal UserService(string connectionString) : base(connectionString) { }
        /// <summary>
        /// Get User by id
        /// </summary>
        /// <param name="id">user's id</param>
        /// <param name="accessToken">your access token</param>
        /// <returns>that user</returns>
        public async Task<User> GetUserAsync(Guid id, Token accessToken)
        {
            var response = await client.PutAsJsonAsync($@"users/{id}", accessToken);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
            return await response.Content.ReadAsAsync<User>();
        }
        /// <summary>
        /// Get User's groups
        /// </summary>
        /// <param name="id">user's id</param>
        /// <param name="accessToken">your access token</param>
        /// <returns>his groups enumerable</returns>
        public async Task<IEnumerable<Group>> GetUserGroupsAsync(Guid id, Token accessToken)
        {
            var response = await client.PutAsJsonAsync($@"users/{id}/groups", accessToken);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
            return await response.Content.ReadAsAsync<IEnumerable<Group>>();
        }
        /// <summary>
        /// Update current user
        /// </summary>
        /// <param name="user">user, who need to update</param>
        /// <param name="accessToken">his access token</param>
        /// <returns>new, updated user</returns>
        public async Task<User> UpdateUserAsync(User user, Token accessToken)
        {
            var response = await client.PutAsJsonAsync($@"users/update", new { user, accessToken});
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
            return await response.Content.ReadAsAsync<User>();
        }
    }
}
