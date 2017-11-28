using myNote.Model;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace myNote.WPFClient.ApiServices
{
    public class LoginService
    {

        private readonly HttpClient _client;

        public LoginService(string connectionString = IoC.IoC.ConnectionString)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(connectionString)
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void Register(Credential credential)
        {
            var response = _client.PostAsJsonAsync("register", credential).Result;
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
        }

        public void Delete(string login, Token token)
        {
            var responseMessage = _client.PutAsJsonAsync($@"delete/{login}", token).Result;
            if(!responseMessage.IsSuccessStatusCode)
                throw new HttpRequestException(responseMessage.StatusCode.ToString() + "\n" + responseMessage.ReasonPhrase);
        }

        public async Task<Token> Login(Credential credential)
        {             
            var response = await _client.PutAsJsonAsync("login", credential);
            if(response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<Token>();
            throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
        }
    }
}