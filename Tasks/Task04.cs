using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2025.Tasks
{
    public static class Task04
    {
        public const char ROLL = '@';
        public const char EMPTY = '.';
        public static long Part1()
        {
            long s = 0;

            var lines = File.ReadAllLines("../../../Inputs/04.1.txt");
            char[,] matrix = new char[lines.Length, lines[0].Length];

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    matrix[i, j] = lines[i][j];
                }
            }

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (IsAccessible(matrix, i, j, lines[i].Length, lines.Length)) s++;

                }
            }
            return s;
        }

        private static bool IsAccessible(char[,] matrix, int i, int j, int maxX, int maxY)
        {
            if (matrix[i,j] != ROLL) return false;

            int touching = 0;

            if (i > 0 && j > 0 && matrix[i - 1, j - 1] == ROLL) touching++;
            if (i > 0 && matrix[i - 1, j] == ROLL) touching++;
            if (i > 0 && j < maxX - 1 && matrix[i - 1, j + 1] == ROLL) touching++;
            
            if (j > 0 && matrix[i, j - 1] == ROLL) touching++;
            if (j < maxX - 1 && matrix[i, j + 1] == ROLL) touching++;
            
            if (i < maxY - 1 && j > 0 && matrix[i + 1, j - 1] == ROLL) touching++;
            if (i < maxY - 1 && matrix[i + 1, j] == ROLL) touching++;
            if (i < maxY - 1 && j < maxX - 1 && matrix[i + 1, j + 1] == ROLL) touching++;

            return touching < 4;
        }

        public static long Part2()
        {
            long s = 0;

            var lines = File.ReadAllLines("../../../Inputs/04.1.txt");
            char[,] matrix = new char[lines.Length, lines[0].Length];

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    matrix[i, j] = lines[i][j];
                }
            }

            

            while (true)
            {
                List<(int Row, int Col)> toRemove = new();
                for (int i = 0; i < lines.Length; i++)
                {
                    for (int j = 0; j < lines[i].Length; j++)
                    {
                        if (IsAccessible(matrix, i, j, lines[i].Length, lines.Length)) toRemove.Add((i,j));

                    }
                }

                if (toRemove.Count == 0) break;
                else
                {
                    s += toRemove.Count;
                    foreach (var item in toRemove) matrix[item.Row, item.Col] = EMPTY;
                }
            }
            
            
            return s;
        }
    }
}
