namespace CodeFights.SDK.Runners
{
    using System;
    using System.IO;
    using System.Text;

    using CodeFights.SDK.Protocol;

    public class ServerModeFightDriver
    {
        private readonly TextReader _inStream;

        private readonly TextWriter _outStream;

        public ServerModeFightDriver(TextReader inStream, TextWriter outStream)
        {
            _outStream = outStream;
            _inStream = inStream;
        }

        public void SendHandshake()
        {
            _outStream.WriteLine(Protocol.HandshakeCode);
        }

        public void SendRequest(IFighterMove fighterMove)
        {
            _outStream.WriteLine(Protocol.RequesHeaderCode + SerializeMove(fighterMove));
        }

        public ServerResponseResult ReadResponse()
        {
            return ParseResponse(_inStream.ReadLine());
        }

        private static string SerializeMove(IFighterMove fighterMove)
        {
            var sb = new StringBuilder();

            foreach (Area attackedArea in fighterMove.AttackedAreas)
            {
                sb.Append("a" + attackedArea.ToString()[0]);
            }

            foreach (Area blockedArea in fighterMove.BlockedAreas)
            {
                sb.Append("b" + blockedArea.ToString()[0]);
            }

            if (fighterMove.Comment != null && fighterMove.Comment.Trim() != string.Empty)
            {
                sb.Append("c" + fighterMove.Comment.Trim());
            }

            return sb.ToString().ToLowerInvariant();
        }

        private static ServerResponseResult ParseResponse(string line)
        {
            var result = new ServerResponseResult();

            string[] words = line.Split(' ');
            int index = 0;

            while (index < words.Length)
            {
                string firstKeyword = words[index++];

                if (index >= words.Length)
                {
                    throw new ArgumentException("Insufficient params. Syntax is [YOUR-SCORE area] [OPPONENT-SCORE area] [ENEMY-MOVE move]");
                }

                string nextKeyword = words[index++];

                if (Protocol.YourScoreCode.Equals(firstKeyword))
                {
                    int.TryParse(nextKeyword, out result.Score1);
                }
                else if (Protocol.OpponentScoreCode.Equals(firstKeyword))
                {
                    int.TryParse(nextKeyword, out result.Score2);
                }
                else if (Protocol.EnemyMoveCode.Equals(firstKeyword))
                {
                    result.FighterMove = Protocol.ParseMove(nextKeyword);
                }
                else
                {
                    throw new ArgumentException("invalid keyword " + firstKeyword + ". Syntax is [YOUR-SCORE area] [OPPONENT-SCORE area] [ENEMY-MOVE move]");
                }
            }

            return result;
        }

        public class ServerResponseResult
        {
            public IFighterMove FighterMove;

            public int Score1;

            public int Score2;
        }
    }
}