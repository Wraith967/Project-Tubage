using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PROJECT_RPG
{
    class EngineLoader
    {
        /**
         * This method should be called by the constructor for PlayableGameScreen ONLY.
         * All other game entities that require a text parser have their own (ie, TileMapParser).
         * So, first line of the method should be to create the Screen, then add stuff
         * as the script file declares it.
         * 
         * */
        public static void LoadScriptFile(String filename, PlayableMainGameScreen PGS)
        {
            StreamReader reader = new StreamReader(filename);
            String line = reader.ReadLine();
            
            // Regex patterns
            String playerPattern = "^player:.*";
            String entityPattern = "^entity:.*";
            String tilePattern = "^tilemap:.*";
            String transferPattern = "^transferPoint:.*";
            while (line != null)
            {
                // use System.Text.RegularExpressions.Regex.IsMatch(String s, String pattern)
                if (System.Text.RegularExpressions.Regex.IsMatch(line,  playerPattern))
                    LoadPlayer(line, PGS);
                else if (System.Text.RegularExpressions.Regex.IsMatch(line, entityPattern))
                    LoadEntity(line, PGS);
                else if (System.Text.RegularExpressions.Regex.IsMatch(line, tilePattern))
                    LoadTileMap(line, PGS);
                else if (System.Text.RegularExpressions.Regex.IsMatch(line, transferPattern))
                    LoadTransferPoints(line, PGS);
            }
        }

        private static void LoadPlayer(String player, PlayableMainGameScreen PGS)
        {
            char[] delims = {'<','>'};
            String[] tokens = player.Substring(8).Split(delims);
            String texture = tokens[0];
            float posX = float.Parse(tokens[1]);
            float posY = float.Parse(tokens[2]);
            PlayerEntity pEntity = new PlayerEntity(texture, new Microsoft.Xna.Framework.Vector2(posX,posY));
            PGS.AddEntity(pEntity);
        }

        private static void LoadEntity(String entity, PlayableMainGameScreen PGS)
        {
            char[] delims = { '<', '>' };
            String[] tokens = entity.Substring(8).Split(delims);
            String texture = tokens[0];
            float posX = float.Parse(tokens[1]);
            float posY = float.Parse(tokens[2]);
            //DrawableEntity dEntity = new DrawableEntity(texture);
            //PGS.AddEntity(dEntity);
        }

        private static void LoadTileMap(String tileMap, PlayableMainGameScreen PGS)
        {
        }

        private static void LoadTransferPoints(String transfer, PlayableMainGameScreen PGS)
        {
        }
    }
}
