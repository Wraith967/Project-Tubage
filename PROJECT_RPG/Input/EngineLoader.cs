using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

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
            String NPentityPattern = "^NPentity:.*";
            String friend = "^friend:.*";
            String enemy = "^enemy:.*";
            String tilePattern = "^tilemap:.*";
            String transferPattern = "^transferPoint:.*";
            String songsPattern = "^songs:.*";
            while (line != null)
            {
                // use System.Text.RegularExpressions.Regex.IsMatch(String s, String pattern)
                if (System.Text.RegularExpressions.Regex.IsMatch(line, playerPattern))
                    LoadPlayer(line, PGS);
                else if (System.Text.RegularExpressions.Regex.IsMatch(line, NPentityPattern))
                    LoadNPEntity(line, PGS);
                else if (System.Text.RegularExpressions.Regex.IsMatch(line, friend))
                    LoadFriend(line, PGS);
                else if (System.Text.RegularExpressions.Regex.IsMatch(line, enemy))
                    LoadEnemy(line, PGS);
                else if (System.Text.RegularExpressions.Regex.IsMatch(line, tilePattern))
                    LoadTileMap(line, PGS);
                else if (System.Text.RegularExpressions.Regex.IsMatch(line, transferPattern))
                    LoadTransferPoints(line, PGS);
                else if (System.Text.RegularExpressions.Regex.IsMatch(line, songsPattern))
                    LoadSongs(line, PGS);
                line = reader.ReadLine();
            }
        }

        private static void LoadPlayer(String player, PlayableMainGameScreen PGS)
        {
            char[] delims = {'<','>'};
            String[] tokens = player.Substring(8).Split(delims);
            String texture = tokens[1];
            PlayerEntity pEntity;
            Vector2 playerPos = PGS.PlayerPos;
            pEntity = new PlayerEntity(texture, playerPos);
            PGS.AddEntity(pEntity);
        }

        private static void LoadNPEntity(String entity, PlayableMainGameScreen PGS)
        {
            char[] delims = { '<', '>' };
            String[] tokens = entity.Substring(8).Split(delims);
            String texture = tokens[1];
            float posX = float.Parse(tokens[3]);
            float posY = float.Parse(tokens[5]);
            NonPlayerEntity npEntity = new NonPlayerEntity(texture, new Vector2(posX, posY));
            PGS.AddEntity(npEntity);
        }

        private static void LoadFriend(String entity, PlayableMainGameScreen PGS)
        {
            char[] delims = { '<', '>' };
            String[] tokens = entity.Substring(8).Split(delims);
            String texture = tokens[1];
            float posX = float.Parse(tokens[3]);
            float posY = float.Parse(tokens[5]);
            String text = tokens[7];
            String filename = tokens[9];
            FriendlyEntity npEntity = new FriendlyEntity(texture, new Vector2(posX, posY), text, filename);
            PGS.AddEntity(npEntity);
        }

        private static void LoadEnemy(String entity, PlayableMainGameScreen PGS)
        {
            char[] delims = { '<', '>' };
            String[] tokens = entity.Substring(8).Split(delims);
            String texture = tokens[1];
            float posX = float.Parse(tokens[3]);
            float posY = float.Parse(tokens[5]);
            String filename = tokens[7];
            EnemyEntity npEntity = new EnemyEntity(texture, new Vector2(posX, posY), filename);
            PGS.AddEntity(npEntity);
        }

        private static void LoadTileMap(String tileMap, PlayableMainGameScreen PGS)
        {
            char[] delims = { '<', '>' };
            String[] tokens = tileMap.Substring(9).Split(delims);
            String mapTextureName = tokens[1];
            PGS.setTileMap(MapReader.readTileMap(mapTextureName, PGS));
        }

        private static void LoadTransferPoints(String transfer, PlayableMainGameScreen PGS)
        {
            char[] delims = { '<', '>' };
            String[] tokens = transfer.Substring(15).Split(delims);
            String nextScreen = tokens[1];
            int xCoord = Int16.Parse(tokens[3]);
            int yCoord = Int16.Parse(tokens[5]);
            int nextX = Int16.Parse(tokens[7]);
            int nextY = Int16.Parse(tokens[9]);
            PGS.setTransferPoint(nextScreen, xCoord, yCoord, nextX, nextY);
        }

        private static void LoadSongs(String songs, PlayableMainGameScreen PGS)
        {
            char[] delims = { '<', '>' };
            String[] tokens = songs.Substring(8).Split(delims);
            // First song will always be the default song to play.
            PGS.currentSong = tokens[1];
            PGS.Songs.Add(tokens[1]);
            PGS.Songs.Add(tokens[3]);
        }
    }
}
