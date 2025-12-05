using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2025.Tasks
{
    public static class Task05
    {

        private static (long, long)? MergeIntervalsIfOverlap((long, long) int1, (long, long) int2)
        {
            var max1 = int1.Item1 > int2.Item1 ? int1.Item1 : int2.Item1;
            var min2 = int1.Item2 < int2.Item2 ? int1.Item2 : int2.Item2;

            if (max1 <= min2)
            {
                // They are overlapping
                var min1 = int1.Item1 < int2.Item1 ? int1.Item1 : int2.Item1;
                var max2 = int1.Item2 > int2.Item2 ? int1.Item2 : int2.Item2;
                return (min1, max2);
            }
            else
            {
                // No overlap return null;
                return null;
            }
        }

        public static long Part1()
        {
            var lines = File.ReadAllLines("../../../Inputs/05.1.txt");
            string line = lines[0];
            int i = 0;
            int fresh = 0;

            // Go through first section, create the intervals
            List<(long Min, long Max)> intervals = new();
            do
            {
                var parts = line.Split('-');
                (long, long) interval = (long.Parse(parts[0]), long.Parse(parts[1]));
                intervals.Add(interval);
                int j = 0;
                bool isMerged = false;
                for (j = intervals.Count - 2; j >= 0; j--)
                {
                    var newInterval = MergeIntervalsIfOverlap(intervals[j], interval);
                    if (newInterval != null)
                    {
                        interval = newInterval.Value;
                        intervals[intervals.Count - 1] = newInterval.Value;
                        intervals.RemoveAt(j);
                    }
                }

                i++;
                line = lines[i];
            } while (line != string.Empty);

            int start = i + 1;
            for (i = start; i < lines.Length; i++)
            {
                long number = long.Parse(lines[i]);
                foreach (var item in intervals)
                {
                    if (item.Min <= number && item.Max >= number)
                    {
                        fresh++;
                        break;
                    }
                }
            }

            return fresh;
        }

        public static long Part2()
        {
            var lines = File.ReadAllLines("../../../Inputs/05.1.txt");
            string line = lines[0];
            int i = 0;
            long sum = 0;

            // Go through first section, create the intervals
            List<(long Min, long Max)> intervals = new();
            do
            {
                var parts = line.Split('-');
                (long, long) interval = (long.Parse(parts[0]), long.Parse(parts[1]));
                intervals.Add(interval);
                int j = 0;
                bool isMerged = false;
                for (j = intervals.Count - 2; j >= 0; j--)
                {
                    var newInterval = MergeIntervalsIfOverlap(intervals[j], interval);
                    if (newInterval != null)
                    {
                        interval = newInterval.Value;
                        intervals[intervals.Count - 1] = newInterval.Value;
                        intervals.RemoveAt(j);
                    }
                }

                i++;
                line = lines[i];
            } while (line != string.Empty);

            foreach (var interval in intervals)
            {
                sum += interval.Max - interval.Min + 1;
            }


            return sum;
        }
    }
}
