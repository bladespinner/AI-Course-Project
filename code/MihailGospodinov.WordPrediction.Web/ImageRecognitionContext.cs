using Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MihailGospodinov.WordPrediction.Web
{
    public static class ImageRecognitionContext
    {
        private static OCR _ocr;
        public static OCR OCR
        {
            get
            {
                if(_ocr == null)
                {
                    _ocr = new OCR("", 1);
                    var path = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/nn00_0");
                    Random rnd = new Random();
                    _ocr.GenerateFromFiles(rnd, new[] { path });
                }
                return _ocr;
            }
        }
    }
}