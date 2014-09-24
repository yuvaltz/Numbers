using System;
using System.Collections.Generic;
using System.Linq;
using Numbers.Web.Generic;

namespace Numbers.Web.ViewModels
{
    public class GameViewModel
    {
        private Game model;

        public event EventHandler Solved;
        public event EventHandler SelectionChanged;

        public ObservableCollection<NumberViewModel> Numbers { get; private set; }
        public IEnumerable<OperatorViewModel> Operators { get; private set; }
        public int TargetValue { get { return model.TargetValue; } }

        private int stepsCount;
        private bool hintUsed;
        private IGameHost host;

        public GameViewModel(Game model, IGameHost host)
        {
            this.model = model;
            this.host = host;

            Numbers = new ObservableCollection<NumberViewModel>();

            foreach (Number number in model.CurrentNumbers)
            {
                Numbers.Add(new NumberViewModel(number));
            }

            Operators = new[]
            {
                new OperatorViewModel(Operator.Add, Number.Add),
                new OperatorViewModel(Operator.Subtract, Number.Subtract),
                new OperatorViewModel(Operator.Multiply, Number.Multiply),
                new OperatorViewModel(Operator.Divide, Number.Divide),
            };

            CyclicSelectionBehavior numbersSelectionBehavior = new CyclicSelectionBehavior(Numbers, 2);
            CyclicSelectionBehavior operatorsSelectionBehavior = new CyclicSelectionBehavior(Operators, 1);

            numbersSelectionBehavior.SelectionChanged += (sender, e) => RaiseSelectionChanged();
            operatorsSelectionBehavior.SelectionChanged += (sender, e) => RaiseSelectionChanged();
        }

        public void Undo()
        {
            if (stepsCount == 0)
            {
                host.RestorePreviousGame();
                return;
            }

            Number number = model.Pop();

            if (number == null)
            {
                return;
            }

            Numbers.Remove(Numbers.FirstOrDefault(vm => vm.Model == number));

            InsertNumber(new NumberViewModel(number.Operand1, source: CreationSource.Undo));
            InsertNumber(new NumberViewModel(number.Operand2, source: CreationSource.Undo));

            ClearSelection();
        }

        public Number Hint()
        {
            Number number = model.Hint();
            hintUsed = true;

            return number;
        }

        public void SetSelection(Number number)
        {
            foreach (NumberViewModel numberViewModel in Numbers)
            {
                numberViewModel.IsSelected = numberViewModel.Model == number.Operand1 || numberViewModel.Model == number.Operand2;
            }

            foreach (OperatorViewModel operatorViewModel in Operators)
            {
                operatorViewModel.IsSelected = operatorViewModel.Operator == number.Operator;
            }
        }

        public void NewGame()
        {
            LevelChange levelChange = LevelChange.Same;

            if (stepsCount > 0)
            {
                if (!model.IsSolved)
                {
                    levelChange = LevelChange.Easier;
                }
                else if (!hintUsed && stepsCount < 20)
                {
                    levelChange = LevelChange.Harder;
                }
            }

            host.NewGame(levelChange);
        }

        public void TryCalculate()
        {
            OperatorViewModel operatorViewModel = Operators.Where(vm => vm.IsSelected).FirstOrDefault();
            NumberViewModel[] numberViewModels = Numbers.Count() == 2 ? Numbers.ToArray() : Numbers.Where(vm => vm.IsSelected).ToArray();

            if (operatorViewModel == null || numberViewModels.Length != 2)
            {
                return;
            }

            Number result = operatorViewModel.Calculate(numberViewModels[0], numberViewModels[1]);

            if (result != null)
            {
                Push(result);
            }
        }

        private void Push(Number number)
        {
            model.Push(number);

            stepsCount++;

            Numbers.Remove(Numbers.FirstOrDefault(vm => vm.Model == number.Operand1));
            Numbers.Remove(Numbers.FirstOrDefault(vm => vm.Model == number.Operand2));

            ClearSelection();

            NumberViewModel resultViewModel = new NumberViewModel(number, Numbers.Count == 0 && number.Value == TargetValue, CreationSource.Result);
            resultViewModel.IsSelected = Numbers.Count > 0;

            InsertNumber(resultViewModel);

            if (model.IsSolved)
            {
                RaiseSolved();
            }
        }

        private void InsertNumber(NumberViewModel numberViewModel)
        {
            int index = Numbers.IndexOf(Numbers.FirstOrDefault(vm => vm.Value > numberViewModel.Value));

            if (index == -1)
            {
                Numbers.Add(numberViewModel);
            }
            else
            {
                Numbers.Insert(index, numberViewModel);
            }
        }

        private void ClearSelection()
        {
            foreach (OperatorViewModel operatorViewModel in Operators)
            {
                operatorViewModel.IsSelected = false;
            }

            foreach (NumberViewModel numberViewModel in Numbers)
            {
                numberViewModel.IsSelected = false;
            }
        }

        private void RaiseSolved()
        {
            if (Solved != null)
            {
                Solved(this, EventArgs.Empty);
            }
        }

        private void RaiseSelectionChanged()
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(this, EventArgs.Empty);
            }
        }
    }
}
