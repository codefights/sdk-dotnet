namespace CodeFights.SDK.Protocol
{
    using System.Collections.Generic;
    using System.Text;

    public class FighterMove : IFighterMove
    {
        private readonly List<Area> _attackedAreas = new List<Area>();

        private readonly List<Area> _blockedAreas = new List<Area>();

        public IList<Area> AttackedAreas
        {
            get
            {
                return _attackedAreas;
            }
        }

        public IList<Area> BlockedAreas
        {
            get
            {
                return _blockedAreas;
            }
        }

        public string Comment { get; set; }

        public FighterMove Attack(Area area)
        {
            _attackedAreas.Add(area);
            return this;
        }

        public FighterMove Block(Area area)
        {
            _blockedAreas.Add(area);
            return this;
        }

        public FighterMove SetComment(string comment)
        {
            Comment = comment;
            return this;
        }

        public override string ToString()
        {
            var sb = new StringBuilder("Move ");

            foreach (var attackedArea in AttackedAreas)
            {
                sb.Append(" ATTACK " + attackedArea);
            }

            foreach (var blockedArea in BlockedAreas)
            {
                sb.Append(" BLOCK " + blockedArea);
            }

            if (!string.IsNullOrWhiteSpace(this.Comment))
            {
                sb.Append(" COMMENT " + this.Comment);
            }

            return sb.ToString();
        }
    }
}
