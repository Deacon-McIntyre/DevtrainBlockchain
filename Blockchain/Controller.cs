using System;

namespace Blockchain
{
    public class Controller
    {
        private readonly LedgerWriter _writer;
        private readonly AccountManager _accountManager;

        public Controller()
        {
            _accountManager = new AccountManager();
            _writer = new LedgerWriter(_accountManager);
        }

        public void Start()
        {
            while (true)
            {
                Console.WriteLine("Enter command:");
                var command = Console.ReadLine();
                ParseCommand(command);
            }
        }

        private void ParseCommand(string command)
        {
            var commandPieces = command.Split(',');
            if (string.IsNullOrEmpty(commandPieces[0]) || commandPieces.Length < 2)
            {
                return;
            }
            switch (commandPieces[0])
            {
                case "data":
                    _writer.WriteData(commandPieces[1]);
                    break;
                case "close":
                    _accountManager.AddMiner(commandPieces[1]);
                    _accountManager.CurrentMiner = commandPieces[1];
                    _writer.CloseBlock();
                    _accountManager.PrintBalances();
                    break;
                case "transfer":
                    if (commandPieces.Length < 4) break;
                    var source = commandPieces[1];
                    var destination = commandPieces[2];
                    var amount = commandPieces[3];
                    var success = _accountManager.HandleTransfer(source, destination, amount);
                    if (success)
                    {
                        _writer.WriteTransfer(source, destination, amount);
                    }
                    break;
            }
        }
    }
}