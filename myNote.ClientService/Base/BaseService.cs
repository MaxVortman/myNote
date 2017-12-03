using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace myNote.ClientService.Base
{
    public abstract class BaseService
    {
        protected readonly HttpClient client;
        #region Default Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="connectionString">Lint of connection to server</param>
        protected BaseService(string connectionString)
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(connectionString)
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        #endregion
    }
}
