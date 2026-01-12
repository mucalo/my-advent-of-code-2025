using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2025.Tasks
{
    public static class Task12
    {
        const int BRICKS = 6;
        public static long Part1()
        {
            long s = 0;
            var lines = File.ReadAllLines("../../../Inputs/12.1.txt");
            char[][] bricks;
            int[] brickAreas;
            List<int[]> areas;
            List<int[]> amounts = new();

            ParseFile(out bricks, out brickAreas, out areas, out amounts, lines);

            List<int> indexesToRemoveByArea = new List<int>();
            for (int i = 0; i < areas.Count; i++)
            {
                // Total Area
                var area = areas[i][0] * areas[i][1];

                // Total Area of Presents
                var areaOfPresents = 0;
                for (int j = 0; j < amounts[i].Length; j++)
                {
                    areaOfPresents += amounts[i][j] * brickAreas[j];
                }

                if (areaOfPresents > area)
                {
                    indexesToRemoveByArea.Add(i);
                }
            }


            return areas.Count - indexesToRemoveByArea.Count;
        }

        private static void ParseFile(out char[][] bricks, out int[] brickAreas, out List<int[]> areas, out List<int[]> amounts, string[]? lines)
        {
            int bricksParsed = 0;
            int lineIndex = 0;
            bricks = new char[BRICKS][];
            brickAreas = new int[BRICKS];
            areas = new List<int[]>();
            amounts = new List<int[]>();

            while (bricksParsed < BRICKS)
            {
                lineIndex++;    // Skip the line with the index
                bricks[bricksParsed] = new char[9];
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                    {
                        bricks[bricksParsed][i * 3 + j] = lines[lineIndex + i][j];
                        if (lines[lineIndex + i][j] == '#') brickAreas[bricksParsed]++;
                    }
                lineIndex += 4; // Skip the brick and the line below
                bricksParsed++;
            }

            while (lineIndex < lines.Length)
            {
                var line = lines[lineIndex];
                var parts = line.Split(": ");

                // areas
                var dimensions = parts[0].Split('x');
                var dim = new int[2];
                dim[0] = int.Parse(dimensions[0]);
                dim[1] = int.Parse(dimensions[1]);
                areas.Add(dim);

                // amounts
                var amts = parts[1].Trim().Split(' ');
                int[] amountInts = new int[amts.Length];
                for(int i = 0;i < amts.Length;i++)
                {
                    amountInts[i] = int.Parse(amts[i]);
                }
                amounts.Add(amountInts);

                lineIndex++;
            }
        }
    }
}
