using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2025.Tasks
{
    public static class Task06
    {
        public static long Part1()
        {
            var lines = File.ReadAllLines("../../../Inputs/06.1.txt");
            long s;
            var operators = lines[lines.Length - 1].Split(' ');
            operators = operators.Where(o => o.Length > 0).ToArray();

            long[] rows = new long[operators.Length];

            for (int i = 0; i < lines.Length - 1; i++)
            {
                var parts = lines[i].Split(' ').Where(p => p.Length > 0).ToArray();

                for (int j = 0; j < parts.Length; j++)
                {
                    if (operators[j] == "*")
                    {
                        if (rows[j] == 0) rows[j] = 1;      // If multiplication then the row needs to be initialized by 1, otherwise 0
                        rows[j] *= long.Parse(parts[j]);
                    }
                    else
                    {
                        rows[j] += long.Parse(parts[j]);
                    }
                }
            }

            s = rows.Sum(o => o);

            return s;
        }

        public static long Part2()
        {
            var lines = File.ReadAllLines("../../../Inputs/06.1.txt");
            long s = 0;
            var operators = lines[lines.Length - 1].Split(' ');
            operators = operators.Where(o => o.Length > 0).ToArray();

            long[] rows = new long[operators.Length];

            string[][] parts = new string[lines.Length - 1][];
            int columnSize = 0;

            List<int> possibleColumnBreaks = new(); 

            // Parse parts
            // initialize with line 0:
            for (int i = 0; i < lines[0].Length; i++)
            {
                if (lines[0][i] == ' ') possibleColumnBreaks.Add(i);
            }
            
            // Go for the other lines and remove possible line breaks
            for (int i = 0; i < lines.Length - 1; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] != ' ' && possibleColumnBreaks.Contains(j)) possibleColumnBreaks.Remove(j);
                }
            }
            // Parse parts knowing each column length
            for (int i = 0; i < lines.Length - 1; i++)
            {
                int words = 0;
                int columnIndex = 0;
                string currentWord = string.Empty;
                parts[i] = new string[operators.Length];
                int lastColumnBreak = -1;

                for (int j = 0; j < lines[i].Length; j++)
                {
                    currentWord += lines[i][j];
                    if(currentWord.Length == possibleColumnBreaks[columnIndex] - lastColumnBreak -1)
                    {
                        // Add final space
                        currentWord += " ";
                        j++;

                        // Complete word
                        parts[i][words] = currentWord;
                        currentWord = string.Empty;
                        lastColumnBreak = possibleColumnBreaks[columnIndex];

                        words++;
                        columnIndex++;
                    }

                    if (columnIndex == operators.Length - 1)
                    {
                        // Last word, add everything from J to the end and break I loop
                        currentWord = lines[i].Substring(j + 1) + " ";
                        parts[i][words] = currentWord;
                        break;
                    }
                }
            }

            //for (int i = 0; i < parts.Length; i++)
            //{
            //    for (int j = 0; j < parts[i].Length; j++)
            //        Console.Write( parts[i][j].Length.ToString().PadLeft(4));
            //    Console.WriteLine();
            //    Console.WriteLine();
            //    Console.WriteLine();
            //}


            // After I have parts with same length:
            for (int col = 0; col < operators.Length; col++)
            {
                long[] numbersInColumn = new long[20];  // There won't be a number larger than 20 digits in the input file

                for (int i = 0; i < parts[0][col].Length - 1; i++)
                {
                    for (int row = 0; row < parts.Length; row++)
                    {
                        var numString = parts[row][col];
                        if (!string.IsNullOrWhiteSpace(numString) && numString[i] != ' ')
                        {
                            // Digit exists
                            var digit = (numString[i] - '0');
                            numbersInColumn[i] = 10 * numbersInColumn[i] + digit;
                        }
                    }
                }

                // Now the numbers in column contains all the numbers for the given column. 
                // Based on the operator we need to multiply or sum them.
                long columnResult = 0;
                if (operators[col] == "*")
                {
                    columnResult = 1;
                    foreach (long num in numbersInColumn) if (num != 0) columnResult *= num;
                }
                else
                {
                    columnResult = numbersInColumn.Sum(p => p);
                }
                s += columnResult;
                // Console.WriteLine($"{string.Join(", ", numbersInColumn.Where(n => n != 0).ToArray())} => {columnResult}");
            }
            return s;
        }
    }
}
