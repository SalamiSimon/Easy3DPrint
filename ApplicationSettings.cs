using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DPrint_SW
{
    internal class ApplicationSettings
    {
        public class CuraSettings
        {
            //private CuraSettings curaSettings = new CuraSettings();

            public string Path { get; set; } = "";
            public string FileType { get; set; } = "";

            public CuraSettings() { }

            public CuraSettings(string path, string fileType)
            {
                Path = path;
                FileType = fileType;
            }
        }
    }
}
