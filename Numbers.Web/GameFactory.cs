using System;
using System.Collections.Generic;
using System.Linq;

namespace Numbers.Web
{
    public static class GameFactory
    {
        private const int MinimumTarget = 40;
        private const int MaximumTarget = 401;

        private const int EasiestTargetMean = 60;
        private const int HardestTargetMean = 120;

        private const int EasiestTargetSd = 20;
        private const int HardestTargetSd = 60;

        private const int EasiestSolutionsCount = 140;
        private const int HardestSolutionsCount = 1;

        private static readonly Random Random = new Random();

        public static Game CreateFromHash(string value)
        {
            IEnumerable<int> hashValues = value.Split("-").Select(valueString => Int32.Parse(valueString)).Take(7).ToArray();

            IEnumerable<int> values = hashValues.Take(hashValues.Count() - 1);
            int targetValue = hashValues.Last();
            int solutionsCount = Solver.CountSolutions(values, targetValue);

            return new Game(values, targetValue, solutionsCount);
        }

        public static Game CreateFromLevel(int level)
        {
            if (level < 0 || level > 100)
            {
                throw new Exception("Level must be between 0 (easiest) and 100 (hardest)");
            }

            double normalizedLevel = (double)level / 100;

            int minimumSolutionsCount = (int)((1 - normalizedLevel) * EasiestSolutionsCount + normalizedLevel * HardestSolutionsCount);
            int maximumSolutionsCount = minimumSolutionsCount + Math.Max(minimumSolutionsCount / 10, 3);

            double targetMean = (1 - normalizedLevel) * EasiestTargetMean + normalizedLevel * HardestTargetMean;
            double targetSd = (1 - normalizedLevel) * EasiestTargetSd + normalizedLevel * HardestTargetSd;

            Console.WriteLine(String.Format("Looking for a problem at level {0} with {1}-{2} solutions and target value around {3} ± {4}", level, minimumSolutionsCount, maximumSolutionsCount, (int)targetMean, (int)targetSd));

            while (true)
            {
                IEnumerable<int> values = GenerateRandomValues(normalizedLevel);
                int target;
                int solutionsCount;

                int preferredTarget = (int)GetNormalDistributedRandom(targetMean, targetSd);

                if (TrySelectTarget(values, minimumSolutionsCount, maximumSolutionsCount, preferredTarget, out target, out solutionsCount))
                {
                    Console.WriteLine(String.Format("Found {0}-{1} with {2} solutions",
                        values.Select(value => value.ToString()).Aggregate((s1, s2) => String.Format("{0}-{1}", s1, s2)), target, solutionsCount));

                    return new Game(values, target, solutionsCount);
                }
            }
        }

        private static bool TrySelectTarget(IEnumerable<int> values, int minimumSolutions, int maximumSolutions, int preferredTarget, out int selectedTarget, out int selectedTargetSolutions)
        {
            selectedTarget = 0;
            selectedTargetSolutions = 0;

            int[] solutionsCount = Solver.CountAllSolutions(values, MaximumTarget);

            int[] targets = solutionsCount.
                    Select((count, target) => Tuple.Create(count, target)).
                    Where(tuple => tuple.Item1 >= minimumSolutions && tuple.Item1 <= maximumSolutions).
                    Select(tuple => tuple.Item2).Where(target => target >= MinimumTarget).ToArray();

            if (targets.Length == 0)
            {
                return false;
            }

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

        private static IEnumerable<int> GenerateRandomValues(double normalizedLevel)
        {
            List<int> values = new List<int>();

            int value = 0;
            values.Add(value += 1 + Random.Next(2));
            values.Add(value += 1 + Random.Next(3));
            values.Add(value += 1 + Random.Next(4));
            values.Add(value += 1 + Random.Next(4 + (int)Math.Round(normalizedLevel * 4)));
            values.Add(value += 1 + Random.Next(6 + (int)Math.Round(normalizedLevel * 6)));
            values.Add(value += 1 + Random.Next(10 + (int)Math.Round(normalizedLevel * 10)));

            return values;
        }
    }
}
