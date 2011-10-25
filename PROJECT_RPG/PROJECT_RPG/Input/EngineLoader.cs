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
            float posX = float.Parse(tokens[2]);
            float posY = float.Parse(tokens[4]);
            PlayerEntity pEntity = new PlayerEntity(texture, new Microsoft.Xna.Framework.Vector2(posX,posY));
            PGS.AddEntity(pEntity);
        }

        private static void LoadEntity(String entity, PlayableMainGameScreen PGS)
        {
            char[] delims = { '<', '>' };
            String[] tokens = entity.Substring(8).Split(delims);
            String texture = tokens[0];
            float posX = float.Parse(tokens[2]);
            float posY = float.Parse(tokens[4]);
            NonPlayerEntity npEntity = new NonPlayerEntity(texture, new Microsoft.Xna.Framework.Vector2(posX, posY));
            PGS.AddEntity(npEntity);
        }

        private static void LoadTileMap(String tileMap, PlayableMainGameScreen PGS)
        {
            char[] delims = { '<', '>' };
            String[] tokens = tileMap.Substring(9).Split(delims);
            String mapTextureName = tokens[0];
            PGS.setTileMap(MapReader.readTileMap(mapTextureName, PGS));
        }

        private static void LoadTransferPoints(String transfer, PlayableMainGameScreen PGS)
        {
            char[] delims = { '<', '>' };
            String[] tokens = transfer.Substring(15).Split(delims);
            String nextScreen = tokens[0];
            int xCoord = Int16.Parse(tokens[2]);
            int yCoord = Int16.Parse(tokens[4]);
            int nextX = Int16.Parse(tokens[6]);
            int nextY = Int16.Parse(tokens[8]);
            PGS.setTransferPoint(nextScreen, xCoord, yCoord, nextX, nextY);
        }
    }
}
