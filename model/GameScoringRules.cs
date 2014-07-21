namespace CodeFights.model
{
    using System;
    using System.Collections.Generic;

    internal class GameScoringRules
    {

        public static int LIFEPOINTS = 150;

        public static int CalculateScore(IList<Area> attackAreas, IList<Area> blockAreas)
        {
            int rez = 0;

            if (attackAreas == null)
                return rez;

            foreach (Area attack in attackAreas)
                if (blockAreas.Contains(attack) == false)
                    rez += (int)attack;

            return rez;
        }

    }
}