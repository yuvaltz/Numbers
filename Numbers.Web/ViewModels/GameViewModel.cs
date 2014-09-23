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

        public ObservableCollection<NumberViewModel> Numbers { get; private set; }
        public IEnumerable<OperatorViewModel> Operators { get; private set; }
        public int TargetValue { get { return model.TargetValue; } }

        private List<NumberViewModel> selectedNumbers;

        private int stepsCount;
        private bool hintUsed;
        private IGameHost host;

        public GameViewModel(Game model, IGameHost host)
        {
            this.model = model;
            this.host = host;

            selectedNumbers = new List<NumberViewModel>();
            
            Numbers = new ObservableCollection<NumberViewModel>();
            Numbers.CollectionChanged += OnItemsCollectionChanged;

            foreach (Number number in model.CurrentNumbers)
            {
                Numbers.Add(new NumberViewModel(number));
            }

            Operators = new[]
            {
                new OperatorViewModel("+", Number.Add),
                new OperatorViewModel("-", Number.Subtract),
                new OperatorViewModel("\u00D7", Number.Multiply),
                new OperatorViewModel("\u00F7", Number.Divide),
            };

            foreach (OperatorViewModel operatorViewModel in Operators)
            {
                operatorViewModel.IsSelectedChanged += OnOperatorIsSelectedChanged;
            }
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

            foreach (OperatorViewModel operatorViewModel in Operators)
            {
                operatorViewModel.IsSelected = false;
            }

            foreach (NumberViewModel numberViewModel in Numbers)
            {
                numberViewModel.IsSelected = false;
            }

            selectedNumbers.Clear();

            NumberViewModel operand1ViewModel = new NumberViewModel(number.Operand1, source: CreationSource.Undo);
            NumberViewModel operand2ViewModel = new NumberViewModel(number.Operand2, source: CreationSource.Undo);

            InsertNumber(operand1ViewModel);
            InsertNumber(operand2ViewModel);
        }

        public void Hint()
        {
            Number number = model.Hint();
            hintUsed = true;

            if (number != null)
            {
                Push(number);
            }
            else
            {
                Undo();
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

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                (e.Item as NumberViewModel).IsSelectedChanged -= OnNumberIsSelectedChanged;
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                (e.Item as NumberViewModel).IsSelectedChanged += OnNumberIsSelectedChanged;
            }
            else
            {
                throw new Exception("Unsupported collection action");
            }
        }

        private void OnNumberIsSelectedChanged(object sender, EventArgs e)
        {
            NumberViewModel numberViewModel = sender as NumberViewModel;

            if (numberViewModel.IsSelected)
            {
                selectedNumbers.Add(numberViewModel);

                if (selectedNumbers.Count > 2)
                {
                    selectedNumbers[0].IsSelected = false;
                }
            }
            else
            {
                selectedNumbers.Remove(numberViewModel);
            }

            TryCalculate();
        }

        private void OnOperatorIsSelectedChanged(object sender, EventArgs e)
        {
            OperatorViewModel senderOperatorViewModel = sender as OperatorViewModel;

            if (senderOperatorViewModel.IsSelected)
            {
                foreach (OperatorViewModel operatorViewModel in Operators)
                {
                    operatorViewModel.IsSelected = operatorViewModel == senderOperatorViewModel;
                }
            }

            TryCalculate();
        }

        private void TryCalculate()
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

            foreach (OperatorViewModel operatorViewModel in Operators)
            {
                operatorViewModel.IsSelected = false;
            }

            foreach (NumberViewModel numberViewModel in Numbers)
            {
                numberViewModel.IsSelected = false;
            }

            selectedNumbers.Clear();

            NumberViewModel resultViewModel = new NumberViewModel(number, Numbers.Count == 0 && number.Value == TargetValue, CreationSource.Result);

            resultViewModel.IsSelected = Numbers.Count > 0;

            if (resultViewModel.IsSelected)
            {
                selectedNumbers.Add(resultViewModel);
            }

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

        private void RaiseSolved()
        {
            if (Solved != null)
            {
                Solved(this, EventArgs.Empty);
            }
        }
    }
}
