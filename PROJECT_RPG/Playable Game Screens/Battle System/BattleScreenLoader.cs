using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PROJECT_RPG
{
    class BattleScreenLoader
    {
        public static void LoadBattle(String filename, BattleScreen BS)
        {
            StreamReader reader = new StreamReader(filename);
            String line = reader.ReadLine();

            // Regex patterns
            String player = "^player:.*";
            String enemy = "^enemy:.*";
            String laptop = "^laptop:.*";
            String screen = "^screen:.*";

            while (line != null)
            {
                // use System.Text.RegularExpressions.Regex.IsMatch(String s, String pattern)
                if (System.Text.RegularExpressions.Regex.IsMatch(line, player))
                    LoadPlayer(line, BS);
                else if (System.Text.RegularExpressions.Regex.IsMatch(line, enemy))
                    LoadEnemy(line, BS);
                else if (System.Text.RegularExpressions.Regex.IsMatch(line, laptop))
                    LoadLaptop(line, BS);
                else if (System.Text.RegularExpressions.Regex.IsMatch(line, screen))
                    LoadScreen(line, BS);
                line = reader.ReadLine();
            }
        }

        private static void LoadPlayer(String line, BattleScreen BS)
        {
            char[] delims = { '<', '>' };
            String[] tokens = line.Substring(8).Split(delims);
            String texture = tokens[1];
            int str = Int32.Parse(tokens[3]);
            int def = Int32.Parse(tokens[5]);
            int maxHP = Int32.Parse(tokens[7]);
            BattleScreenPlayer player = new BattleScreenPlayer(texture, BS);
            player.Strength = str;
            player.Defense = def;
            player.MaxHP = maxHP;
            BS.AddBattleMember(player);
        }

        private static void LoadEnemy(String line, BattleScreen BS)
        {
            char[] delims = { '<', '>' };
            String[] tokens = line.Substring(7).Split(delims);
            String texture = tokens[1];
            int str = Int32.Parse(tokens[3]);
            int def = Int32.Parse(tokens[5]);
            int maxHP = Int32.Parse(tokens[7]);
            BattleScreenEnemy enemy = new BattleScreenEnemy(texture, BS);
            enemy.Strength = str;
            enemy.Defense = def;
            enemy.MaxHP = maxHP;
            BS.AddBattleMember(enemy);
        }

        private static void LoadLaptop(String line, BattleScreen BS)
        {
            char[] delims = { '<', '>' };
            String[] tokens = line.Substring(8).Split(delims);
            String texture = tokens[1];
            int str = Int32.Parse(tokens[3]);
            int def = Int32.Parse(tokens[5]);
            int maxHP = Int32.Parse(tokens[7]);
            BattleScreenAIPartyMember laptop = new BattleScreenAIPartyMember(texture, BS);
            laptop.Strength = str;
            laptop.Defense = def;
            laptop.MaxHP = maxHP;
            BS.AddBattleMember(laptop);
        }

        private static void LoadScreen(String line, BattleScreen BS)
        {
            char[] delims = { '<', '>' };
            String[] tokens = line.Substring(8).Split(delims);
            String texture = tokens[1];
            BS.Texture = texture;
        }
    }
}
