namespace CodeFights.SDK.Protocol
{
    using System.Collections.Generic;

    public interface IFighterMove
    {
        IList<Area> AttackedAreas { get; }

        IList<Area> BlockedAreas { get; }

        string Comment { get; }
    }
}