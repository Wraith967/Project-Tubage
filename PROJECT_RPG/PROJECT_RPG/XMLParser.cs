using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PROJECT_RPG
{
    class XMLParser
    {
        public static void ParseXMLFile(List<GameEntity> list, String fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            List<String> tagList = new List<string>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                tagList.Add(line);
            }
            list.Add(ParseEntity(tagList));
        }

        private static GameEntity ParseEntity(List<String> list)
        {
            // TODO Add Regex coding to read in tags, then split strings by delimiter (tbd)
            GameEntity temp = new GameEntity("temp");
            return temp;
        }
    }
}
