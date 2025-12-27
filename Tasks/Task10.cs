using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2025.Tasks
{
    public static class Task10
    {
        public static long Part1()
        {
            var lines = File.ReadAllLines("../../../Inputs/10.1.txt");
            string msg;
            List<string> buttons = new List<string>();
            object joltage;
            int i = 0;
            long s = 0;

            foreach (var line in lines)
            {
                ParseLine(line, out msg, out buttons, out joltage);
                int minMoves = SolveKnapsack(msg, buttons);
                s += minMoves;    
            }

            return s;
        }

        private static int SolveKnapsack(string msg, List<string> buttons)
        {
            var endMsg = string.Empty.PadLeft(msg.Length, '0');
            Dictionary<string, int> solutions = new Dictionary<string, int>();
            solutions.Add(msg, 0);
            List<string> keysToRecheck = new List<string>() { msg };
            int bestSolution = int.MaxValue;

            while (true)
            {
                var listOfKeysToCheck = new List<string>();
                listOfKeysToCheck.AddRange(keysToRecheck);
                keysToRecheck.Clear();

                foreach (var key in listOfKeysToCheck)
                {
                    foreach (var button in buttons)
                    {
                        var altMsg = Toggle(key, button);
                        int currentCount = solutions[key] + 1;
                        if (solutions.ContainsKey(altMsg) && currentCount < solutions[altMsg])
                        {
                            solutions[altMsg] = currentCount;
                            keysToRecheck.Add(altMsg);
                        }
                        else if (!solutions.ContainsKey(altMsg))
                        {
                            solutions.Add(altMsg, currentCount);
                            keysToRecheck.Add(altMsg);
                        }
                    }
                }

                // Stopping conditions
                if (solutions.ContainsKey(endMsg))
                {
                    if (solutions[endMsg] < bestSolution)
                    {
                        bestSolution = solutions[endMsg];
                        return bestSolution;
                    }
                }

                //var minValue = solutions.Values.Min();
                //if (minValue >= bestSolution)
                //{
                //    return bestSolution;
                //}
            }
        }

        public static string Toggle(string msg, string button)
        {
            string toReturn = string.Empty;
            for (int i = 0; i < msg.Length; i++)
            {
                if (button[i] == '0')   // No toggling
                {
                    toReturn += msg[i];
                } else if (msg[i] == '1')
                {
                    toReturn += '0';
                } else
                {
                    toReturn += '1';
                }
            }
            return toReturn;
        }

        private static void ParseLine(string line, out string msg, out List<string> buttons, out object joltage)
        {
            joltage = null;
            buttons = new List<string>();
            msg = string.Empty;

            int length = line.IndexOf(']') - 1;
            for (int i = 1; i < length + 1; i++)
            {
                if (line[i] == '.') msg += "0"; else msg += "1";
            }

            line = line.Substring(length + 2).Trim();

            while (line.IndexOf("(") != -1)
            {
                line = line.Substring(line.IndexOf("(") + 1);
                length = line.IndexOf(")");
                var parsable = line.Substring(0, length);
                var parts = parsable.Split(',');

                string button = string.Empty;
                int partsIndex = 0;
                for (int i = 0; i < msg.Length; i++)
                {
                    int place = partsIndex < parts.Length ? int.Parse(parts[partsIndex]) : -1;
                    if (i != place) button += "0"; else { button += "1"; partsIndex++; }
                }
                buttons.Add(button);

                line = line.Substring(length);
            }
        }

        private static bool BitArrayEquals(BitArray a, BitArray b)
        {
            if (a.Length != b.Length) return false;

            int[] ia = new int[(a.Length + 31) / 32];
            int[] ib = new int[(b.Length + 31) / 32];

            a.CopyTo(ia, 0);
            b.CopyTo(ib, 0);

            for (int i = 0; i < ia.Length; i++)
                if (ia[i] != ib[i])
                    return false;

            return true;
        }
    }
}
