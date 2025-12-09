namespace AdventOfCode2025.Tasks
{
    public static class Task09
    {
        public static long Part1()
        {
            var lines = File.ReadAllLines("../../../Inputs/09.1.txt");
            long max = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = i+1; j < lines.Length; j++)
                {
                    var point1 = lines[i].Split(',');
                    var point2 = lines[j].Split(',');

                    long p1x = long.Parse(point1[0]);
                    long p1y = long.Parse(point1[1]);
                    long p2x = long.Parse(point2[0]);
                    long p2y = long.Parse(point2[1]);

                    long area = Math.Abs((p1x - p2x + 1) * (p1y - p2y + 1));
                    if (area > max)
                    {
                        max = area;
                    }
                }
            }
            return max;
        }
    }
}
