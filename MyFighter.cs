namespace CodeFights
{
    using CodeFights.SDK.Protocol;

    public class MyFighter : IFighter
    {
        public IFighterMove MakeNextMove(IFighterMove opponentsLastMove, int myLastScore, int opponentsLastScore)
        {
            return new FighterMove().Attack(Area.Nose)
                                    .Attack(Area.Jaw)
                                    .Block(Area.Jaw);
        }
    }
}
