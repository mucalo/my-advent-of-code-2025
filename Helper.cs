using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2025
{
    public static class Helper
    {
        public static void PrintCharMatrix(char[,] matrix, int maxX, int maxY)
        {
            for (int i = 0; i < maxY; i++)
            {
                for (int j = 0; j < maxX; j++)
                    Console.Write(matrix[i, j]);
                Console.WriteLine();
            }
        }
    }
}
