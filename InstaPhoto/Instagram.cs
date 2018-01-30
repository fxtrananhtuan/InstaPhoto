using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;



namespace InstaPhoto
{
    /// <summary>
    /// 
    /// </summary>
    public class Instagram
    {
        private string access_token;
        private DataTable dt_info = new DataTable();
        private AddInDatanase _add = new AddInDatanase();
        private DownloadImages _download = new DownloadImages();
        /// <summary>
        /// Instantiate this method with a valid access token.
        /// </summary>
        /// <param name="access_token">A valid access token</param>
        public Instagram(string access_token)
        {
            this.access_token = access_token;
        }

        /// <summary>
        /// Returns a JSON string of your resulting query. If the user parameter contains a valid username
        /// the method will return the user information, including the 'id' which you can use against the 
        /// other API endpoints in this class.
        /// </summary>
        /// <param name="user">Instagram username</param>
        /// <returns>string (json)</returns>
        public string getUserId(string user)
        {
            string output = "";
            string url = "https://api.instagram.com/v1/users";

            string urlRequest = "/search?q=" + user +"&access_token=";
            string accessToken = this.access_token;

             
            
            string fullUrl = url + urlRequest + accessToken;

            try
            {
                WebResponse response = processWebRequest(fullUrl);

                using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    JsonTextReader reader = new JsonTextReader(new StringReader(sr.ReadToEnd()));
                    while (reader.Read())
                    {
                        output += reader.Value;
                    }
                }

                return output;
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
        }
        

        /// <summary>
        /// This methods returns a Data Table containing the most recent collection of large and small
        /// images from the supplied user id. The two columns returned are, "LargeImage" and "SmallImage".
        /// You can bind this method to an ASP.NET Repeater server control to expose the data easily.
        /// </summary>
        /// <param name="user_id">user id of an instagram user. Example: "222680642"</param>
        /// <returns>DataTable with Columns, "LargeImage" and "SmallImage".</returns>
        public DataTable getMediaRecent(string user_id, int media_count = 50)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("LargeImage");
            dt.Columns.Add("SmallImage");
            dt.Columns.Add("Likes");
            dt.Columns.Add("Caption");
            dt.Columns.Add("Tags");

            string url = "https://api.instagram.com/v1/users";
            string id = user_id;
            string userId = String.Format("/{0}/", id);

            string urlRequest = "media/recent?count=" + media_count +"&access_token=";
            string accessToken = this.access_token;

            string fullUrl = url + userId + urlRequest + accessToken;

            WebResponse response = processWebRequest(fullUrl);

            using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
            {

                InstagramObject _instagram = JsonConvert.DeserializeObject<InstagramObject>(sr.ReadToEnd());

                int count = 0;
                int totalPhotos = _instagram.data.Count - 1;

                
                while (count < totalPhotos)
                {

                    string tags = "";

                    foreach (object o in _instagram.data[count].tags)
                    {
                        tags += "#" + o + ",";
                    
                    }

                    dt.Rows.Add(_instagram.data[count].images.standard_resolution.url, _instagram.data[count].images.low_resolution.url, _instagram.data[count].likes.count, _instagram.data[count].caption.text, tags.TrimEnd(new char[]{','}));
                    
                    count = count + 1;

                }

            }

            return dt;
        }

        public DataTable getImageTag(string tag)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("LargeImage");
            dt.Columns.Add("User_Name");
            dt.Columns.Add("Profile_picture");
            dt.Columns.Add("Caption");
            dt.Columns.Add("Tags");
            string url = "https://api.instagram.com/v1/tags/";
            string id = tag;
            string userId = String.Format("/{0}/", id);

            string urlRequest = "/media/recent?access_token=";
            string accessToken = this.access_token;

            string fullUrl = url + tag + urlRequest + accessToken;
            GCLass.URL = fullUrl;
            WebResponse response = processWebRequest(fullUrl);

            using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
            {

                InstagramObject _instagram = JsonConvert.DeserializeObject<InstagramObject>(sr.ReadToEnd());

                GCLass.Next_URL = _instagram.pagination.next_url;
                int count = 0;
                int totalPhotos = _instagram.data.Count - 1;


                while (count < totalPhotos)
                {

                    string tags = "";

                    foreach (object o in _instagram.data[count].tags)
                    {
                        tags += "#" + o + ",";

                    }

                    dt.Rows.Add(_instagram.data[count].images.standard_resolution.url, _instagram.data[count].user.username, _instagram.data[count].user.profile_picture, _instagram.data[count].caption.text, tags.TrimEnd(new char[] { ',' }));
                    if (_add.CheckImages(_instagram.data[count].images.standard_resolution.url))
                    {
                        _add.AddImages(_instagram.data[count].images.standard_resolution.url, _instagram.data[count].user.username, _instagram.data[count].user.profile_picture, _instagram.data[count].caption.text, tags.TrimEnd(new char[] { ',' }));
                        _download.SaveImage(GCLass.Path, _instagram.data[count].images.standard_resolution.url);
                    }
                    count = count + 1;

                }

            }
            dt_info = dt;
            return dt_info;
        }
        public DataTable getImageTag(string tag,string next)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("LargeImage");
            dt.Columns.Add("User_Name");
            dt.Columns.Add("Profile_picture");
            dt.Columns.Add("Caption");
            dt.Columns.Add("Tags");
            string fullUrl = GCLass.Next_URL;
            if (GCLass.Next_URL == null)
            {
                fullUrl = GCLass.URL;
            }
            WebResponse response = processWebRequest(fullUrl);

            using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
            {

                InstagramObject _instagram = JsonConvert.DeserializeObject<InstagramObject>(sr.ReadToEnd());
               

                GCLass.Next_URL = _instagram.pagination.next_url;
                int count = 0;
                int totalPhotos = _instagram.data.Count - 1;


                while (count < totalPhotos)
                {

                    string tags = "";

                    foreach (object o in _instagram.data[count].tags)
                    {
                        tags += "#" + o + ",";

                    }

                    string caption = "";
                    if (_instagram.data[count].caption==null)
                    {
                        dt.Rows.Add(_instagram.data[count].images.standard_resolution.url, _instagram.data[count].user.username, _instagram.data[count].user.profile_picture, "", tags.TrimEnd(new char[] { ',' }));
                    }
                    else
                    {
                        caption = _instagram.data[count].caption.text;
                        dt.Rows.Add(_instagram.data[count].images.standard_resolution.url, _instagram.data[count].user.username, _instagram.data[count].user.profile_picture, caption, tags.TrimEnd(new char[] { ',' }));
                    }
                   

                    if (_add.CheckImages(_instagram.data[count].images.standard_resolution.url))
                    {
                        _add.AddImages( _instagram.data[count].images.standard_resolution.url, _instagram.data[count].user.username, _instagram.data[count].user.profile_picture, caption, tags.TrimEnd(new char[] { ',' }));
                        _download.SaveImage(GCLass.Path, _instagram.data[count].images.standard_resolution.url);
                    }
                    count = count + 1;

                }

            }

            return dt;
        }



        private WebResponse processWebRequest(string url)
        {
            if(Execute.HasConnection())
            {
                WebRequest request;
                WebResponse response;

                request = WebRequest.Create(url);
                response = request.GetResponse();

                return response;
            }
            else
            {
                return null;
            }
           

        }
    }
}
