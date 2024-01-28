using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Windows;
using System.Security.Principal;

//Error displaying in this function is only viable in windows.

namespace DOT.Models
{
    public class DatabaseLoader
    {
        private const string DBPATH = "db.json";
        private const string AssetsPath = "./Assets/";


        
        public DatabaseLoader()
        {
            Logger.Instance.Log("DatabaseLoader Initializing");
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
                Logger.Instance.Log($"Error Message : {ex.Message}");
            }

        }

        public List<Type> GetTypes()
        {
            return Types;
        }

        public static Stream? LoadImageFromAssets(string ImageName,bool many = false)
        {
            ImageName = ImageName + ".png";
            if (File.Exists(AssetsPath + ImageName))
            {
                return File.OpenRead(AssetsPath + ImageName);
            }
            else
            {
                if(!many)
                    Logger.Instance.Log($"Didn't find image : {ImageName}");
                return null;
            }
            
            
        }

        public static List<Stream>? LoadSequentialImages(string startImageName)
        {
            int i = 1;
            List<Stream> images = new List<Stream>();
            var im = LoadImageFromAssets(startImageName);
            if(im != null)
            {
                images.Add(im);
            }else
            {
                return null;
            }
            while (true)
            {
                var newName = startImageName + i;
                im = LoadImageFromAssets(newName,true);
                if (im == null)
                {
                    return images;
                }
                images.Add(im);

            }
        }


        public List<Type> Types;

    }
}
