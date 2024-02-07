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
            return DatabaseLoader.LoadImageFromAssets(ImageName);
        }
        public List<Stream>? LoadAllImages()
        {
            return DatabaseLoader.LoadSequentialImages(ImageName);
        }

    }

    public class Item
    {
        // PS SOMEHOW implement check for null values
        public float Price { get; set; }
        public string Name { get; set; }
        public List<string> FilterValues { get; set; }
        public List<string> colors { get; set; }
        public Stream? LoadImage()
        {
            return DatabaseLoader.LoadImageFromAssets(Name);
        }
        public List<Stream>? LoadAllImages()
        {
            return DatabaseLoader.LoadSequentialImages(Name);
        }
    }
}
