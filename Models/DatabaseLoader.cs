using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;

namespace DOT.Models
{
    public class DatabaseLoader
    {
        private const string DBPATH = "db.json";
        private const string AssetsPath = "./Assets/";
        public DatabaseLoader()
        {
            string json = File.ReadAllText(AssetsPath + DBPATH); //Add error checking here.
            Types = JsonConvert.DeserializeObject<List<Type>>(json);


        }

        public List<Item> GetItemsByTypeName(string type)
        {
            return Types.Where(item => item.Name == type).First().Items; // Add error checking here.
        }

        public List<Type> GetTypes()
        {
            return Types;
        }

        public static Stream LoadImageFromAssets(string ImageName)
        {
            return File.OpenRead(AssetsPath + ImageName);
        }


        public List<Type> Types;

    }
}
