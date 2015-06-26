using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MihailGospodinov.WordPrediction.Web.Controllers
{
    public class WordPredictionController : ApiController
    {
        private Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        // GET api/wordprediction
        public IEnumerable<string> Get(string text)
        {
            text = System.Web.HttpContext.Current.Server.UrlDecode(text);
            return WordPrediction.Predict(text).OrderByDescending(a => a.Item2).Take(3).Select(a => a.Item1).ToList();
        }

        public void Post([FromBody]string text)
        {
            string txt = System.Web.HttpContext.Current.Request.Form["text"];
            using (var stream = GenerateStreamFromString(txt))
            {
                WordPrediction.Teach(stream);
            }
        }
    }
}
