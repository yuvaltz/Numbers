using System;
using System.Collections.Generic;
using System.Linq;

namespace Numbers.Web
{
    public static class GameFactory
    {
        private const int MinimumTarget = 40;
        private const int MaximumTarget = 401;
        private const int TargetMean = 200;
        private const int TargetMeanSd = 100;

        private static readonly Random Random = new Random();

        public static Game CreateFromHash(string value)
        {
            IEnumerable<int> values = value.Split("-").Select(valueString => Int32.Parse(valueString)).Take(7).ToArray();
            return new Game(values.Take(values.Count() - 1), values.Last());
        }

        public static Game CreateFromSolutionRange(int minimumSolutions, int maximumSolutions)
        {
            Console.WriteLine(String.Format("Creating a game with {0}-{1} solutions", minimumSolutions, maximumSolutions));

            while (true)
            {
                IEnumerable<int> values = GenerateRandomValues();
                int target = GetRandomTarget(values, minimumSolutions, maximumSolutions);

                if (target != -1)
                {
                    return new Game(values, target);
                }
            }
        }

        private static int GetRandomTarget(IEnumerable<int> values, int minimumSolutions, int maximumSolutions)
        {
            int[] solutionsCount = Solver.GetSolutionsCount(values, MaximumTarget);

            int[] targets = solutionsCount.
                    Select((count, target) => Tuple.Create(count, target)).
                    Where(tuple => tuple.Item1 >= minimumSolutions && tuple.Item1 <= maximumSolutions).
                    Select(tuple => tuple.Item2).Where(target => target >= MinimumTarget).ToArray();

            if (targets.Length == 0)
            {
                return -1;
            }

            int randomTarget = (int)GetNormalDistributedRandom(TargetMean, TargetMeanSd);

            return targets.OrderBy(target => Math.Abs(target - randomTarget)).First();
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
