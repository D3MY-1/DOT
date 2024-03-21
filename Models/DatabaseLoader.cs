using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

//Error displaying in this function is only viable in windows.

namespace DOT.Models
{
    public class DatabaseLoader
    {
        private const string DBPATH = "db.json";
        private const string AssetsPath = "./Assets/";



        public DatabaseLoader()
        {
            _ = Logger.Instance.Log("DatabaseLoader Initializing");
            try
            {
                if (!File.Exists(AssetsPath + DBPATH))
                {
                    throw new FileNotFoundException("The database file could not be found.");
                }

                string json = File.ReadAllText(AssetsPath + DBPATH);

                if (string.IsNullOrEmpty(json))
                {
                    throw new InvalidDataException("The database file is empty or could not be read.");
                }

                Types = JsonConvert.DeserializeObject<List<Type>>(json);
            }
            catch (Exception ex)
            {
                _ = Logger.Instance.Log($"Error Message : {ex.Message}");
            }

        }

        public List<Type> GetTypes()
        {
            return Types;
        }

        public static Stream? LoadImageFromAssets(string ImageName, string color, bool many = false) // Need to remake for jpg
        {
            if (color.Length > 0)
            {
                ImageName = ImageName + color + ".jpg";
            }
            else
                ImageName = ImageName + ".jpg";

            if (File.Exists(AssetsPath + ImageName))
            {
                return File.OpenRead(AssetsPath + ImageName);
            }
            else
            {
                if (!many)
                    _ = Logger.Instance.Log($"Didn't find image : {ImageName}");
                return null;
            }


        }

        public static List<Stream>? LoadSequentialImages(string startImageName, string color = "")
        {
            int i = 1;
            List<Stream> images = new List<Stream>();
            var im = LoadImageFromAssets(startImageName, color);
            if (im != null)
            {
                images.Add(im);
            }
            else
            {
                return null;
            }
            while (true)
            {
                var newName = startImageName + color + i;
                im = LoadImageFromAssets(newName, "", true);
                if (im == null)
                {
                    return images;
                }
                images.Add(im);
                i++;

            }
        }


        public List<Type> Types;

    }
}
