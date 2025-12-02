
namespace AdventOfCode2025.Tasks
{
    public static class Task02
    {
        private static bool IsValid(string input)
        {
            if (input.Length % 2 != 0) return true;
            string part1 = input.Substring(0, input.Length / 2);
            string part2 = input.Substring(input.Length / 2);
            if (part1 == part2) return false; else return true;
        }

        private static bool IsRecursivelyInvalid(string input, string part)
        {
            if (input == "") return true;
            if (input.StartsWith(part))
            {
                return IsRecursivelyInvalid(input.Substring(part.Length), part);
            }
            else
            {
                return false;
            }
        }

        public static long Part1()
        {
            long s = 0;
            var lines = File.ReadAllText("../../../Inputs/02.1.txt").Split(",");
            foreach (var line in lines)
            {
                var parts = line.Split('-');
                long start = long.Parse(parts[0]);
                long end = long.Parse(parts[1]);
                long i = start;
                while (i <= end)
                {
                    if (!IsValid(i.ToString())) s += i;

                    i++;
                }

            }
            return s;
        }

        public static long Part2()
        {
            long s = 0;
            var lines = File.ReadAllText("../../../Inputs/02.1.txt").Split(",");
            foreach (var line in lines)
            {
                var parts = line.Split('-');
                long start = long.Parse(parts[0]);
                long end = long.Parse(parts[1]);
                long i = start;
                while (i <= end)
                {
                    for (int len = 1; len <= i.ToString().Length / 2; len++)
                    {
                        string part = i.ToString().Substring(0, len);
                        if (IsRecursivelyInvalid(i.ToString(), part))
                        {
                            s += i;
                            break;
                        }
                    }
                    i++;
                }

            }
            return s;
        }
    }
}
