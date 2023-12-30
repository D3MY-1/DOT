using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOT.Models
{ 
    
    public class Type  
    {
        // PS SOMEHOW implement check for null values
        public string Name { get; set; }
        public string ImageName { get; set; }
        public List<string> Filters { get; set; }
        public List<Item>? Items { get; set; }
        public Stream LoadImage()
        {
            return DatabaseLoader.LoadImageFromAssets(ImageName);
        }
        

    }

    public class Item
    {
        // PS SOMEHOW implement check for null values
        public float Price { get; set; }
        public string Name { get; set; }
        public List<string> FilterValues { get; set; }
        public string ImageName { get; set; }
        public Stream LoadImage()
        {
            return DatabaseLoader.LoadImageFromAssets(ImageName);
        }
    }
}
