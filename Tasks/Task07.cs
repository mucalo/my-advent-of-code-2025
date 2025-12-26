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
                    nextBeams = nextBeams.Distinct().ToList();
                    distinctLines.Add(nextBeams);
                    wasLineAltered = false;
                }
            }

            // Now we have all Beams and all Beams in previous step.
            // We go backwards
            long[][] count = new long[distinctLines.Count][];
            for (int i = distinctLines.Count - 1; i >= 0; i--)
            {
                count[i] = new long[lines[0].Length];
                
                // If it is the last line, just take into account the last line!
                if (i == distinctLines.Count - 1 )
                {
                    foreach (var item in distinctLines[i])
                    {
                        count[i][item] = 1;
                    }
                }
                // Result for this point in this line is the sum of the ones to the left and ones to the right
                else
                {
                    for (int j = 0; j < lines[0].Length; j++)
                    {
                        if (distinctLines[i].Contains(j))
                        {
                            // We are in a column in the top row that contains a ray.
                            // If the row directly below contains a ray then just copy it.
                            if (distinctLines[i + 1].Contains(j))
                            {
                                count[i][j] = count[i + 1][j];
                            }
                            // Otherwise if the row directly below is a break, then sum left and right
                            else
                            {
                                if (j > 0) count[i][j] += count[i + 1][j-1];
                                if (j < lines[0].Length - 1) count[i][j] += count[i + 1][j + 1];
                            }
                        }
                    }
                }

            }

            return count[0].Sum();
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
