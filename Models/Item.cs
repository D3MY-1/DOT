using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOT.Models
{
    public class Item
    {
        public Item(string type, string name,List<string> ImageNames)
        {
            this.type = type;
            this.name = name;
            this.ImageNames = ImageNames;
        }

        List<string> ImageNames;

        private string AssetsPath = "./Assets/";

        public Stream LoadMainImage()
        {
            return File.OpenRead(AssetsPath + ImageNames[0] +  ".png");
        }

        public List<Stream> LoadSecondaryImages()
        {
            List<Stream> ret = new();
            if (ImageNames.Count > 1) 
            {
                for (int i = 1;i < ImageNames.Count;i++)
                {
                    ret.Add(File.OpenRead(AssetsPath + ImageNames[i] + ".png"));
                }
            }
            return ret;
        }


        public string type { get; }

        public string name {  get; }

        public string mainImageName { get; }

    }
}
