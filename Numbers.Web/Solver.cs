using System;
using System.Collections.Generic;
using System.Linq;
using Numbers.Web.Generic;

namespace Numbers.Web
{
    public static class Solver
    {
        public static IEnumerable<Number> GetSolutions(IEnumerable<Number> numbers, int targetValue)
        {
            if (numbers.Distinct(new ComparableEqualityComparer<Number>()).Count() != numbers.Count())
            {
                throw new Exception("Values are not distinct");
            }

            return GetTargets(numbers.ToArray()).Where(target => target.Value == targetValue);
        }

        public static int CountSolutions(IEnumerable<int> values, int targetValue)
        {
            return GetSolutions(values.Select(Number.Create).ToArray(), targetValue).Count();
        }

        public static int[] CountAllSolutions(IEnumerable<int> values, int maximumTargetValue)
        {
            if (values.Distinct().Count() != values.Count())
            {
                throw new Exception("Values are not distinct");
            }

            int[] solutionsCount = Array.Repeat<int>(0, maximumTargetValue);

            foreach (Number target in GetTargets(values.Select(Number.Create).ToArray()))
            {
                if (target.Value < maximumTargetValue)
                {
                    solutionsCount[target.Value]++;
                }
            }

            return solutionsCount;
        }


        private static IEnumerable<Number> GetTargets(Number[] numbers)
        {
            if (numbers.Length == 1)
            {
                yield return numbers[0];
                yield break;
            }

            foreach (Tuple<Number[], Number[]> split in GetSplittedGroups(numbers))
            {
                foreach (Number target1 in GetTargets(split.Item1))
                {
                    foreach (Number target2 in GetTargets(split.Item2))
                    {
                        foreach (Number result in GetNumbersOperations(target1, target2))
                        {
                            yield return result;
                        }
                    }
                }
            }
        }

        private static IEnumerable<Tuple<T[], T[]>> GetSplittedGroups<T>(T[] items)
        {
            int count = 1 << (items.Length - 1);

            for (int split = 1; split < count; split++)
            {
                yield return Tuple.Create(
                    items.Where((item, i) => ((split >> i) & 1) == 0).ToArray(),
                    items.Where((item, i) => ((split >> i) & 1) == 1).ToArray());
            }
        }

        private static IEnumerable<Number> GetNumbersOperations(Number number1, Number number2)
        {
            if (number1.CompareTo(number2) < 0)
            {
                Number number3 = number1;
                number1 = number2;
                number2 = number3;
            }

            // check only these patterns (where a > b > c) and skip any other combination:
            // (a+b)+c, (a+b)-c, (a-b)+c, (a-b)-c (when c is not (d+e) or (d-e))
            // (a*b)*c, (a*b)/c, (a/b)*c, (a/b)/c (when c is not (d*e) or (d/e))

            if ((number1.Operator != Operator.Add && number1.Operator != Operator.Subtract || number1.Operand1.CompareTo(number2) > 0 && number1.Operand2.CompareTo(number2) > 0) &&
                number2.Operator != Operator.Add && number2.Operator != Operator.Subtract)
            {
                yield return Number.Add(number1, number2);
                if (number2.Value != 0)
                {
                    yield return Number.Subtract(number1, number2);
                }
            }

            if ((number1.Operator != Operator.Multiply && number1.Operator != Operator.Divide || number1.Operand1.CompareTo(number2) > 0 && number1.Operand2.CompareTo(number2) > 0) &&
                number2.Operator != Operator.Multiply && number2.Operator != Operator.Divide)
            {
                yield return Number.Multiply(number1, number2);
                if (number2.Value != 1)
                {
                    Number result = Number.Divide(number1, number2);
                    if (result != null)
                    {
                        yield return result;
                    }
                }
            }
        }

        public static Number FindInitialOperation(Number target, IEnumerable<Number> initialNumbers)
        {
            return FindInitialOperation(target, initialNumbers.ToArray());
        }

        private static Number FindInitialOperation(Number target, Number[] initialNumbers)
        {
            if (target == null)
            {
                return null;
            }

            if (initialNumbers.Contains(target.Operand1) && initialNumbers.Contains(target.Operand2))
            {
                return target;
            }

            return FindInitialOperation(target.Operand1, initialNumbers) ?? FindInitialOperation(target.Operand2, initialNumbers);
        }
    }
}
