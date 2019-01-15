using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MTCPlaybackGMap
{
    public class XMLData
    {
        public bool fileExists()
        {
            string fileName = Directory.GetCurrentDirectory() + "\\BeatData.xml";
            if (File.Exists(fileName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void writeFile()
        {
 
        }
    }

    public class xmlFileData
    {
        public string beatNumber { get; set; }
        public List<latLonPair> points { get; set; }
    }

    public class latLonPair
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }
}
