using System;
using System.Collections.Generic;
using System.Html;
using System.Linq;
using Numbers.Web.Controls;
using Numbers.Web.ViewModels;

namespace Numbers.Web.Views
{
    public class OperatorsCollectionView : Control
    {
        private const int OperatorWidth = 80;
        private const int OperatorMargin = 8;
        private const int OperatorsCollectionWidth = 600;

        private IEnumerable<Button> operatorsButtons;

        public OperatorsCollectionView(IEnumerable<OperatorViewModel> viewModel) :
            base("operators-panel")
        {
            int left = (OperatorsCollectionWidth - 4 * (OperatorWidth + OperatorMargin) + OperatorMargin) / 2;

            operatorsButtons = viewModel.Select(CreateButton).ToArray();

            foreach (Button button in operatorsButtons)
            {
                button.Left = left;
                button.Shadow.Left = left;

                AppendChild(button);
                AppendChild(button.Shadow);

                left += OperatorWidth + OperatorMargin;
            }
        }

        public void StartAppearAnimation(int totalAppearDurationMilliseconds)
        {
            foreach (Button button in operatorsButtons)
            {
                int start = totalAppearDurationMilliseconds * button.Left / OperatorsCollectionWidth;
                Window.SetTimeout(button.StartAppearAnimation, start);
            }
        }

        public void StartDisappearAnimation(int totalDisappearDurationMilliseconds)
        {
            foreach (Button button in operatorsButtons)
            {
                int start = totalDisappearDurationMilliseconds * button.Left / OperatorsCollectionWidth;
                Window.SetTimeout(button.StartDisappearAnimation, start);
            }
        }

        private static Button CreateButton(OperatorViewModel operatorViewModel)
        {
            Label label = new Label { Text = GetOperatorHeader(operatorViewModel.Operator) };
            label.HtmlElement.ClassList.Add("button-content");

            Button button = new Button(operatorViewModel.IsSelected, "operator") { label };
            operatorViewModel.IsSelectedChanged += (sender, e) => button.IsChecked = operatorViewModel.IsSelected;
            button.IsCheckedChanged += (sender, e) => operatorViewModel.IsSelected = button.IsChecked;

            return button;
        }

        private static string GetOperatorHeader(Operator @operator)
        {
            switch (@operator)
            {
                case Operator.Add: return "+";
                case Operator.Subtract: return "-";
                case Operator.Multiply: return "\u00D7";
                case Operator.Divide: return "\u00F7";
                default: throw new Exception("Operator is not supported");
            }
        }
    }
}
