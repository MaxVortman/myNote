using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace myNote.Api.Controllers
{
    public class ImageController : ApiController
    {        
        /// <summary>
        /// Get image from server by path
        /// </summary>
        /// <param name="path">image path on server</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/image/{path}")]
        public ImageSource Get(string path)
        {
            try
            {
                return new BitmapImage(new Uri(path));
            }
            catch (Exception e)
            {
                Logger.Log.Instance.Error(e.Message);
                throw;
            }
        }
        /// <summary>
        /// Post image in server
        /// </summary>
        /// <param name="imageBytes">byte array fro image</param>
        /// <param name="userId">id of user who post image</param>
        /// <returns>path of image in server file system</returns>
        [HttpPost]
        [Route("api/image/{userId}")]
        public string Post(byte[] imageBytes, Guid userId)
        {
            var path = $"pack://application:,,,/Images/{userId}.bmp";
            var image = Image.FromStream(new MemoryStream(imageBytes));
            image.Save(path);
            return path;
        }
    }
}
