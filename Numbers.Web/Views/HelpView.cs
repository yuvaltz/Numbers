using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Numbers.Web.Controls;
using Numbers.Web.ViewModels;
using System.Html;

namespace Numbers.Web.Views
{
    public class HelpView : Control
    {
        private GameViewModel viewModel;

        private List<Tooltip> tooltips;
        private List<int> timeouts;

        private bool welcomeTooltipAdded;
        private bool target1TooltipAdded;
        private bool target2TooltipAdded;
        private bool hintTooltipAdded;
        private bool operation1TooltipsAdded;
        private bool operation2TooltipsAdded;

        public HelpView(GameViewModel viewModel) :
            base("help-overlay")
        {
            this.viewModel = viewModel;

            tooltips = new List<Tooltip>();
            timeouts = new List<int>();

            viewModel.NumbersChanged += OnNumbersChanged;

            AddTooltips();
        }

        private void OnNumbersChanged(object sender, EventArgs e)
        {
            ClearTooltips();
            AddTooltips();
        }

        private void AddTooltips()
        {
            Number operation = viewModel.Hint();

            AddWelcomeTooltip();

            if (operation == null)
            {
                if (!viewModel.IsSolved)
                {
                    AddUndoTooltip();
                }
            }
            else if (viewModel.Numbers.Count > 3)
            {
                AddOperationTooltips(operation);

                if (viewModel.Numbers.Count == 6)
                {
                    AddTargetTooltip1();
                }
            }
            else if (viewModel.Numbers.Count == 3)
            {
                AddTargetTooltip2();
                AddHintTooltip();
            }
        }

        private void AddWelcomeTooltip()
        {
            if (welcomeTooltipAdded)
            {
                return;
            }

            welcomeTooltipAdded = true;

            AddTooltip(new Tooltip("Welcome!", Direction.Bottom, 56) { Top = -16, Left = 328 }, 1000, 3000);
            AddTooltip(new Tooltip("Just a quick tour before you start", Direction.Bottom, 164) { Top = -16, Left = 220 }, 5000, 5000);
        }

        private void AddTargetTooltip1()
        {
            if (target1TooltipAdded)
            {
                return;
            }

            target1TooltipAdded = true;

            AddTooltip(new Tooltip("This is your target", Direction.Left, 16) { Top = 288, Left = 360 }, 9000);
        }

        private void AddTargetTooltip2()
        {
            if (target2TooltipAdded)
            {
                return;
            }

            target2TooltipAdded = true;

            AddTooltip(new Tooltip("Find a way to get here", Direction.Left, 16) { Top = 288, Left = 360 }, 1000);
        }

        private void AddHintTooltip()
        {
            if (hintTooltipAdded)
            {
                return;
            }

            hintTooltipAdded = true;

            AddTooltip(new Tooltip("Touch and hold for a hint", Direction.Right, 16) { Top = 16, Left = 224 }, 3000);
        }

        private void AddUndoTooltip()
        {
            AddTooltip(new Tooltip("It's a dead end, undo", Direction.Right, 16) { Top = 16, Left = 320 }, 1000);
        }

        private void AddOperationTooltips(Number operation)
        {
            int delay = operation1TooltipsAdded ? 1000 : 13000;

            operation1TooltipsAdded = true;

            string singleTooltipFormat;
            string firstTooltipFormat;

            if (viewModel.Numbers.Count == 6)
            {
                singleTooltipFormat = "Try to {0} these two";
                firstTooltipFormat = "Try to {0} this one";
            }
            else if (viewModel.Numbers.Count == 5 && !operation2TooltipsAdded)
            {
                singleTooltipFormat = "now {0} these two";
                firstTooltipFormat = "now {0} this one";
                operation2TooltipsAdded = true;
            }
            else
            {
                singleTooltipFormat = "...{0} these two";
                firstTooltipFormat = "...{0} this one";
            }

            int index1 = viewModel.Numbers.IndexOf(viewModel.Numbers.Where(numberViewModel => numberViewModel.Model == operation.Operand1).First());
            int index2 = viewModel.Numbers.IndexOf(viewModel.Numbers.Where(numberViewModel => numberViewModel.Model == operation.Operand2).First());

            if (index1 > index2)
            {
                int index = index1;
                index1 = index2;
                index2 = index;
            }

            int numbersOffset = (GameView.Width - viewModel.Numbers.Count * (NumbersCollectionView.NumberWidth + NumbersCollectionView.NumberMargin) + NumbersCollectionView.NumberMargin) / 2;

            int operand1Left = numbersOffset + index1 * (NumbersCollectionView.NumberWidth + NumbersCollectionView.NumberMargin) + NumbersCollectionView.NumberWidth / 2;
            int operand2Left = numbersOffset + index2 * (NumbersCollectionView.NumberWidth + NumbersCollectionView.NumberMargin) + NumbersCollectionView.NumberWidth / 2;

            if (index2 - index1 < 3)
            {
                int tooltipLeft = Math.Min(numbersOffset + index1 * (NumbersCollectionView.NumberWidth + NumbersCollectionView.NumberMargin), GameView.Width - 264);

                string tooltipText = String.Format(singleTooltipFormat, GetOperatorName(operation.Operator));

                AddTooltip(new Tooltip(tooltipText, Direction.Bottom, operand1Left - tooltipLeft, operand2Left - tooltipLeft) { Left = tooltipLeft, Top = 34 }, delay);
            }
            else
            {
                int tooltip1Left = numbersOffset + index1 * (NumbersCollectionView.NumberWidth + NumbersCollectionView.NumberMargin);
                int tooltip2Left = Math.Min(numbersOffset + index2 * (NumbersCollectionView.NumberWidth + NumbersCollectionView.NumberMargin), GameView.Width - 136);

                string tooltip1Text = String.Format(firstTooltipFormat, GetOperatorName(operation.Operator));

                AddTooltip(new Tooltip(tooltip1Text, Direction.Bottom, operand1Left - tooltip1Left) { Left = tooltip1Left, Top = 34 }, delay);
                AddTooltip(new Tooltip("and this one", Direction.Bottom, operand2Left - tooltip2Left) { Left = tooltip2Left, Top = 34 }, delay + 500);
            }
        }

        private void AddTooltip(Tooltip tooltip, int appearDelay, int visibleDuration = 10000)
        {
            tooltips.Add(tooltip);
            timeouts.Add(Window.SetTimeout(tooltip.StartAppearAnimation, appearDelay));
            timeouts.Add(Window.SetTimeout(tooltip.StartDisappearAnimation, appearDelay + visibleDuration));
            AppendChild(tooltip);
        }

        public void ClearTooltips()
        {
            foreach (Tooltip tooltip in tooltips)
            {
                tooltip.StartDisappearAnimation();
                Window.SetTimeout(() => RemoveChild(tooltip), Tooltip.DisappearDuration);
            }

            foreach (int timeout in timeouts)
            {
                Window.ClearTimeout(timeout);
            }
        }

        private static string GetOperatorName(Operator @operator)
        {
            switch (@operator)
            {
                case Operator.Add: return "add";
                case Operator.Subtract: return "subtract";
                case Operator.Multiply: return "multiply";
                case Operator.Divide: return "divide";
                default: throw new Exception("Unsupported operator");
            }
        }
    }
}
