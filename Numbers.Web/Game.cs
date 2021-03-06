﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Numbers.Web
{
    public class Game
    {
        public event EventHandler Solved;

        public IEnumerable<int> InitialValues { get; private set; }
        public IEnumerable<Number> CurrentNumbers { get; private set; }
        public int TargetValue { get; private set; }
        public int SolutionsCount { get; private set; }
        public bool IsSolved { get { return CurrentNumbers.Count() == 1 && CurrentNumbers.First().Value == TargetValue; } }
        public int StepsCount { get; private set; }
        public int HintsCount { get; private set; }

        private Stack<Number> stack;

        public Game(IEnumerable<int> values, int targetValue, int solutionsCount)
        {
            this.InitialValues = values.ToArray();
            this.CurrentNumbers = values.Select(Number.Create).ToArray();
            this.TargetValue = targetValue;
            this.SolutionsCount = solutionsCount;
            this.stack = new Stack<Number>();
        }

        public override string ToString()
        {
            string valuesString = InitialValues.Select(value => value.ToString()).Aggregate((value1, value2) => String.Format("{0}-{1}", value1, value2));
            return String.Format("{0}-{1}", valuesString, TargetValue);
        }

        public void Push(Number result)
        {
            if (!CurrentNumbers.Contains(result.Operand1) ||
                !CurrentNumbers.Contains(result.Operand2))
            {
                throw new Exception("Result was not created with current numbers");
            }

            stack.Push(result);

            CurrentNumbers = CurrentNumbers.
                Where(number => number != result.Operand1 && number != result.Operand2).
                Concat(new[] { result }).ToArray();

            StepsCount++;

            if (IsSolved)
            {
                Console.WriteLine(String.Format("Solved {0}={1}", result.Value, result.ToString(false, true)));
                RaiseSolved();
            }
        }

        public Number Pop()
        {
            if (stack.Count == 0)
            {
                return null;
            }

            Number result = stack.Pop();

            CurrentNumbers = CurrentNumbers.
                Where(number => number != result).
                Concat(new[] { result.Operand1, result.Operand2 }).ToArray();

            return result;
        }

        public Number Hint()
        {
            List<Number> numbers = CurrentNumbers.ToList();
            numbers.Sort((number1, number2) => number1.CompareTo(number2));

            Number solution = Solver.GetSolution(numbers, TargetValue);

            HintsCount++;

            return solution != null ? Solver.FindInitialOperation(solution, numbers) : null;
        }

        private void RaiseSolved()
        {
            if (Solved != null)
            {
                Solved(this, EventArgs.Empty);
            }
        }
    }
}
