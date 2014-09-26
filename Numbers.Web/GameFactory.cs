using System;
using System.Collections.Generic;
using System.Linq;

namespace Numbers.Web
{
    public static class GameFactory
    {
        private const int MinimumTarget = 40;
        private const int MaximumTarget = 401;
        private const int TargetMean = 120;
        private const int TargetMeanSd = 60;

        private static readonly Random Random = new Random();

        public static Game CreateFromHash(string value)
        {
            IEnumerable<int> hashValues = value.Split("-").Select(valueString => Int32.Parse(valueString)).Take(7).ToArray();

            IEnumerable<int> values = hashValues.Take(hashValues.Count() - 1);
            int targetValue = hashValues.Last();

            int[] solutionsCount = Solver.CountSolutions(values, MaximumTarget);

            return new Game(values, targetValue, solutionsCount[targetValue]);
        }

        public static Game CreateFromSolutionRange(int minimumSolutions, int maximumSolutions)
        {
            Console.WriteLine(String.Format("Looking for a problem with {0}-{1} solutions", minimumSolutions, maximumSolutions));

            while (true)
            {
                IEnumerable<int> values = GenerateRandomValues();
                int target;
                int solutionsCount;

                if (SelectRandomTarget(values, minimumSolutions, maximumSolutions, out target, out solutionsCount))
                {
                    Console.WriteLine(String.Format("Found {0}-{1} with {2} solutions",
                        values.Select(value => value.ToString()).Aggregate((s1, s2) => String.Format("{0}-{1}", s1, s2)), target, solutionsCount));

                    return new Game(values, target, solutionsCount);
                }
            }
        }

        private static bool SelectRandomTarget(IEnumerable<int> values, int minimumSolutions, int maximumSolutions, out int selectedTarget, out int selectedTargetSolutions)
        {
            selectedTarget = 0;
            selectedTargetSolutions = 0;

            int[] solutionsCount = Solver.CountSolutions(values, MaximumTarget);

            int[] targets = solutionsCount.
                    Select((count, target) => Tuple.Create(count, target)).
                    Where(tuple => tuple.Item1 >= minimumSolutions && tuple.Item1 <= maximumSolutions).
                    Select(tuple => tuple.Item2).Where(target => target >= MinimumTarget).ToArray();

            if (targets.Length == 0)
            {
                return false;
            }

            int preferredTarget = (int)GetNormalDistributedRandom(TargetMean, TargetMeanSd);

            selectedTarget = targets.OrderBy(target => Math.Abs(target - preferredTarget)).First();
            selectedTargetSolutions = solutionsCount[selectedTarget];
            return true;
        }

        private static double GetNormalDistributedRandom(double mean, double sd)
        {
            //Box-Muller transform
            double normalizedValue = Math.Sqrt(-2.0 * Math.Log(Random.NextDouble())) * Math.Sin(2.0 * Math.PI * Random.NextDouble());

            return mean + sd * normalizedValue;
        }

        private static IEnumerable<int> GenerateRandomValues()
        {
            List<int> values = new List<int>();

            int value = 0;
            values.Add(value += 1 + Random.Next(2));
            values.Add(value += 1 + Random.Next(3));
            values.Add(value += 1 + Random.Next(4));
            values.Add(value += 1 + Random.Next(7));
            values.Add(value += 1 + Random.Next(12));
            values.Add(value += 1 + Random.Next(20));

            return values;
        }
    }
}
