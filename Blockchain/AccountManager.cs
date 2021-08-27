using System;
using System.Collections.Generic;

namespace Blockchain
{
    public class AccountManager
    {
        private readonly Dictionary<string, int> _minerFunds;
        private const int SolveBlockPayment = 100;
        public string CurrentMiner { get; set; } 

        public AccountManager()
        {
            _minerFunds = new Dictionary<string, int>();
        }

        public void AddMiner(string name)
        {
            if (!_minerFunds.ContainsKey(name))
            {
                _minerFunds.Add(name, 0);
                CurrentMiner = name;
            }
        }

        public void CurrentMinerSolvedBlock()
        {
            AddFundsToMiner(CurrentMiner, SolveBlockPayment);
        }

        public void PrintBalances()
        {
            Console.WriteLine("Account balances:");
            foreach (var keyValuePair in _minerFunds)
            {
                Console.WriteLine($"{keyValuePair.Key}: {keyValuePair.Value}");
            }
        }

        private void AddFundsToMiner(string miner, int amount)
        {
            if (_minerFunds.ContainsKey(miner))
            {
                _minerFunds[miner] += amount;
            }
        }

        private bool SubtractFundsFromMiner(string miner, int amount)
        {
            if (!_minerFunds.ContainsKey(miner) || _minerFunds[miner] < amount) return false;
            
            _minerFunds[miner] -= amount;
            return true;
        }

        public bool HandleTransfer(string source, string destination, string amount)
        {
            if (SubtractFundsFromMiner(source, int.Parse(amount)))
            {
                AddFundsToMiner(destination, int.Parse(amount));
                return true;
            }

            return false;
        }
    }
}