using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2025.Tasks
{
    public static class Task11
    {
        public static long Part1()
        {
            long s = 0;
            var lines = File.ReadAllLines("../../../Inputs/11.1.txt");

            Dictionary<string, List<string>> map = new();
            HashSet<string> allNodes = new();

            foreach (var line in lines)
            {
                var parts = line.Split(": ");
                allNodes.Add(parts[0]);
                var outnodes = parts[1].Split(" ");
                foreach (var outnode in outnodes)
                {
                    allNodes.Add(outnode);
                    if (!map.ContainsKey(outnode)) map.Add(outnode, new List<string>());

                    map[outnode].Add(parts[0]);
                }
            }

            // if there are nodes that exist that don't have a path to them, add them to map with an empty list!
            var emptyNodes = allNodes.Where(n => !map.ContainsKey(n));
            foreach (var item in emptyNodes)
            {
                map.Add(item, new());
            }

            // Remove all nodes I can not reach iteratively
            bool supstitutionsMade = false;
            do
            {
                supstitutionsMade = false;
                var nodesToRemove = map.Where(m => m.Value.Count == 0 && m.Key != "you").Select(n => n.Key).ToList();
                if (nodesToRemove.Count > 0)
                {
                    supstitutionsMade = true;
                    nodesToRemove.ForEach(n =>
                    {
                        map.Remove(n);
                        var nodesThatContain = map.Where(m => m.Value.Contains(n)).ToList();
                        nodesThatContain.ForEach(cont =>
                        {
                            cont.Value.Remove(n);
                        });
                    });
                }

            } while (supstitutionsMade);


            Dictionary<string, int> solutions = new Dictionary<string, int>();
            List<string> keysToRecheck = new List<string>() { "you" };
            solutions.Add("you", 1);

            while (map.ContainsKey("out") && map["out"].Count > 0)
            {
                List<string> keysToRecheckInNextStep = new();
                foreach (var key in keysToRecheck)
                {
                    // Add all nodes that you can enter from "you"
                    var youNodes = map.Where(m => m.Value.Contains(key)).ToList();
                    youNodes.ForEach(n =>
                    {
                        if (solutions.ContainsKey(n.Key)) solutions[n.Key] += solutions[key];
                        else solutions.Add(n.Key, solutions[key]);
                        n.Value.Remove(key);
                    });
                    keysToRecheckInNextStep.AddRange(youNodes.Where(n => n.Value.Count == 0).Select(n => n.Key).ToList());
                    foreach (var rem in keysToRecheckInNextStep) map.Remove(rem);
                }

                keysToRecheck = keysToRecheckInNextStep;
            }

            //while (true)
            //{

            //    if (keysToRecheck.Count == 0) break;

            //    var listOfKeysToCheck = new List<string>();
            //    listOfKeysToCheck.AddRange(keysToRecheck);
            //    keysToRecheck.Clear();

            //    foreach (var key in listOfKeysToCheck)
            //    {
            //        if (map.ContainsKey(key))
            //        foreach (var node in map[key])
            //        {
            //            int currentCount = solutions[key] + 1;
            //            if (solutions.ContainsKey(node) && currentCount < solutions[node])
            //            {
            //                solutions[node] = currentCount;
            //                keysToRecheck.Add(node);
            //            }
            //            else if (!solutions.ContainsKey(node))
            //            {
            //                solutions.Add(node, currentCount);
            //                keysToRecheck.Add(node);
            //            }
            //        }
            //    }
            //}

            //if (solutions.ContainsKey("you")) return solutions["you"]; else throw new Exception("No solution!");
            return solutions["out"];

        }

        public static long Part2()
        {
            long s = 0;
            var lines = File.ReadAllLines("../../../Inputs/11.1.txt");

            Dictionary<string, List<string>> map = new();
            HashSet<string> allNodes = new();

            foreach (var line in lines)
            {
                var parts = line.Split(": ");
                allNodes.Add(parts[0]);
                var outnodes = parts[1].Split(" ");
                foreach (var outnode in outnodes)
                {
                    allNodes.Add(outnode);
                    if (!map.ContainsKey(outnode)) map.Add(outnode, new List<string>());

                    map[outnode].Add(parts[0]);
                }
            }

            // if there are nodes that exist that don't have a path to them, add them to map with an empty list!
            var emptyNodes = allNodes.Where(n => !map.ContainsKey(n));
            foreach (var item in emptyNodes)
            {
                map.Add(item, new());
            }

            // Remove all nodes I can not reach iteratively
            bool supstitutionsMade = false;
            do
            {
                supstitutionsMade = false;
                var nodesToRemove = map.Where(m => m.Value.Count == 0 && m.Key != "svr").Select(n => n.Key).ToList();
                if (nodesToRemove.Count > 0)
                {
                    supstitutionsMade = true;
                    nodesToRemove.ForEach(n =>
                    {
                        map.Remove(n);
                        var nodesThatContain = map.Where(m => m.Value.Contains(n)).ToList();
                        nodesThatContain.ForEach(cont =>
                        {
                            cont.Value.Remove(n);
                        });
                    });
                }

            } while (supstitutionsMade);


            Dictionary<string, long> solutions = new();
            Dictionary<string, long> solutionsWithFft = new();
            Dictionary<string, long> solutionsWithDac = new();
            Dictionary<string, long> solutionsWithBoth = new();

            Dictionary<string, List<HashSet<string>>> paths = new();
            List<string> keysToRecheck = new List<string>() { "svr" };
            solutions.Add("svr", 1);
            solutionsWithFft.Add("svr", 0);
            solutionsWithDac.Add("svr", 0);
            solutionsWithBoth.Add("svr", 0);
            paths.Add("svr", new() { new() { "svr " } });

            while (map.ContainsKey("out") && map["out"].Count > 0)
            {
                List<string> keysToRecheckInNextStep = new();
                foreach (var key in keysToRecheck)
                {
                    // Add all nodes that you can enter from "you"
                    var youNodes = map.Where(m => m.Value.Contains(key)).ToList();
                    youNodes.ForEach(n =>
                    {
                        if (!solutionsWithDac.ContainsKey(n.Key)) solutionsWithDac.Add(n.Key, 0);
                        if (!solutionsWithFft.ContainsKey(n.Key)) solutionsWithFft.Add(n.Key, 0);
                        if (!solutionsWithBoth.ContainsKey(n.Key)) solutionsWithBoth.Add(n.Key, 0);

                        // Add all paths
                        if (solutions.ContainsKey(n.Key))
                        {
                            solutions[n.Key] += solutions[key];
                        }
                        else
                        {
                            solutions.Add(n.Key, solutions[key]);
                        }

                        bool isFft = n.Key == "fft";
                        bool isDac = n.Key == "dac";
                        bool isPredecessorFft = solutionsWithFft[key] != 0;
                        bool isPredecessorDac = solutionsWithDac[key] != 0;
                        bool isPredecessorBoth = solutionsWithBoth[key] != 0;

                        // If previous branch was somehting, then this branch's property for sure needs to be increased AT LEAST by that!
                        solutionsWithDac[n.Key] += solutionsWithDac[key];
                        solutionsWithFft[n.Key] += solutionsWithFft[key];
                        solutionsWithBoth[n.Key] += solutionsWithBoth[key];

                        if (isFft)
                        {
                            solutionsWithFft[n.Key] += solutions[key];   // All solutions for this node will be with FFT
                        }
                        if (isDac)
                        {
                            solutionsWithDac[n.Key] += solutions[key];
                        }
                        if (isFft && isPredecessorDac && solutionsWithBoth[key] == 0)
                        {
                            solutionsWithBoth[n.Key] += solutionsWithDac[key];     // If I am the one turning Dac only to Both, then only branches that were Dac, but not Both count!
                        }
                        if (isDac && isPredecessorFft && solutionsWithBoth[key] == 0)
                        {
                            solutionsWithBoth[n.Key] += solutionsWithFft[key];     // Oposite to above
                        }

                        n.Value.Remove(key);
                    });
                    keysToRecheckInNextStep.AddRange(youNodes.Where(n => n.Value.Count == 0).Select(n => n.Key).ToList());
                    foreach (var rem in keysToRecheckInNextStep) map.Remove(rem);
                }

                keysToRecheck = keysToRecheckInNextStep;
            }


            return solutionsWithBoth["out"];

        }

    }
}
