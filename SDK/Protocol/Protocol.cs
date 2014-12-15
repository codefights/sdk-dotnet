namespace CodeFights.SDK.Protocol
{
    using System;

    public static class Protocol
    {
        public const string EnemyMoveCode = "ENEMY-MOVE";

        public const string HandshakeCode = "I-AM ready";

        public const string OpponentScoreCode = "OPPONENT-SCORE";

        public const string YourScoreCode = "YOUR-SCORE";

        public const string RequesHeaderCode = "";

        public static IFighterMove ParseMove(string input)
        {
            var fighterMove = new FighterMove();

            if (string.IsNullOrWhiteSpace(input))
            {
                return fighterMove;
            }

            int index = 0;

            while (index < input.Length)
            {
                var action = input[index++];

                switch (action)
                {
                    case 'a':
                        fighterMove.Attack(GetArea(input, index++));
                        break;
                    case 'b':
                        fighterMove.Block(GetArea(input, index++));
                        break;
                    case 'c':
                        fighterMove.SetComment(input.Substring(index));
                        index = input.Length + 1;
                        break;
                    default:
                        throw new ArgumentException("Unrecognized input: " + action);
                }
            }

            return fighterMove;
        }

        private static Area GetArea(string line, int index)
        {
            if (index >= line.Length)
            {
                throw new ArgumentException("Must also specify attack/defence area!");
            }

            switch (line[index])
            {
                case 'n':
                    return Area.Nose;
                case 'j':
                    return Area.Jaw;
                case 'b':
                    return Area.Belly;
                case 'g':
                    return Area.Groin;
                case 'l':
                    return Area.Legs;
                default:
                    throw new ArgumentException("Unrecognized area: " + line[index]);
            }
        }
    }
}