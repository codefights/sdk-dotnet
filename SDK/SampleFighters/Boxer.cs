namespace CodeFights.SDK.SampleFighters
{
    using System;

    using CodeFights.SDK.Protocol;

    internal class Boxer : IFighter
    {
        private static readonly Random _random = new Random();

        private Area _defenceArea = Area.Nose;

        private int _myScoreTotal;

        private int _opponentScoreTotal;

        public IFighterMove MakeNextMove(IFighterMove opponentsLastMove, int myLastScore, int opponentsLastScore)
        {
            _myScoreTotal += myLastScore;
            _opponentScoreTotal += opponentsLastScore;

            FighterMove fighterMove = new FighterMove().Attack(Area.Nose)
                                                       .Attack(Area.Jaw);

            if (opponentsLastMove != null)
            {
                if (opponentsLastMove.AttackedAreas.Contains(_defenceArea) == false)
                {
                    _defenceArea = ChangeDefence(_defenceArea);
                }
            }

            if (_myScoreTotal >= _opponentScoreTotal)
            {
                fighterMove.Attack(GetRandomArea()); // 3 attacks, 0 defence
            }
            else
            {
                fighterMove.Block(_defenceArea); // 2 attacks, 1 defence
            }

            return fighterMove;
        }

        private static Area ChangeDefence(Area oldDefence)
        {
            return (oldDefence == Area.Nose) ? Area.Jaw : Area.Nose;
        }

        private static Area GetRandomArea()
        {
            return _random.NextDouble() > 0.5d ? Area.Belly : Area.Jaw;
        }
    }
}