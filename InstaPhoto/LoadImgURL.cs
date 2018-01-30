using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;


namespace InstaPhoto
{
    public  class LoadImgURL
    {

        public Image LoadFrom_URL (string url)
        {
            Image image;
            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                image = Image.FromStream(stream);
            }
            return image;
        }
    }
}
