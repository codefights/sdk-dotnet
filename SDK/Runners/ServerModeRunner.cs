namespace CodeFights.SDK.Runners
{
    using System.IO;

    using CodeFights.SDK.Protocol;

    public class ServerModeRunner : IFighterRunner
    {
        private readonly TextReader _inStream;

        private readonly TextWriter _outStream;

        public ServerModeRunner(TextReader inStream, TextWriter outStream)
        {
            _inStream = inStream;
            _outStream = outStream;
        }

        public void Run(IFighter fighter)
        {
            var serverModeFightDriver = new ServerModeFightDriver(_inStream, _outStream);
            serverModeFightDriver.SendHandshake();

            var resp = new ServerModeFightDriver.ServerResponseResult();

            while (true)
            {
                var fighterMove = fighter.MakeNextMove(resp.FighterMove, resp.Score1, resp.Score2);
                serverModeFightDriver.SendRequest(fighterMove);
                resp = serverModeFightDriver.ReadResponse();
            }
        }
    }
}
