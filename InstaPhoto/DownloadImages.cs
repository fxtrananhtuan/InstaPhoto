using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace InstaPhoto
{
    public class DownloadImages
    {
        public System.Drawing.Image DownloadImageFromUrl(string imageUrl)
        {
            System.Drawing.Image image = null;

            try
            {
                  
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;

                System.Net.WebResponse webResponse = webRequest.GetResponse();

                System.IO.Stream stream = webResponse.GetResponseStream();

                image = System.Drawing.Image.FromStream(stream);

                webResponse.Close();
            }
            catch (Exception )
            {
                return null;
            }

            return image;
        }
        public void SaveImage(string filename, string url)
        {
            filename = filename.Trim('?');
            filename = System.IO.Path.Combine(filename, Img_name(url));
            Image img = DownloadImageFromUrl(url);
            img.Save(filename, ImageFormat.Bmp);
            
        }
        public string Img_name(string url)
        {
            string[] words = url.Split('/');
            string name = "";
            foreach (string word in words)
            {
                name = word;
            }
            words = name.Split('?');

            return words[0];
        }

    }
}
