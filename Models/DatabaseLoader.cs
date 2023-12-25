using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOT.Models
{
    public class DatabaseLoader
    {
        public DatabaseLoader()
        {
            Items = new()
            {
                new Item("Shoes","Air flow",new List<string>{"Air_flow" }),
                new Item("Shoes","Air kicker",new List < string > { "Air_kicker" }),
                new Item("Shoes","Mega flow",new List < string > { "Mega_flow" }),
                new Item("Shoes","Mega flow",new List < string > { "Mega_flow" }),
                new Item("Shoes","Mega flow",new List < string > { "Mega_flow" }),
                new Item("Shoes","Mega flow",new List < string > { "Mega_flow" }),
                new Item("Shoes","Mega flow",new List < string > { "Mega_flow" }),
                new Item("Shoes","Mega flow",new List < string > { "Mega_flow" }),
                new Item("Shoes","Mega flow",new List < string > { "Mega_flow" }),
                new Item("Shoes","Mega flow",new List < string > { "Mega_flow" }),
                new Item("Shoes","Mega flow",new List < string > { "Mega_flow" }),
                new Item("Shoes","Mega flow",new List < string > { "Mega_flow" }),
                new Item("Shoes","Mega flow",new List < string > { "Mega_flow" }),
                new Item("Shoes","Mega flow",new List < string > { "Mega_flow" }),
                new Item("Shoes","Mega flow",new List < string > { "Mega_flow" }),
                new Item("Shoes","Mega flow",new List < string > { "Mega_flow" }),
                new Item("Jach","Ultra flow",new List < string > { "Ultra_flow" })
            };
        }

        public List<Item> GetItemByType(string type)
        {
            return Items.Where(item => item.type == type).ToList();
        }

        static public List<Item> Items;
    }
}
