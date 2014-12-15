namespace CodeFights.SDK.Runners
{
    using System.IO;
    using System.Text;

    using CodeFights.SDK.Protocol;

    public class ArenaCommentator
    {
        private readonly TextWriter _outStream;

        private string _nameFighter1 = "Fighter1";

        private string _nameFighter2 = "Fighter2";

        private int _lifePointsFighter1 = GameScoringRules.LifePointsPerFight;

        private int _lifePointsFighter2 = GameScoringRules.LifePointsPerFight;

        public ArenaCommentator(TextWriter outStream)
        {
            _outStream = outStream;
        }

        public void SetFighterNames(string nameFighter1, string nameFighter2)
        {
            _nameFighter1 = nameFighter1;
            _nameFighter2 = nameFighter2;
        }

        public void DescribeRound(IFighterMove move1, IFighterMove move2, int score1, int score2)
        {
            DescribeMove(_nameFighter1, move1, score1, move2);
            DescribeMove(_nameFighter2, move2, score2, move1);

            _lifePointsFighter1 -= score2;
            _lifePointsFighter2 -= score1;

            _outStream.WriteLine(this._nameFighter1 + " vs " + this._nameFighter2 + ": " + this._lifePointsFighter1 + " to " + this._lifePointsFighter2);
            _outStream.WriteLine();
        }

        public void GameOver(int lifePointsFighter1, int lifePointsFighter2)
        {
            _outStream.WriteLine("FIGHT OVER");

            if (lifePointsFighter1 > lifePointsFighter2)
            {
                _outStream.WriteLine("THE WINNER IS " + this._nameFighter1);
            }
            else if (lifePointsFighter2 > lifePointsFighter1)
            {
                _outStream.WriteLine("THE WINNER IS " + this._nameFighter2);
            }
            else
            {
                _outStream.WriteLine("IT'S A DRAW!!!");
            }
        }

        private static string DescribeAttacks(IFighterMove fighterMove, IFighterMove counterMove, int score)
        {
            if (fighterMove.AttackedAreas.Count <= 0)
            {
                return " did NOT attack at all ";
            }

            var sb = new StringBuilder(" attacked ");

            foreach (var attack in fighterMove.AttackedAreas)
            {
                sb.Append(attack);
                sb.Append(counterMove.BlockedAreas.Contains(attack) ? "(-), " : "(+), ");
            }

            sb.Append(" scoring " + score);
            return sb.ToString();
        }

        private static string DescribeBlocks(IFighterMove fighterMove)
        {
            if (fighterMove.BlockedAreas.Count <= 0)
            {
                return "  and was NOT defending at all.";
            }

            var sb = new StringBuilder(" while defending ");

            foreach (var defence in fighterMove.BlockedAreas)
            {
                sb.Append(defence + ", ");
            }

            return sb.ToString();
        }

        private void DescribeMove(string fighterName, IFighterMove fighterMove, int score, IFighterMove counterMove)
        {
            _outStream.WriteLine(fighterName + DescribeAttacks(fighterMove, counterMove, score) + DescribeBlocks(fighterMove));
        }
    }
}