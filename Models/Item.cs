using System.Collections.Generic;
using System.IO;

namespace DOT.Models
{

    public class Type
    {
        // PS SOMEHOW implement check for null values
        public string Name { get; set; }
        public string ImageName { get; set; }
        public List<string> Filters { get; set; }
        public List<Item>? Items { get; set; }
        public Stream? LoadImage()
        {
            return DatabaseLoader.LoadImageFromAssets(ImageName, "");
        }

    }

    public class SubItem
    {
        public string ShopName { get; set; }
        public List<string> Colors { get; set; }
        public List<string> Sizes { get; set; }
        public float Price { get; set; }
    }


    public class Item
    {
        // PS SOMEHOW implement check for null values
        public string Name { get; set; }
        public List<string> FilterValues { get; set; }
        public List<SubItem> SubItems { get; set; }
        public Stream? LoadSomeImage(string color)
        {
            return DatabaseLoader.LoadImageFromAssets(Name, color);
        }
        public List<Stream>? LoadAllImages(string color)
        {
            return DatabaseLoader.LoadSequentialImages(Name, color);
        }
    }
}
