using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using myNote.Model;
using myNote.ClientService.Base;

namespace myNote.ClientService
{
    public class GroupService : BaseService
    {
        public GroupService(string connectionString) : base(connectionString) { }
        /// <summary>
        /// Create new user's group
        /// </summary>
        /// <param name="name">group name</param>
        /// <param name="accessToken"></param>  
        /// <returns></returns>
        public async Task<Group> CreateGroup(string name, Token accessToken)
        {
            var response = await client.PostAsJsonAsync($@"groups/{name}", accessToken);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
            return await response.Content.ReadAsAsync<Group>();
        }
        /// <summary>
        /// Delete user's group
        /// </summary>
        /// <param name="id">group id</param>
        /// <param name="accessToken">user's access token</param>
        public void DeleteGroup(Guid id, Token accessToken)
        {
            var response =  client.PutAsJsonAsync($@"groups/deletebyid/{id}", accessToken).Result;
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
        }
        /// <summary>
        /// Delete user's group
        /// </summary>
        /// <param name="name">group name</param>
        /// <param name="accessToken">user's access token</param>
        public void DeleteGroup(string name, Token accessToken)
        {
            var response = client.PutAsJsonAsync($@"groups/deletebyname/{name}", accessToken).Result;
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
        }
    }
}
