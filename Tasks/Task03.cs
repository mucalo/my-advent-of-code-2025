using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2025.Tasks
{
    public static class Task03
    {
        public static long Part1()
        {
            long s = 0;
            var lines = File.ReadAllLines("../../../Inputs/03.1.txt");
            foreach (var line in lines)
            {
                int i = 0;
                int maxStart = 0;
                int maxEnd = 0;
                int startIndex = 0;
                SortedDictionary<int, int> endLocalMaximums = new SortedDictionary<int, int>();
                var j = line.Length - 1;
                while (i < line.Length - 1)
                {
                    int endInt = line[j] - '0';
                    int startInt = line[i] - '0';
                    if (startInt > maxStart)
                    {
                        maxStart = startInt;
                        startIndex = i;
                    }
                    if (endInt > maxEnd)
                    {
                        maxEnd = endInt;
                        endLocalMaximums.Add(j, endInt);
                    }

                    // if maxStart or maxEnd are at 9, then that side needs to search no more!
                    if (maxStart < 9) i++;
                    if (maxEnd < 9) j--;

                    // if (i > j) break;
                    if (maxStart == 9 && maxEnd == 9) break;
                }

                // Now we need to assemble the number.
                var number = 10 * maxStart;
                foreach (var item in endLocalMaximums)
                {
                    if (item.Key > startIndex)
                    {
                        number += item.Value;
                        break;
                    }
                }

                s += number;
            }

            return s;
        }



        public static long Part2()
        {
            long s = 0;
            var lines = File.ReadAllLines("../../../Inputs/03.1.txt");
            foreach (var line in lines)
            {
                int i = 0;
                int maxStart = 0;
                int minEnd = 10;
                int maxEnd = 0;
                int startIndex = 0;

                List<(int, int)> digits = new List<(int, int)>();

                while (i < line.Length - 11)
                {
                    var num = line[i] - '0';
                    if (num > maxStart)
                    {
                        maxStart = num;
                        startIndex = i;
                    }
                    i++;
                }

                string numString = maxStart.ToString();

                int biggestDigitIndex = startIndex;
                for (int j = 11; j > 0; j--)
                {
                    // find biggest digit in the position where it can be.
                    int biggestDigit = 0;
                    int index = 0;
                    int startForThisLoop = biggestDigitIndex;
                    while (true)
                    {
                        var num = line[startForThisLoop + index + 1] - '0';
                        if (num > biggestDigit)
                        {
                            biggestDigit = num;
                            biggestDigitIndex = startForThisLoop + index + 1;
                        }

                        index++;
                        if (startForThisLoop + index >= line.Length - j) break;

                        
                    }

                    numString += biggestDigit.ToString();
                }

                long number = long.Parse(numString);
                s += number;
            }

            return s;

        }
    }
}
