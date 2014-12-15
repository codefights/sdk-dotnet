namespace CodeFights.SDK.SampleFighters
{
    using System;

    using CodeFights.SDK.Protocol;

    internal class Human : IFighter
    {
        public IFighterMove MakeNextMove(IFighterMove opponentsLastMove, int myLastScore, int opponentsLastScore)
        {
            PrintInstructions();

            while (true)
            {
                try
                {
                    var input = Console.ReadLine() ?? string.Empty;
                    var fighterMove = ParseInput(input.Trim().Replace(" ", string.Empty));
                    return fighterMove;
                }
                catch (ArgumentException ex)
                {
                    Console.Error.WriteLine("Human error: " + ex.Message);
                }
                catch (OperationCanceledException)
                {
                    Console.Error.WriteLine("Bye");
                    Environment.Exit(0);
                }
            }
        }

        private static void PrintInstructions()
        {
            Console.WriteLine("Make your move by (A)ttacking and (B)locking (N)ose, (J)aw, (B)elly, (G)roin, (L)eggs");
            Console.WriteLine("  (for example, BN BJ AN)");
            Console.Write(": ");
        }

        private static IFighterMove ParseInput(string input)
        {
            input = input.Replace("\\W", string.Empty).ToLowerInvariant();

            if (input.StartsWith("q"))
            {
                throw new OperationCanceledException("Exiting");
            }

            var fighterMove = Protocol.ParseMove(input);

            if (fighterMove.AttackedAreas.Count + fighterMove.BlockedAreas.Count > 3)
            {
                throw new ArgumentException("Can make max 3 things at a time!");
            }

            return fighterMove;
        }
    }
}