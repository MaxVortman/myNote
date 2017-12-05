using myNote.ClientService.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace myNote.ClientService
{
    public class ImageService : BaseService
    {
        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="connectionString"></param>
        internal ImageService(string connectionString) : base(connectionString){ }
        #endregion

        /// <summary>
        /// Getting image bytes from server by userid
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<byte[]> GetImageBytes(Guid userId)
        {           
            var response = await client.GetAsync($@"image/{userId}");
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
            return await response.Content.ReadAsAsync<byte[]>();
        }
        /// <summary>
        /// Post image on server by bytes <see cref="imageBytes"/>
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <param name="userId">id of user who post image</param>
        /// <returns>path of image on server file system</returns>
        public async void PostImage(byte[] imageBytes, Guid userId)
        {
            var response = await client.PostAsJsonAsync($@"image/user/{userId}", imageBytes);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException(response.StatusCode.ToString() + "\n" + response.ReasonPhrase);
        }
    }
}
