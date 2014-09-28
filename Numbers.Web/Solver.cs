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

        private static IEnumerable<Number> GetTargets(Number[] numbers, int startIndex = 0)
        {
            if (numbers.Length == 1)
            {
                yield return numbers[0];
                yield break;
            }

            Number[] newNumbers = new Number[numbers.Length - 1];

            for (int i = 0; i < numbers.Length - 1; i++)
            {
                for (int j = i + 1; j < numbers.Length; j++)
                {
                    // combinations of [i,j] below startIndex have already been checked earlier in the recursion
                    // except [i,j=last] (because the last number is a new result which only exists in the current iteration)
                    if (i < startIndex && j < numbers.Length - 1)
                    {
                        continue;
                    }

                    CopyPartialArray(numbers, ref newNumbers, i, j);

                    foreach (Number nextNumber in GetNumbersOperations(numbers[i], numbers[j]))
                    {
                        if (nextNumber == null)
                        {
                            continue;
                        }

                        newNumbers[newNumbers.Length - 1] = nextNumber;

                        foreach (Number target in GetTargets(newNumbers, Math.Max(startIndex, i)))
                        {
                            yield return target;
                        }
                    }
                }
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
                    yield return Number.Divide(number1, number2);
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

        public static Number FindSolution(IEnumerable<Number> numbers, int target)
        {
            return FindSolution(numbers.ToArray(), target);
        }

        private static Number FindSolution(Number[] numbers, int target)
        {
            if (numbers.Length == 1)
            {
                return numbers[0].Value == target ? numbers[0] : null;
            }

            Number[] newNumbers = new Number[numbers.Length - 1];

            for (int i = 0; i < numbers.Length; i++)
            {
                for (int j = 0; j < numbers.Length; j++)
                {
                    Number number1 = numbers[i];
                    Number number2 = numbers[j];

                    if (i == j || number1.Value < number2.Value)
                    {
                        continue;
                    }

                    CopyPartialArray(numbers, ref newNumbers, i, j);

                    Number[] nextNumbers = new[]
                    {
                        Number.Add(number1, number2),
                        Number.Subtract(number1, number2),
                        Number.Multiply(number1, number2),
                        Number.Divide(number1, number2),
                    };
                    
                    foreach (Number nextNumber in nextNumbers)
                    {
                        if (nextNumber == null)
                        {
                            continue;
                        }

                        newNumbers[newNumbers.Length - 1] = nextNumber;

                        Number result = FindSolution(newNumbers, target);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }

            return null;
        }

        private static void CopyPartialArray<T>(T[] source, ref T[] target, params int[] excludeIndexes)
        {
            int resultIndex = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (excludeIndexes.Contains(i))
                {
                    continue;
                }

                target[resultIndex] = source[i];
                resultIndex++;
            }
        }
    }
}
