namespace CodeFights
{
    using System;

    using CodeFights.SDK.Runners;

    public class Program
    {
        private const string FightHumanSwitch = "--fight-me";
        private const string FightBotSwitch = "--fight-bot";
        private const string RunOnServerSwitch = "--fight-on-server";

        public static void Main(string[] args)
        {
            IFighterRunner runner = null;

            if (IsFightHumanMode(args))
            {
                runner = new ArenaRunner(new[] { FightBotSwitch, "human" }, Console.Out);
            }
            else if (IsFightBotMode(args))
            {
                runner = new ArenaRunner(args, Console.Out);
            }
            else if (IsRunInServerMode(args))
            {
                runner = new ServerModeRunner(Console.In, Console.Out);
            }

            if (runner != null)
            {
				runner.Run(new MyFighter());
            }
            else
            {
                PrintUsageInstructions(args);
            }
        }

        private static bool IsRunInServerMode(string[] args)
        {
            return args.Length == 1 && args[0].Equals(RunOnServerSwitch, StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool IsFightBotMode(string[] args)
        {
            return args.Length >= 2 && args[0].Equals(FightBotSwitch, StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool IsFightHumanMode(string[] args)
        {
            return args.Length == 1 && args[0].Equals(FightHumanSwitch, StringComparison.InvariantCultureIgnoreCase);
        }

        private static void PrintUsageInstructions(string[] args)
        {
            const string UsageInstructions = FightHumanSwitch + "\t\t\truns your bot against you in interactive mode\n" +
                                             FightBotSwitch + " boxer\t\truns your bot against a built-in boxer bot\n" +
                                             FightBotSwitch + " kickboxer\t\truns your bot against a built-in kickboxer bot\n" +
                                             FightBotSwitch + " remote -l <port> \truns your bot against remote opponent in server mode\n" +
                                             FightBotSwitch + " remote <ip> <port> \tconnects to remote opponent bot\n" +
                                             RunOnServerSwitch + "\t\truns your bot on codefights.net server";

            if (args.Length > 0)
            {
                Console.Out.Write("unrecognized option(s): ");

                foreach (string arg in args)
                {
                    Console.Out.Write(arg + " ");
                }

                Console.Out.WriteLine();
            }

            Console.Out.WriteLine(UsageInstructions);
        }
    }
}
