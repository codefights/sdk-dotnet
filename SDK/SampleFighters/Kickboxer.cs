namespace CodeFights.SDK.SampleFighters
{
    using System;

    using CodeFights.SDK.Protocol;

    internal class Kickboxer : IFighter
    {
        private static readonly Random _random = new Random();

        private Area _attackArea1 = Area.Jaw;

        private Area _attackArea2 = Area.Nose;

        public IFighterMove MakeNextMove(IFighterMove opponentsLastMove, int myLastScore, int opponentsLastScore)
        {
            if (opponentsLastMove != null && opponentsLastMove.BlockedAreas.Contains(_attackArea1))
            {
                _attackArea1 = GetRandomArea();
            }

            _attackArea2 = GetRandomArea();

            return new FighterMove().Attack(_attackArea1)
                                    .Attack(_attackArea2)
                                    .Block(Area.Nose);
        }

        private Area GetRandomArea()
        {
            double random = _random.NextDouble();

            if (random < 0.3)
            {
                return Area.Nose;
            }

            if (random < 0.7)
            {
                return Area.Jaw;
            }

            if (random < 0.9)
            {
                return Area.Groin;
            }

            return Area.Belly;
        }
    }
}