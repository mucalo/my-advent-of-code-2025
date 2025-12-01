namespace AdventOfCode2025.Tasks
{
    public static class Task01
    {
        public static int Part1()
        {
            var lines = File.ReadAllLines("../../../Inputs/01.1.txt");
            int s = 0;
            int position = 50;
            foreach (var line in lines)
            {
                var direction = line[0];
                var value = int.Parse(line[1..]);

                if (direction == 'L')
                {
                    position -= value;
                }
                else
                {
                    position += value;
                }

                position %= 100;

                if (position == 0) s++;
            }

            return s;
        }

        public static int Part2()
        {
            var lines = File.ReadAllLines("../../../Inputs/01.1.txt");
            int s = 0;
            int position = 50;
            int oldPosition = 50;
            foreach (var line in lines)
            {
                oldPosition = position;
                var direction = line[0];
                var value = int.Parse(line[1..]);

                if (direction == 'L')
                {
                    position -= value;
                }
                else
                {
                    position += value;
                }

                int numOfTimesPassedZero = Math.Abs(position / 100);
                if ((oldPosition < 0 && position >= 0) || (oldPosition > 0 && position <= 0))
                {
                    numOfTimesPassedZero++;
                }
                s += numOfTimesPassedZero;
                position %= 100;
            }

            return s;
        }
    }
}
