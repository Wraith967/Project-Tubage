using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PROJECT_RPG
{
    class PlayerStats
    {
        public static int Strength = 10;
        public static int Defense = 10;
        public static int MaximumEnergy = 100;
        public static int CurrentEnergy = 100;
        public static int CurrentXP = 0;
        public static int NextLevelXP = 100;
        public static int Level = 1;

        public static void AddXP(int xp)
        {
            CurrentXP += xp;
            if (CurrentXP == NextLevelXP)
            {
                Strength += 10;
                Defense += 10;
                MaximumEnergy += 50;
                CurrentEnergy += 50;
                NextLevelXP *= 2;
            }
        }
    }
}
