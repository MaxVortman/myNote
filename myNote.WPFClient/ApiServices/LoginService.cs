using myNote.Model;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace myNote.WPFClient.ApiServices
{
    public class LoginService
    {

        private readonly HttpClient client;
        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="connectionString">Link of connection to server</param>
        public LoginService(string connectionString = IoC.IoC.ConnectionString)
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(connectionString)
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #endregion

        public void Register(Credential credential)
        {
            var response = client.PostAsJsonAsync("register", credential).Result;
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
        }

        public void Delete(string login, Token token)
        {
            var responseMessage = client.PutAsJsonAsync($@"delete/{login}", token).Result;
            if(!responseMessage.IsSuccessStatusCode)
                throw new HttpRequestException(responseMessage.StatusCode.ToString() + "\n" + responseMessage.ReasonPhrase);
        }

        public async Task<Token> LoginAsync(Credential credential)
        {             
            var response = await client.PutAsJsonAsync("login", credential);
            if(response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<Token>();
            throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
        }
    }
}