using System;
using System.Html;
using System.Linq;
using Numbers.Web.Controls;
using Numbers.Web.Generic;
using Numbers.Web.ViewModels;

namespace Numbers.Web.Views
{
    public class NumbersCollectionView : Control
    {
        private const int NumberWidth = 80;
        private const int NumberMargin = 8;
        private const int NumbersCollectionWidth = 600;

        private ConvertedObservableCollection<NumberViewModel, Button> numbersButtons;

        public NumbersCollectionView(ObservableCollection<NumberViewModel> viewModel) :
            base("numbers-panel")
        {
            numbersButtons = new ConvertedObservableCollection<NumberViewModel, Button>(viewModel, CreateButton);
            numbersButtons.CollectionChanged += OnNumbersButtonsCollectionChanged;

            foreach (Button button in numbersButtons)
            {
                AddButton(button);
            }

            UpdateLayout();
        }

        public void StartAppearAnimation(int totalAppearDurationMilliseconds)
        {
            foreach (Button button in numbersButtons)
            {
                int start = totalAppearDurationMilliseconds * button.Left / NumbersCollectionWidth;
                Window.SetTimeout(button.StartAppearAnimation, start);
            }
        }

        public void StartDisappearAnimation(int totalDisappearDurationMilliseconds)
        {
            foreach (Button button in numbersButtons)
            {
                int start = totalDisappearDurationMilliseconds * button.Left / NumbersCollectionWidth;
                Window.SetTimeout(button.StartDisappearAnimation, start);
            }
        }

        private void UpdateLayout()
        {
            int left = (NumbersCollectionWidth - numbersButtons.Count() * (NumberWidth + NumberMargin) + NumberMargin) / 2;

            foreach (Button button in numbersButtons)
            {
                button.Left = left;
                button.Shadow.Left = left;

                left += NumberWidth + NumberMargin;
            }
        }

        private void OnNumbersButtonsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Button button = e.Item as Button;

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AddButton(button);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                RemoveButton(button);
                button.Dispose();
            }
            else
            {
                throw new Exception("Collection change action is not supported");
            }

            UpdateLayout();
        }

        private void AddButton(Button button)
        {
            AppendChild(button);
            AppendChild(button.Shadow);
        }

        private void RemoveButton(Button button)
        {
            RemoveChild(button);
            RemoveChild(button.Shadow);
        }

        private static Button CreateButton(NumberViewModel numberViewModel)
        {
            Label label = new Label { Text = numberViewModel.Value.ToString() };

            label.HtmlElement.ClassList.Add("button-content");
            label.HtmlElement.ClassList.Add(GetLabelSizeClass(label.Text));

            Button button = new Button(numberViewModel.IsSelected, "number", GetLevelClass(numberViewModel)) { label };

            button.IsEnabled = !numberViewModel.IsTarget;

            button.HtmlElement.Style.GetTransitionDictionary().Set("left", "200ms");
            button.Shadow.HtmlElement.Style.GetTransitionDictionary().Set("left", "200ms");

            numberViewModel.IsSelectedChanged += (sender, e) => button.IsChecked = numberViewModel.IsSelected;
            button.IsCheckedChanged += (sender, e) => numberViewModel.IsSelected = button.IsChecked;

            if (numberViewModel.Source == CreationSource.Undo)
            {
                button.StartFadeInAnimation();
            }

            if (numberViewModel.Source == CreationSource.Result)
            {
                button.StartCreateAnimation();
            }

            return button;
        }

        private static string GetLevelClass(NumberViewModel numberViewModel)
        {
            return numberViewModel.IsTarget ? "target" : String.Format("level{0}", numberViewModel.Level);
        }

        private static string GetLabelSizeClass(string text)
        {
            if (text.Length < 4)
            {
                return "medium";
            }

            if (text.Length < 5)
            {
                return "small";
            }

            return "extra-small";
        }
    }
}
