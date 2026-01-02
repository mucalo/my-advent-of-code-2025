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
                ParseLine1(line, out msg, out buttons, out joltage);
                int minMoves = SolveKnapsackFor1(msg, buttons);
                s += minMoves;
            }

            return s;
        }

        public static long Part2()
        {
            var lines = File.ReadAllLines("../../../Inputs/10.1.txt");
            string msg;
            List<int[]> buttons = new List<int[]>();
            int[] joltage;
            int i = 0;
            long s = 0;
            int counter = 0;

            foreach (var line in lines)
            {
                ParseLine2(line, out msg, out buttons, out joltage);
                int minMoves = SolveKnapsackFor2(buttons, joltage);
                s += minMoves;
                Console.WriteLine("Completed line: " + counter++);
            }

            return s;
        }

        /// <summary>
        /// Parsing input file for part 2 - different output types
        /// </summary>
        /// <param name="line"></param>
        /// <param name="msg"></param>
        /// <param name="buttons"></param>
        /// <param name="joltage"></param>
        /// <exception cref="NotImplementedException"></exception>
        private static void ParseLine2(string line, out string msg, out List<int[]> buttons, out int[] joltage)
        {
            // Parse initial msg just to get tht length of the array, c/p from 1st problem
            joltage = null;
            buttons = new List<int[]>();
            msg = string.Empty;

            int length = line.IndexOf(']') - 1;
            for (int i = 1; i < length + 1; i++)
            {
                if (line[i] == '.') msg += "0"; else msg += "1";
            }

            line = line.Substring(length + 2).Trim();

            int arrLength = msg.Length;
            joltage = new int[arrLength];

            while (line.IndexOf("(") != -1)
            {
                line = line.Substring(line.IndexOf("(") + 1);
                length = line.IndexOf(")");
                var parsable = line.Substring(0, length);
                var parts = parsable.Split(',');

                int[] button = new int[arrLength];
                int partsIndex = 0;
                for (int i = 0; i < arrLength; i++)
                {
                    int place = partsIndex < parts.Length ? int.Parse(parts[partsIndex]) : -1;
                    if (i == place) { button[i] = 1; partsIndex++; }
                }
                buttons.Add(button);

                line = line.Substring(length);
            }

            line = line.Substring(line.IndexOf('{') + 1); // Start after {{
            line = line.Substring(0, line.Length - 1);  // Remove }
            string[] joltageParts = line.Split(',');
            for (int i = 0; i < joltageParts.Length; i++)
            {
                joltage[i] = int.Parse(joltageParts[i]);
            }

        }

        private static int SolveKnapsackFor1(string msg, List<string> buttons)
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

        private static int SolveKnapsackFor2(List<int[]> buttons, int[] joltage)
        {
            Dictionary<string, int> solutions = new Dictionary<string, int>();
            solutions.Add(string.Join('|', joltage), 0);

            Dictionary<string, int[]> keyIndex = new Dictionary<string, int[]>();
            keyIndex.Add(string.Join('|', joltage), joltage);

            var finalKey = "0";
            for (int i = 0; i < joltage.Length - 1; i++) finalKey += "|0";

            List<string> keysToRecheck = new List<string>() { solutions.First().Key };
            int bestSolution = int.MaxValue;

            while (true)
            {
                Console.WriteLine($"Keys to Recheck Count: {keysToRecheck.Count}");
                if (keysToRecheck.Count == 0) break;

                var listOfKeysToCheck = new List<string>();
                listOfKeysToCheck.AddRange(keysToRecheck);
                keysToRecheck.Clear();

                foreach (var key in listOfKeysToCheck)
                {
                    foreach (var button in buttons)
                    {
                        var altJoltage = UndoButton(button, keyIndex[key]);
                        var altJoltageKey = string.Join("|", altJoltage);
                        int currentCount = solutions[key] + 1;

                        // if any new result is -1 then we stop for this option and don't add key to recheck.
                        if (altJoltage.Any(x => x < 0)) continue;

                        if (solutions.ContainsKey(altJoltageKey) && currentCount < solutions[altJoltageKey])
                        {
                            solutions[altJoltageKey] = currentCount;
                            keysToRecheck.Add(altJoltageKey);
                            keyIndex.Add(altJoltageKey, altJoltage);
                        }
                        else if (!solutions.ContainsKey(altJoltageKey))
                        {
                            solutions.Add(altJoltageKey, currentCount);
                            keysToRecheck.Add(altJoltageKey);
                            keyIndex.Add(altJoltageKey, altJoltage);
                        }
                    }
                }
            }

            if (solutions.ContainsKey(finalKey))
            {
                return solutions[finalKey];
            }
            else
            {
                throw new Exception("No solution found!");
            }
        }

        /// <summary>
        /// Method substracts button from joltage
        /// </summary>
        /// <param name="button"></param>
        /// <param name="joltage"></param>
        /// <returns></returns>
        private static int[] UndoButton(int[] button, int[] joltage)
        {
            int[] result = new int[button.Length];
            for (int i = 0; i < button.Length; i++)
            {
                result[i] = joltage[i] - button[i];
            }
            return result;
        }


        public static string Toggle(string msg, string button)
        {
            string toReturn = string.Empty;
            for (int i = 0; i < msg.Length; i++)
            {
                if (button[i] == '0')   // No toggling
                {
                    toReturn += msg[i];
                }
                else if (msg[i] == '1')
                {
                    toReturn += '0';
                }
                else
                {
                    toReturn += '1';
                }
            }
            return toReturn;
        }

        private static void ParseLine1(string line, out string msg, out List<string> buttons, out object joltage)
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
    }
}
