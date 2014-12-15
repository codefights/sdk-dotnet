namespace CodeFights.SDK.Protocol
{
    public interface IFighter
    {
        IFighterMove MakeNextMove(IFighterMove opponentsLastMove, int myLastScore, int opponentsLastScore);
    }
}