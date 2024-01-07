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
            if (File.Exists(AssetsPath + ImageName))
            {
                return File.OpenRead(AssetsPath + ImageName);
            }
            else
            {
                Logger.Instance.Log($"Didn't find image : {ImageName}");
                return null;
            }
            
            
        }


        public List<Type> Types;

    }
}
