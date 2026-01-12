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

        private class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
            public override string ToString()
            {
                return $"({X},{Y})";
            }
        }

        public static long Part2()
        {
            var lines = File.ReadAllLines("../../../Inputs/09.1.txt");
            long max = 0;

            List<Point> points = new List<Point>();
            foreach (var line in lines)
            {
                var parts = line.Split(",");
                points.Add(new Point(int.Parse(parts[0]), int.Parse(parts[1])));
            }

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = i + 1; j < lines.Length; j++)
                {
                    var point1 = points[i];
                    var point2 = points[j];



                    // If there are points within, it doesn't count
                    if (PointsWithin(point1, point2, points)) continue;
                    // If there are lines crossing the inner section, it doesn't count!
                    if (LinesCrossingMiddle(point1, point2, points)) continue;

                    long area = (Math.Abs(point1.X - point2.X) + 1) * (Math.Abs(point1.Y - point2.Y) + 1);        // TODO is +1 inside or outside abs?
                    // Console.WriteLine($"Considering ({point1.X},{point1.Y}) - ({point2.X},{point2.Y}) with A = {area}");
                    if (area > max)
                    {
                        max = area;
                    }
                }
            }
            return max;
        }

        private static bool LinesCrossingMiddle(Point point1, Point point2, List<Point> points)
        {
            // Clone points, remove point1 and point2
            var clonedPoints = new List<Point>();
            clonedPoints.AddRange(points);
            clonedPoints.Add(points[0]);
            //clonedPoints.Remove(point1);
            //clonedPoints.Remove(point2);

            var leftBorder = point1.X < point2.X ? point1.X : point2.X;
            var rightBorder = point1.X > point2.X ? point1.X : point2.X;
            var topBorder = point1.Y < point2.Y ? point1.Y : point2.Y;
            var bottomBorder = point1.Y > point2.Y ? point1.Y : point2.Y;

            for (int i = 0; i < clonedPoints.Count - 1; i++)
            {
                var p1 = clonedPoints[i];
                var p2 = clonedPoints[i + 1];

                if (p1.X == p2.X && p1.X > leftBorder && p1.X < rightBorder)
                {
                    // Vertical line!
                    if ((p1.Y <= topBorder && p2.Y >= bottomBorder) || (p1.Y >= bottomBorder && p2.Y <= topBorder))
                    {
                        return true;
                    }
                }

                if (p1.Y == p2.Y && p1.Y < bottomBorder && p1.Y > topBorder)
                {
                    // Horizontal line
                    if ((p1.X <= leftBorder && p2.X >= rightBorder)
                        || (p1.X >= rightBorder && p2.X <= leftBorder))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool PointsWithin(Point point1, Point point2, List<Point> points)
        {
            var leftBorder = point1.X < point2.X ? point1.X : point2.X;
            var rightBorder = point1.X > point2.X ? point1.X : point2.X;
            var topBorder = point1.Y < point2.Y ? point1.Y : point2.Y;
            var bottomBorder = point1.Y > point2.Y ? point1.Y : point2.Y;

            foreach (var p in points)
            {
                if (p.X > leftBorder && p.X < rightBorder && p.Y > topBorder && p.Y < bottomBorder) { return true; }
            }

            return false;
        }
    }
}
