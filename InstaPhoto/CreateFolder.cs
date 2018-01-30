using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InstaPhoto
{
    public class CreateFolder
    {

        public String Filename { get; set; }

        public CreateFolder (string filename)
        {
            filename = Filename;
            if (!File.Exists(filename))
            {
                Directory.CreateDirectory(filename);
            }
           
        }

      
    }
}
