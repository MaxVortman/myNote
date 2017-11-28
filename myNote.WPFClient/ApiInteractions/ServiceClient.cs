using myNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace myNote.WPFClient.ApiInteractions
{
    public class ServiceClient
    {
        private readonly HttpClient _client;

        public ServiceClient(string connectionString)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(connectionString)
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async void Register(Credential credential)
        {
            await _client.PostAsJsonAsync("register", credential);
        }

        public async void Delete(string login, Token token)
        {
            await _client.PostAsJsonAsync($@"delete/{login}", token);
        }

        public async Task<Token> Login(Credential credential)
        {
            var response = await _client.PostAsJsonAsync("login", credential);
            return await response.Content.ReadAsAsync<Token>();
        }
    }

}
