using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2025.Tasks
{
    public static class Task08
    {
        private class Point
        {
            public int Id { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }

            public Point(string s)
            {
                var parts = s.Trim().Split(',');
                this.X = int.Parse(parts[0]);
                this.Y = int.Parse(parts[1]);
                this.Z = int.Parse(parts[2]);
            }
        }

        private static double Distance(Point p1, Point p2)
        {
            var distance = Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) + Math.Pow(p1.Z - p2.Z, 2));
            return distance;
        }

        public static long Part1()
        {
            var lines = File.ReadAllLines("../../../Inputs/08.1.txt");
            long s = 0;

            Dictionary<(int, int), double> distances = new Dictionary<(int, int), double>();
            List<List<int>> junctions = new List<List<int>>();
            for (int i = 0; i < lines.Length - 1; i++)
            {
                var p1 = new Point(lines[i]);
                for (int j = i + 1; j < lines.Length; j++)
                {
                    // Calculate distance between Point[i] and Point[j]
                    var p2 = new Point(lines[j]);
                    var distance = Distance(p1, p2);
                    distances.Add((i, j), distance);
                }
            }
            distances = distances.OrderBy(s => s.Value).ToDictionary();

            // Go through the dictionary item by item.
            // If any junction contains any of the indexes in key, add that line to that junction
            // If not, create a new one!
            int index = 0;
            foreach (var item in distances)
            {
                var junction1 = junctions.FirstOrDefault(j => j.Contains(item.Key.Item1));
                var junction2 = junctions.FirstOrDefault(j => j.Contains(item.Key.Item2));

                // if both are null => new Junction
                if (junction1 == null && junction2 == null)
                {
                    // Not added to existing junction, create a new junction
                    var junction = new List<int>() { item.Key.Item1, item.Key.Item2 };
                    junctions.Add(junction);
                    index++;
                    
                }
                // if one is null, add the other item to the junction
                else if (junction1 != null && junction2 == null)
                {
                    junction1.Add(item.Key.Item2);
                    index++;
                    
                }
                else if (junction1 == null && junction2 != null)
                {
                    junction2.Add(item.Key.Item1);
                    index++;
                }
                // if both are not null and same - nothing happens
                else if (junction1 != null && junction1 == junction2)
                {
                    // TODO: TEST IF WE COUNT THE CONNECTION
                    index++;
                }
                // if both are not null and different - join junctions!!!!!!
                else if (junction1 != null && junction2 != null && junction1 != junction2)
                {
                    junction1.AddRange(junction2);
                    junctions.Remove(junction2);
                    index++;
                }


                if (index == 1000) break;
            }

            var top3 = junctions.Select(j => j.Count).OrderByDescending(j => j).Take(3).ToList();


            return top3[0] * top3[1] * top3[2];
        }


        public static long Part2()
        {
            var lines = File.ReadAllLines("../../../Inputs/08.1.txt");
            long s = 0;

            Dictionary<(int, int), double> distances = new Dictionary<(int, int), double>();
            List<List<int>> junctions = new List<List<int>>();
            for (int i = 0; i < lines.Length - 1; i++)
            {
                var p1 = new Point(lines[i]);
                for (int j = i + 1; j < lines.Length; j++)
                {
                    // Calculate distance between Point[i] and Point[j]
                    var p2 = new Point(lines[j]);
                    var distance = Distance(p1, p2);
                    distances.Add((i, j), distance);
                }
            }
            distances = distances.OrderBy(s => s.Value).ToDictionary();

            // Go through the dictionary item by item.
            // If any junction contains any of the indexes in key, add that line to that junction
            // If not, create a new one!
            int index = 0;
            foreach (var item in distances)
            {
                var junction1 = junctions.FirstOrDefault(j => j.Contains(item.Key.Item1));
                var junction2 = junctions.FirstOrDefault(j => j.Contains(item.Key.Item2));

                // if both are null => new Junction
                if (junction1 == null && junction2 == null)
                {
                    // Not added to existing junction, create a new junction
                    var junction = new List<int>() { item.Key.Item1, item.Key.Item2 };
                    junctions.Add(junction);
                    index++;

                }
                // if one is null, add the other item to the junction
                else if (junction1 != null && junction2 == null)
                {
                    junction1.Add(item.Key.Item2);
                    index++;

                }
                else if (junction1 == null && junction2 != null)
                {
                    junction2.Add(item.Key.Item1);
                    index++;
                }
                // if both are not null and same - nothing happens
                else if (junction1 != null && junction1 == junction2)
                {
                    // TODO: TEST IF WE COUNT THE CONNECTION
                    index++;
                }
                // if both are not null and different - join junctions!!!!!!
                else if (junction1 != null && junction2 != null && junction1 != junction2)
                {
                    junction1.AddRange(junction2);
                    junctions.Remove(junction2);
                    index++;
                }


                if (junctions != null && junctions.Count == 1 && junctions[0].Count == lines.Length)
                {
                    var p1 = new Point(lines[item.Key.Item1]);
                    var p2 = new Point(lines[item.Key.Item2]);
                    return p1.X * p2.X;
                } 
            }

            throw new Exception("Should never get here!");
        }
    }
}
