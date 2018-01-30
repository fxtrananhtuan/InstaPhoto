using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace InstaPhoto
{
    public class Execute
    {
        Instagram _insta = new Instagram("258559306.da06fb6.c222db6f1a794dccb7a674fec3f0941f");
        DownloadImages _down = new DownloadImages();
        

        public DataTable List_information (string tag)
        {
            DataTable tb = new DataTable();
            tb = _insta.getImageTag(tag);

            return tb;
        }
        public DataTable List_timer (string tag)
        {
            DataTable tb = new DataTable();
            tb = _insta.getImageTag(tag, "");
            return tb;
        }


        public void Create_folder (string path)
        {
            try
            {
                // Determine whether the directory exists. 
                if (Directory.Exists(path))
                {
                    Console.WriteLine("That path exists already.");
                    return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));

                // Delete the directory.
               // di.Delete();
               // Console.WriteLine("The directory was deleted successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }
        }
        public static void Savevarible(string path, string value)
        {
            string[] lines = { value };
            // WriteAllLines creates a file, writes a collection of strings to the file, 
            // and then closes the file.  You do NOT need to call Flush() or Close().
            System.IO.File.WriteAllLines(path + "//ID.txt", lines);

        }
        public static int Readvarible(string path)
        {
            string text = System.IO.File.ReadAllText(GCLass._path_ID + "//ID.txt");
            text = text.Replace("\r\n", string.Empty);

            if (text == "")
                return 0;
            return Convert.ToInt32(text);
        }

        public static bool HasConnection()
        {
            try
            {
                System.Net.IPHostEntry i = System.Net.Dns.GetHostEntry("www.google.com");
                return true;
            }
            catch
            {
                return false;
            }
        }




    }
}
