using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2025.Tasks
{
    public static class Task07
    {
        public const char START = 'S';
        public const char BREAK = '^';
        public static long Part1()
        {
            var lines = File.ReadAllLines("../../../Inputs/07.1.txt");
            long s = 0;

            int prev = 0;
            List<int> beams = new List<int>();
            beams.Add(lines[0].IndexOf(START));


            for (int i = 1; i < lines.Length; i++)
            {
                List<int> nextBeams = new List<int>();
                List<int> alreadyBrokenOn = new List<int>();

                string currentLine = lines[i];

                foreach (int beam in beams)
                {
                    bool wasBeamAltered = false;
                    if (currentLine[beam] == BREAK)
                    {
                        // If not already broken on this obstacle, add the break
                        if (!alreadyBrokenOn.Contains(beam))
                        {
                            alreadyBrokenOn.Add(beam);
                            s++;
                        }

                        // Try and add left
                        if (beam - 1 >= 0 && !nextBeams.Contains(beam - 1))
                        {
                            nextBeams.Add(beam - 1);
                        }
                        // Try and add right
                        if (beam + 1 <= currentLine.Length - 1 && !nextBeams.Contains(beam + 1))
                        {
                            nextBeams.Add(beam + 1);
                        }

                        // Modify additions
                        wasBeamAltered = true;
                    }

                    if(!wasBeamAltered) nextBeams.Add(beam);
                }
                // Console.WriteLine($"After line {i} count is {s}.");
                beams = nextBeams;
            }

            return s;
        }

        public static long Part2()
        {
            var lines = File.ReadAllLines("../../../Inputs/07.1.txt");
            long s = 0;

            int prev = 0;
            List<int> beams = new List<int>();
            beams.Add(lines[0].IndexOf(START));
            List<List<int>> distinctLines = new List<List<int>>();
            distinctLines.Add(beams);

            for (int i = 1; i < lines.Length; i++)
            {
                List<int> nextBeams = new List<int>();
                List<int> alreadyBrokenOn = new List<int>();

                string currentLine = lines[i];

                bool wasLineAltered = false;
                foreach (int beam in beams)
                {
                    bool wasBeamAltered = false;
                    if (currentLine[beam] == BREAK)
                    {
                        // If not already broken on this obstacle, add the break
                        if (!alreadyBrokenOn.Contains(beam))
                        {
                            alreadyBrokenOn.Add(beam);
                            s++;
                        }

                        // Try and add left
                        if (beam - 1 >= 0 && !nextBeams.Contains(beam - 1))
                        {
                            nextBeams.Add(beam - 1);
                        }
                        // Try and add right
                        if (beam + 1 <= currentLine.Length - 1 && !nextBeams.Contains(beam + 1))
                        {
                            nextBeams.Add(beam + 1);
                        }

                        // Modify additions
                        wasBeamAltered = true;
                        wasLineAltered = true;
                    }

                    if (!wasBeamAltered) nextBeams.Add(beam);
                }
                // Console.WriteLine($"After line {i} count is {s}.");
                beams = nextBeams;

                if (wasLineAltered)
                {
                    distinctLines.Add(nextBeams);
                    wasLineAltered = false;
                }
            }

            // Now we have all Beams and all Beams in previous step.
            // We go backwards
            int[][] count = new int[distinctLines.Count][];
            for (int i = distinctLines.Count - 1; i > 0; i--)
            {
                count[i] = new int[lines[0].Length];
                
                // If it is the last line, just take into account the last line!
                if (i == distinctLines.Count - 1 )
                {
                    foreach (var item in distinctLines[i])
                    {
                        count[i][item] = 1;
                    }
                }
                // Result for this point in this line is the sum of the ones to the left and ones to the right!
                else
                {
                    foreach (var item in distinctLines[i])
                    {
                        var left = item - 1 > 0 ? count[i + 1][item - 1] : 0;
                        var right = item + 1 < lines[0].Length - 1 ? count[i + 1][item + 1] : 0;
                        count[i][item] = left + right;
                    }
                }

                foreach (var item in count[i])
                    Console.Write(item.ToString().PadLeft(3, ' '));
                Console.WriteLine();
            }

            return s;
        }

        private static long CountPossibilities(int beam, int nextLine, string[] lines)
        {
            // If last line and beam hits, return 2
            // If last line and beam doesn't hit, return 1
            if (nextLine == lines.Length - 1)
            {
                if (lines[nextLine][beam] == BREAK) return 2; else return 1;
            }

            // Else if not hitting a break then just return 1 and move to the next line
            else if (lines[nextLine][beam] != BREAK)
            {
                return CountPossibilities(beam, nextLine+1, lines);
            }

            // Else return CountPossibilities from left (if exists) and right (if exists)
            else
            {
                long s = 0;
                if (beam - 1 >= 0)
                {
                    s += CountPossibilities(beam - 1, nextLine + 1, lines);
                }
                if (beam + 1 <= lines[nextLine + 1].Length - 1)
                {
                    s += CountPossibilities(beam + 1, nextLine + 1, lines);
                }
                return s;
            }
        }
    }
}
