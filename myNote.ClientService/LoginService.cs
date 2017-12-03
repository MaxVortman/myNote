using myNote.ClientService.Base;
using myNote.Model;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace myNote.ClientService
{
    public class LoginService : BaseService
    {
        internal LoginService(string connectionString) : base(connectionString) { }
        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="credential">user's credential</param>
        public void Register(Credential credential)
        {
            var response = client.PostAsJsonAsync("register", credential).Result;
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
        }
        /// <summary>
        /// Delete the user
        /// </summary>
        /// <param name="login">user's login</param>
        /// <param name="token">user's token</param>
        public void Delete(string login, Token token)
        {
            var responseMessage = client.PutAsJsonAsync($@"delete/{login}", token).Result;
            if(!responseMessage.IsSuccessStatusCode)
                throw new HttpRequestException(responseMessage.StatusCode.ToString() + "\n" + responseMessage.ReasonPhrase);
        }
        /// <summary>
        /// Login the user
        /// </summary>
        /// <param name="credential">user's credential</param>
        /// <returns>new user's access token</returns>
        public async Task<Token> LoginAsync(Credential credential)
        {             
            var response = await client.PutAsJsonAsync("login", credential);
            if(response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<Token>();
            throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
        }
    }
}