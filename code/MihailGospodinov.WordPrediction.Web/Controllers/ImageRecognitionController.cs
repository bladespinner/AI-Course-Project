using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MihailGospodinov.WordPrediction.Web.Controllers
{
    public class ImageRecognitionController : ApiController
    {
        // GET api/imagerecognition
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/imagerecognition/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/imagerecognition
        public double[] Post([FromBody]object text)
        {
            var txt = System.Web.HttpContext.Current.Request.Form["text"];
            txt = txt.Substring("data:image/png;base64,".Count());
            var imgData = Convert.FromBase64String(txt);
            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/") + "\\tempimg.png";
            var path1 = System.Web.Hosting.HostingEnvironment.MapPath("~/") + "\\tempimg.bmp";

            using (var img = System.IO.File.OpenWrite(path))
            {
                img.Write(imgData, 0, imgData.Count());
                img.Close();
                img.Dispose();
            }

            Image img1 = Image.FromFile(path);
            using (Bitmap b = new Bitmap(img1.Width, img1.Height))
            {
                b.SetResolution(img1.HorizontalResolution, img1.VerticalResolution);

                using (Graphics g = Graphics.FromImage(b))
                {
                    g.Clear(Color.White);
                    g.DrawImageUnscaled(img1, 0, 0);
                }

                b.Save(path1,System.Drawing.Imaging.ImageFormat.Bmp);
                // Now save b as a JPEG like you normally would
            }
            img1.Dispose();

            return ImageRecognitionContext.OCR.RecognizeImage(Image.FromFile(path1));
        }

        // PUT api/imagerecognition/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/imagerecognition/5
        public void Delete(int id)
        {
        }
    }
}
