﻿using System;
using System.Collections.Generic;
using System.Text;
using Numbers.Web.Controls;
using Numbers.Web.Transitions;
using Numbers.Web.ViewModels;
using System.Html;

namespace Numbers.Web.Views
{
    public class ToolbarView : Control
    {
        public event EventHandler NewGameRequest;

        public bool IsHintPressed { get { return hintButton.IsPressed; } }

        private GameViewModel viewModel;

        private ToolbarButton newGameButton;
        private ToolbarButton hintButton;
        private ToolbarButton undoButton;

        private ITransition buttonsAppearAnimation;
        private ITransition buttonDisappearAnimation;

        public ToolbarView(GameViewModel viewModel) :
            base("toolbar")
        {
            this.viewModel = viewModel;
            viewModel.Solved += OnSolved;

            newGameButton = new ToolbarButton("new", RaiseNewGameRequest) { new Control("toolbar-button-image", "image-new") };
            hintButton = new ToolbarButton("hint", SelectHint, CalculateHint) { new Control("toolbar-button-image", "image-help") };
            undoButton = new ToolbarButton("undo", viewModel.Undo) { new Control("toolbar-button-image", "image-undo") };

            newGameButton.IsEnabled = false;

            this.AppendChild(new Label("header", "background1") { Text = "Numbers" });
            this.AppendChild(new Label("header", "background2") { Text = "Numbers" });
            this.AppendChild(new Label("header") { Text = "Numbers" });
            this.AppendChild(newGameButton);
            this.AppendChild(hintButton);
            this.AppendChild(undoButton);

            buttonsAppearAnimation = new ParallelTransition(
                new Transition(hintButton.HtmlElement, "opacity", new DoubleValueBounds(0, 1), new TransitionTiming(800)),
                new Transition(undoButton.HtmlElement, "opacity", new DoubleValueBounds(0, 1), new TransitionTiming(800)));

            buttonDisappearAnimation = new ParallelTransition(
                new Transition(hintButton.HtmlElement, "opacity", new DoubleValueBounds(1, 0), new TransitionTiming(800)),
                new Transition(undoButton.HtmlElement, "opacity", new DoubleValueBounds(1, 0), new TransitionTiming(800)));
        }

        public void StartAppearAnimation()
        {
            buttonsAppearAnimation.Start();
            Window.SetTimeout(() => newGameButton.IsEnabled = true, 1000);
        }

        public void StartDisappearAnimation()
        {
            buttonDisappearAnimation.Start();
        }

        private void SelectHint()
        {
            Number number = viewModel.Hint();

            if (number != null)
            {
                viewModel.SetSelection(number);
            }
            else
            {
                viewModel.Undo();
            }
        }

        private void CalculateHint()
        {
            Window.SetTimeout(viewModel.TryCalculate, 100);
        }

        private void OnSolved(object sender, EventArgs e)
        {
            hintButton.IsEnabled = false;
            undoButton.IsEnabled = false;

            buttonDisappearAnimation.Start();
        }

        private void RaiseNewGameRequest()
        {
            if (NewGameRequest != null)
            {
                NewGameRequest(this, EventArgs.Empty);
            }
        }
    }
}
