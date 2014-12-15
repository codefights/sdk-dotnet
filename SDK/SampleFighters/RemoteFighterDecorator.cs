namespace CodeFights.SDK.SampleFighters
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    using CodeFights.SDK.Protocol;

    public class RemoteFighterDecorator : IFighter
    {
        private readonly int _port = 30000;

        private readonly string _ip;

        private TcpClient _client;

        private IFighter _fighter;

        private IFighterMove _lastMove;

        public RemoteFighterDecorator(IFighter fighter, string[] args)
        {
            _fighter = fighter;

            if (args.Length > 3)
            {
                int.TryParse(args[3], out _port);

                if (!args[2].Equals("--listen", StringComparison.OrdinalIgnoreCase) &&
                    !args[2].Equals("-l", StringComparison.OrdinalIgnoreCase))
                {
                    _ip = args[2];
                }
            }
            else if (args.Length > 2)
            {
                _ip = args[2];
            }
        }

        public IFighterMove MakeNextMove(IFighterMove opponentsLastMove, int myLastScore, int opponentsLastScore)
        {
            if (_client == null)
            {
                Start();
            }

            if (_lastMove != null)
            {
                SendMove(_lastMove);
                _lastMove = null;
                return ReceiveMove();
            }

            _lastMove = _fighter.MakeNextMove(opponentsLastMove, myLastScore, opponentsLastScore);
            return _lastMove;
        }

        private void Start()
        {
            if (_ip == null)
            {
                StartServer();
            }
            else
            {
                ConnectToServer(_ip);
            }
        }

        private void StartServer()
        {
            Console.WriteLine("Provoking a fight at {0}:{1}", IPAddress.Loopback, _port);
            var tcpListener = new TcpListener(IPAddress.Loopback, _port);

            try
            {
                tcpListener.Start();
            }
            catch(SocketException ex)
            {
                Console.WriteLine("{0} (Socket exception)", ex.Message);
                Environment.Exit(1);
            }

            Console.WriteLine("Waiting for opponent...");
            _client = tcpListener.AcceptTcpClient();
            Console.WriteLine("Opponent is found");
        }

        private void ConnectToServer(string ip)
        {
            Console.WriteLine("Looking for a fight at {0}:{1} ...", ip, _port);
            _client = new TcpClient();

            try
            {
                _client.Connect(IPAddress.Parse(ip), _port);
            }
            catch(SocketException)
            {
                Console.WriteLine("Nothing found (Connection refused)");
                Environment.Exit(1);
            }

            Console.WriteLine("Opponent is found");
        }

        private void SendMove(IFighterMove fighterMove)
        {
            var message = Protocol.SerializeMove(fighterMove);
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            var clientStream = _client.GetStream();
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

        private IFighterMove ReceiveMove()
        {
            var clientStream = _client.GetStream();
            byte[] buffer = new byte[4096];
            int bytesRead = 0;

            try
            {
                bytesRead = clientStream.Read(buffer, 0, 4096);
            }
            catch(Exception ex)
            {
                Console.WriteLine("error: " + ex.Message);
            }

            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            return Protocol.ParseMove(message);
        }
    }
}
