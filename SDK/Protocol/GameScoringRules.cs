namespace CodeFights.SDK.Protocol
{
    using System.Collections.Generic;
    using System.Linq;

    public static class GameScoringRules
    {
        public const int LifePointsPerFight = 150;

        public static int CalculateScore(IList<Area> attackAreas, IList<Area> blockAreas)
        {
            return (attackAreas == null)
                ? 0
                : attackAreas.Where(attackedArea => !blockAreas.Contains(attackedArea)).Sum(area => (int)area);
        }

        public static bool IsInvalidMove(IFighterMove fighterMove)
        {
            return (fighterMove.AttackedAreas.Count + fighterMove.BlockedAreas.Count) > 3;
        }
    }
}
