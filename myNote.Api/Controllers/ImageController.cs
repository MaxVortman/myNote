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
        /// <returns>image byte array</returns>
        [HttpGet]
        [Route("api/image/{userId}")]
        public byte[] Get(Guid userId)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    var image = Image.FromFile(@"C:\Users\User\Source\Repos\myNote\myNote.Api\Images\user-blue.jpg");
                    var path = Path.Combine(@"C:\Users\User\Source\Repos\myNote\myNote.Api\Images", userId.ToString() + ".bmp");
                    if (File.Exists(path))
                        image = Image.FromFile(path);
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    return ms.ToArray();
                }
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
        [Route("api/image/user/{userId}")]
        public void Post([FromBody]byte[] imageBytes, Guid userId)
        {
            using (var ms = new MemoryStream(imageBytes))
            {
                var path = $@"C:\Users\User\Source\Repos\myNote\myNote.Api\Images\{userId}.bmp";
                if (File.Exists(path))
                    File.Delete(path);
                var image = Image.FromStream(ms);
                image.Save(path);
            }
        }
    }
}
