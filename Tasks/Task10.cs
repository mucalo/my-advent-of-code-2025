using AdventOfCode2025.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2025.Tasks
{
    public static class Task10
    {


        public static long Part1()
        {
            var lines = File.ReadAllLines("../../../Inputs/10.1.txt");
            string msg;
            List<string> buttons = new List<string>();
            object joltage;
            int i = 0;
            long s = 0;

            foreach (var line in lines)
            {
                ParseLine1(line, out msg, out buttons, out joltage);
                int minMoves = SolveKnapsackFor1(msg, buttons);
                s += minMoves;
            }

            return s;
        }

        public static long Part2()
        {
            bool isDebug = false;
            var lines = File.ReadAllLines("../../../Inputs/10.1.txt");
            string msg;
            List<int[]> buttons = new List<int[]>();
            int[] joltage;
            long s = 0;
            int counter = 0;
            int lineIndex = 0;

            foreach (var line in lines)
            {
                ParseLine2(line, out msg, out buttons, out joltage);

                List<int> fixedVariables = new();
                List<int> looseVariables = new();
                Rational[] resultVector = null;
                int[] limitationsOfVariables = new int[buttons.Count];
                var minSum = int.MaxValue;

                for (int i = 0; i < limitationsOfVariables.Length; i++) limitationsOfVariables[i] = int.MaxValue;

                for (int i = 0; i < buttons.Count; i++)
                {
                    for (int j = 0; j < buttons[i].Length; j++)
                    {
                        if (buttons[i][j] == 1)
                        {
                            // This could affect the minimum!
                            if (joltage[j] < limitationsOfVariables[i]) limitationsOfVariables[i] = joltage[j];
                        }
                    }
                }

                var matrix = GetRREFMatrix(buttons, joltage, out fixedVariables, out looseVariables, out resultVector);

                if (isDebug)
                {
                    PrintMatrixAndVector(matrix, resultVector);
                    Console.WriteLine();
                }

                // If there are loose variables, then you need to iterate over all their possibilities
                if (looseVariables.Count > 0)
                {
                    // We need to iterate over all loose variables up to their maximum
                    int[] counters = new int[looseVariables.Count];
                    while (counters[counters.Length - 1] <= limitationsOfVariables[looseVariables[looseVariables.Count - 1]])
                    {
                        int[] fixedSolutions = new int[fixedVariables.Count];

                        // Try and calcuate the final values with parameters from counters
                        for (int fixedIndex = 0; fixedIndex < fixedVariables.Count; fixedIndex++)
                        {
                            Rational sum = resultVector[fixedIndex];
                            for (int looseIndex = 0; looseIndex < looseVariables.Count; looseIndex++)
                            {
                                sum = sum - matrix[fixedIndex][looseVariables[looseIndex]] * Rational.FromInt(counters[looseIndex]);
                            }



                            if (sum.ToDecimal() < 0)
                                fixedSolutions[fixedIndex] = 100000;    // HACK
                            else if (sum.ToDecimal() % 1 != 0)
                                fixedSolutions[fixedIndex] = 100000;    // HACK
                            else
                                fixedSolutions[fixedIndex] = (int)sum.ToDecimal();

                        }

                        if (isDebug)
                            PrintSums(fixedSolutions, fixedVariables, looseVariables, counters);

                        var sumForCurrentLooseCombo = fixedSolutions.Sum(x => x) + counters.Sum();
                        if (sumForCurrentLooseCombo < minSum) minSum = sumForCurrentLooseCombo;

                        // Increase the counters
                        int indexOfCounter = 0;
                        while (indexOfCounter < counters.Length)
                        {
                            counters[indexOfCounter]++;
                            if (counters[indexOfCounter] > limitationsOfVariables[looseVariables[indexOfCounter]])
                            {
                                // Overflow happening
                                counters[indexOfCounter] = 0;
                                indexOfCounter++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (indexOfCounter == counters.Length) break;
                    }
                    s += minSum;
                    //Console.WriteLine("Min sum for line is " + minSum);
                    //Console.WriteLine("Press any key to continue the loop.\n\n\n");
                    //Console.ReadKey();
                    if (isDebug)
                        Console.WriteLine(lineIndex + " => " + minSum);
                }
                // If no loose variables, then I have a perfect upper right triangle matrix and I should be just reading the results?
                else
                {
                    Rational sumOfVectorRational = new Rational(0,1);
                    for (int xx = 0; xx < resultVector.Length; xx++)
                    {
                        sumOfVectorRational += resultVector[xx];
                    }
                    if (sumOfVectorRational.ToDecimal() % 1 == 0)
                    {
                        s += (long)sumOfVectorRational.ToDecimal();
                        if (isDebug)
                            Console.WriteLine( lineIndex + " => " + sumOfVectorRational.ToDecimal());
                    }
                }

                lineIndex++;
            }

            return s;
        }

        private static void PrintSums(int[] fixedSolutions, List<int> fixedVariables, List<int> looseVariables, int[] counters)
        {
            var sum = fixedSolutions.Sum(x => x) + counters.Sum();
            //if (sum > 10000) return;

            for (int i = 0; i < counters.Length; i++)
            {
                Console.Write($"X{looseVariables[i]} = {counters[i]}; ");
            }
            Console.Write(" => ");
            for (int i = 0; i < fixedSolutions.Length; i++)
            {
                Console.Write($"X{fixedVariables[i]} = {fixedSolutions[i]};");

            }
            Console.WriteLine($" ====> SUM = {sum}");

        }

        private static void PrintMatrixAndVector(Rational[][] matrix, Rational[] vector)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    Console.Write(matrix[i][j].ToString().PadLeft(10));
                }

                Console.WriteLine(" | " + vector[i].ToString().PadLeft(10));
            }
        }


        private static Rational[][] GetRREFMatrix(List<int[]> buttons, int[] joltage,
            out List<int> fixedVariables,
            out List<int> looseVariables,
            out Rational[] resultVector)
        {
            bool isDebugMode = false;

            // Create result vector
            resultVector = new Rational[joltage.Length];
            for (int i = 0; i < joltage.Length; i++) resultVector[i] = new Rational(joltage[i], 1);



            // Create initial matrix with arrays
            Rational[][] matrix = new Rational[joltage.Length][];
            for (int i = 0; i < joltage.Length; i++)
            {
                matrix[i] = new Rational[buttons.Count];
                for (int j = 0; j < buttons.Count; j++)
                {
                    matrix[i][j] = new Rational(buttons[j][i], 1);
                }

            }
            if (isDebugMode)
            {
                Console.WriteLine();
                PrintMatrixAndVector(matrix, resultVector);
                Console.ReadKey();
            }

            fixedVariables = new List<int>();
            looseVariables = new List<int>();

            int nextPivotRowIndex = 0;
            for (int pivotColumn = 0; pivotColumn < buttons.Count; pivotColumn++)
            {
                int pivotRow = -1;
                for (int row = nextPivotRowIndex; row < matrix.Length; row++)
                {
                    if (matrix[row][pivotColumn].ToDecimal() != 0)
                    {
                        fixedVariables.Add(pivotColumn);

                        pivotRow = row;

                        // This is going to be the row with our pivot element.
                        // Step 1: Get the row to have 1 in pivot element.
                        if (matrix[row][pivotColumn].ToDecimal() != 1)
                        {
                            var pivotElement = matrix[row][pivotColumn];
                            // divide the whole row by the pivotElement
                            for (int j = 0; j < matrix[row].Length; j++)
                            {
                                matrix[row][j] /= pivotElement;
                            }
                            // divid the result vectors row by pivotElement
                            resultVector[row] /= pivotElement;
                        }

                        if (isDebugMode)
                        {
                            Console.WriteLine("After normalizing row " + row + " to be the pivot row for pivot column " + pivotColumn);
                            PrintMatrixAndVector(matrix, resultVector);
                            Console.ReadKey();
                        }

                        // now pivotColumn is 1, we can proceed.

                        // Step 2: Get each other row to have 0 in that column:
                        for (int modifyRow = 0; modifyRow < matrix.Length; modifyRow++)
                        {
                            if (modifyRow == row) continue;     // do not apply to myself

                            // Factor for modification
                            var factorToModifyOriginalRowAndSum = new Rational(-1* matrix[modifyRow][pivotColumn].Num, matrix[modifyRow][pivotColumn].Den);

                            // For each column in each row add the pivot row modified by the factor
                            for (int k = 0; k < matrix[modifyRow].Length; k++)
                            {
                                matrix[modifyRow][k] += factorToModifyOriginalRowAndSum * matrix[row][k];
                            }

                            // Do the same for the final vector
                            resultVector[modifyRow] += resultVector[row] * factorToModifyOriginalRowAndSum;

                            if (isDebugMode)
                            {
                                Console.WriteLine("After row " + modifyRow + " has been set to 0 in column" + pivotColumn + " with factor " + factorToModifyOriginalRowAndSum);
                                PrintMatrixAndVector(matrix, resultVector);
                                Console.ReadKey();
                            }
                        }

                        // We have pivoted around the pivotRow X
                        // Now we need to swap that row with row nextPivotRowIndex and then take it from there.
                        // We need to make the same swap in the resultVector
                        if (pivotRow != nextPivotRowIndex)
                        {
                            var tempRow = matrix[pivotRow];
                            matrix[pivotRow] = matrix[nextPivotRowIndex];
                            matrix[nextPivotRowIndex] = tempRow;

                            var tempInt = resultVector[pivotRow];
                            resultVector[pivotRow] = resultVector[nextPivotRowIndex];
                            resultVector[nextPivotRowIndex] = tempInt;
                        }

                        if (isDebugMode)
                        {
                            Console.WriteLine("After 'sort':");
                            PrintMatrixAndVector(matrix, resultVector);
                            Console.ReadKey();
                        }

                        // We are finished with indexing the row, increase counter and end loop
                        nextPivotRowIndex++;
                        break;
                    }
                }

                // If not added to fixed then add to loose
                if (!fixedVariables.Contains(pivotColumn)) looseVariables.Add(pivotColumn);

            }


            return matrix;
        }

        /// <summary>
        /// Parsing input file for part 2 - different output types
        /// </summary>
        /// <param name="line"></param>
        /// <param name="msg"></param>
        /// <param name="buttons"></param>
        /// <param name="joltage"></param>
        /// <exception cref="NotImplementedException"></exception>
        private static void ParseLine2(string line, out string msg, out List<int[]> buttons, out int[] joltage)
        {
            // Parse initial msg just to get tht length of the array, c/p from 1st problem
            joltage = null;
            buttons = new List<int[]>();
            msg = string.Empty;

            int length = line.IndexOf(']') - 1;
            for (int i = 1; i < length + 1; i++)
            {
                if (line[i] == '.') msg += "0"; else msg += "1";
            }

            line = line.Substring(length + 2).Trim();

            int arrLength = msg.Length;
            joltage = new int[arrLength];

            while (line.IndexOf("(") != -1)
            {
                line = line.Substring(line.IndexOf("(") + 1);
                length = line.IndexOf(")");
                var parsable = line.Substring(0, length);
                var parts = parsable.Split(',');

                int[] button = new int[arrLength];
                int partsIndex = 0;
                for (int i = 0; i < arrLength; i++)
                {
                    int place = partsIndex < parts.Length ? int.Parse(parts[partsIndex]) : -1;
                    if (i == place) { button[i] = 1; partsIndex++; }
                }
                buttons.Add(button);

                line = line.Substring(length);
            }

            line = line.Substring(line.IndexOf('{') + 1); // Start after {{
            line = line.Substring(0, line.Length - 1);  // Remove }
            string[] joltageParts = line.Split(',');
            for (int i = 0; i < joltageParts.Length; i++)
            {
                joltage[i] = int.Parse(joltageParts[i]);
            }

        }

        private static int SolveKnapsackFor1(string msg, List<string> buttons)
        {
            var endMsg = string.Empty.PadLeft(msg.Length, '0');
            Dictionary<string, int> solutions = new Dictionary<string, int>();
            solutions.Add(msg, 0);
            List<string> keysToRecheck = new List<string>() { msg };
            int bestSolution = int.MaxValue;

            while (true)
            {
                var listOfKeysToCheck = new List<string>();
                listOfKeysToCheck.AddRange(keysToRecheck);
                keysToRecheck.Clear();

                foreach (var key in listOfKeysToCheck)
                {
                    foreach (var button in buttons)
                    {
                        var altMsg = Toggle(key, button);
                        int currentCount = solutions[key] + 1;
                        if (solutions.ContainsKey(altMsg) && currentCount < solutions[altMsg])
                        {
                            solutions[altMsg] = currentCount;
                            keysToRecheck.Add(altMsg);
                        }
                        else if (!solutions.ContainsKey(altMsg))
                        {
                            solutions.Add(altMsg, currentCount);
                            keysToRecheck.Add(altMsg);
                        }
                    }
                }

                // Stopping conditions
                if (solutions.ContainsKey(endMsg))
                {
                    if (solutions[endMsg] < bestSolution)
                    {
                        bestSolution = solutions[endMsg];
                        return bestSolution;
                    }
                }

                //var minValue = solutions.Values.Min();
                //if (minValue >= bestSolution)
                //{
                //    return bestSolution;
                //}
            }
        }

        private static int SolveKnapsackFor2(List<int[]> buttons, int[] joltage)
        {
            Dictionary<string, int> solutions = new Dictionary<string, int>();
            solutions.Add(string.Join('|', joltage), 0);

            Dictionary<string, int[]> keyIndex = new Dictionary<string, int[]>();
            keyIndex.Add(string.Join('|', joltage), joltage);

            var finalKey = "0";
            for (int i = 0; i < joltage.Length - 1; i++) finalKey += "|0";

            List<string> keysToRecheck = new List<string>() { solutions.First().Key };
            int bestSolution = int.MaxValue;

            while (true)
            {
                Console.WriteLine($"Keys to Recheck Count: {keysToRecheck.Count}");
                if (keysToRecheck.Count == 0) break;

                var listOfKeysToCheck = new List<string>();
                listOfKeysToCheck.AddRange(keysToRecheck);
                keysToRecheck.Clear();

                foreach (var key in listOfKeysToCheck)
                {
                    foreach (var button in buttons)
                    {
                        var altJoltage = UndoButton(button, keyIndex[key]);
                        var altJoltageKey = string.Join("|", altJoltage);
                        int currentCount = solutions[key] + 1;

                        // if any new result is -1 then we stop for this option and don't add key to recheck.
                        if (altJoltage.Any(x => x < 0)) continue;

                        if (solutions.ContainsKey(altJoltageKey) && currentCount < solutions[altJoltageKey])
                        {
                            solutions[altJoltageKey] = currentCount;
                            keysToRecheck.Add(altJoltageKey);
                            keyIndex.Add(altJoltageKey, altJoltage);
                        }
                        else if (!solutions.ContainsKey(altJoltageKey))
                        {
                            solutions.Add(altJoltageKey, currentCount);
                            keysToRecheck.Add(altJoltageKey);
                            keyIndex.Add(altJoltageKey, altJoltage);
                        }
                    }
                }
            }

            if (solutions.ContainsKey(finalKey))
            {
                return solutions[finalKey];
            }
            else
            {
                throw new Exception("No solution found!");
            }
        }

        /// <summary>
        /// Method substracts button from joltage
        /// </summary>
        /// <param name="button"></param>
        /// <param name="joltage"></param>
        /// <returns></returns>
        private static int[] UndoButton(int[] button, int[] joltage)
        {
            int[] result = new int[button.Length];
            for (int i = 0; i < button.Length; i++)
            {
                result[i] = joltage[i] - button[i];
            }
            return result;
        }


        public static string Toggle(string msg, string button)
        {
            string toReturn = string.Empty;
            for (int i = 0; i < msg.Length; i++)
            {
                if (button[i] == '0')   // No toggling
                {
                    toReturn += msg[i];
                }
                else if (msg[i] == '1')
                {
                    toReturn += '0';
                }
                else
                {
                    toReturn += '1';
                }
            }
            return toReturn;
        }

        private static void ParseLine1(string line, out string msg, out List<string> buttons, out object joltage)
        {
            joltage = null;
            buttons = new List<string>();
            msg = string.Empty;

            int length = line.IndexOf(']') - 1;
            for (int i = 1; i < length + 1; i++)
            {
                if (line[i] == '.') msg += "0"; else msg += "1";
            }

            line = line.Substring(length + 2).Trim();

            while (line.IndexOf("(") != -1)
            {
                line = line.Substring(line.IndexOf("(") + 1);
                length = line.IndexOf(")");
                var parsable = line.Substring(0, length);
                var parts = parsable.Split(',');

                string button = string.Empty;
                int partsIndex = 0;
                for (int i = 0; i < msg.Length; i++)
                {
                    int place = partsIndex < parts.Length ? int.Parse(parts[partsIndex]) : -1;
                    if (i != place) button += "0"; else { button += "1"; partsIndex++; }
                }
                buttons.Add(button);

                line = line.Substring(length);
            }
        }
    }
}
