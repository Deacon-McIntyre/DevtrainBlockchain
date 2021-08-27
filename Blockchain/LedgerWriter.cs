using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Blockchain
{
    public class LedgerWriter
    {
        private AccountManager _accountManager;
        public LedgerWriter(AccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        private const string LedgerPath = "ledger.txt";

        public void WriteData(string value)
        {
            File.AppendAllText(LedgerPath, $"data,{value}{Environment.NewLine}");
            Console.WriteLine($"wrote data: {value}");
        }

        public void WriteTransfer(string source, string destination, string amount)
        {
            File.AppendAllText(LedgerPath, $"transfer,{source},{destination},{amount}{Environment.NewLine}");
            Console.WriteLine($"wrote transfer: {source},{destination},{amount}");
        }
        
        public void WriteBlock()
        {
            var dataUpToPreviousBlock = GetUpToPreviousBlock();
            var hash = ComputeHash(dataUpToPreviousBlock);
            File.AppendAllText(LedgerPath, $"block,{hash}{Environment.NewLine}");
            Console.WriteLine($"Wrote hash: {hash}");
        }
        
        public void CloseBlock()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            bool saltFound = false;
            var stringToEvaluate = "";
            var salt = 0;
            while (!saltFound)
            {
                salt++;
                stringToEvaluate = $"miner,{_accountManager.CurrentMiner}{Environment.NewLine}salt,{salt}{Environment.NewLine}";
                if (SaltIsValid(stringToEvaluate))
                {
                    saltFound = true;
                }
            }
            Console.WriteLine($"Took {stopwatch.ElapsedMilliseconds} milliseconds to find valid salt");
            
            File.AppendAllText(LedgerPath, stringToEvaluate);
            WriteBlock();
            _accountManager.CurrentMinerSolvedBlock();
            Console.WriteLine($"Wrote salt: {salt}");
        }

        private bool SaltIsValid(string stringToEvaluate)
        {
            var dataUpToPreviousBlock = GetUpToPreviousBlock();
            var hash = ComputeHash(dataUpToPreviousBlock + stringToEvaluate);
            return hash.StartsWith("000");
        }

        private string ComputeHash(string data)
        {
            using SHA256 hasher = SHA256.Create();
            var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        private string GetUpToPreviousBlock()
        {
            var linesToKeep = new List<string>();
            var lines = File.ReadAllLines(LedgerPath).Reverse();
            foreach (var line in lines)
            {
                if (line.StartsWith("block"))
                {
                    linesToKeep.Add(line);
                    break;
                }
                linesToKeep.Add(line);
            }

            linesToKeep.Reverse();
            return String.Join(Environment.NewLine, linesToKeep) + Environment.NewLine;
        }
    }
}