using System;
using System.Html;
using Numbers.Web.Controls;
using Numbers.Web.Transitions;
using Numbers.Web.ViewModels;

namespace Numbers.Web.Views
{
    public class GameView : Control
    {
        public const int Width = 600;
        public const int Height = 340;

        private GameViewModel viewModel;

        private ToolbarView toolbarView;
        private NumbersCollectionView numbersCollectionView;
        private OperatorsCollectionView operatorsCollectionView;

        private ITransition targetLabelAppearAnimation;
        private ITransition targetLabelDisappearAnimation;
        private ITransition solveAppearAnimation;
        private ITransition solveDisappearAnimation;

        private bool solved;
        private bool newGameRequested;

        public GameView(GameViewModel viewModel) :
            base("root")
        {
            this.viewModel = viewModel;

            Control targetBackground1 = new Control("target-background1");
            Control targetBackground2 = new Control("target-background2");

            Control targetBackgroundOverlay1 = new Control("target-background-overlay1");
            Control targetBackgroundOverlay2 = new Control("target-background-overlay2");

            targetBackground2.HtmlElement.AddEventListener("mousedown", e => NewGame(), false);

            toolbarView = new ToolbarView(viewModel);
            toolbarView.NewGameRequest += (sender, e) => NewGame();

            numbersCollectionView = new NumbersCollectionView(viewModel.Numbers);
            operatorsCollectionView = new OperatorsCollectionView(viewModel.Operators);

            Control targetLabel = new Label("target-label") { Text = viewModel.TargetValue.ToString() };

            AppendChild(new Control("frame")
            {
                toolbarView,
                targetBackground1,
                targetBackground2,
                numbersCollectionView,
                operatorsCollectionView,
                targetLabel,
                targetBackgroundOverlay1,
                targetBackgroundOverlay2,
            });

            AppendChild(new Control("frame-shadow"));

            viewModel.SelectionChanged += OnSelectionChanged;
            viewModel.Solved += OnSolved;

            targetLabelAppearAnimation = new ParallelTransition(
                new Transition(targetLabel.HtmlElement, "top", new PixelValueBounds(340, 280), new TransitionTiming(800)),
                new Transition(targetLabel.HtmlElement, "opacity", new DoubleValueBounds(0, 1), new TransitionTiming(800)));

            targetLabelDisappearAnimation = new ParallelTransition(
                new Transition(targetLabel.HtmlElement, "top", new PixelValueBounds(280, 340), new TransitionTiming(800)),
                new Transition(targetLabel.HtmlElement, "opacity", new DoubleValueBounds(1, 0), new TransitionTiming(800)));

            solveAppearAnimation = new ParallelTransition(
                new Keyframe(targetBackground1.HtmlElement, "visibility", "visible", 300),
                new MultiplePropertyTransition(targetBackground1.HtmlElement, new[] { "transform", "-webkit-transform" }, new ScaleValueBounds(1, 10), new TransitionTiming(2000), 200),
                new Transition(targetBackground1.HtmlElement, "top", new PixelValueBounds(80, 164), new TransitionTiming(800, TimingCurve.EaseOut), 200),
                new MultiplePropertyTransition(targetBackground1.HtmlElement, new[] { "border-radius", "-webkit-border-radius" }, new PixelValueBounds(2, 40), new TransitionTiming(1000), 200),

                new Keyframe(targetBackground2.HtmlElement, "visibility", "visible", 500),
                new MultiplePropertyTransition(targetBackground2.HtmlElement, new[] { "transform", "-webkit-transform" }, new ScaleValueBounds(1, 10), new TransitionTiming(1800), 500),
                new Transition(targetBackground2.HtmlElement, "top", new PixelValueBounds(80, 164), new TransitionTiming(800, TimingCurve.EaseOut), 200),
                new MultiplePropertyTransition(targetBackground2.HtmlElement, new [] { "border-radius", "-webkit-border-radius" }, new PixelValueBounds(2, 40), new TransitionTiming(800), 500),

                new Keyframe(numbersCollectionView.HtmlElement, "pointerEvents", "none"),
                new Keyframe(operatorsCollectionView.HtmlElement, "pointerEvents", "none"),

                new Transition(numbersCollectionView.HtmlElement, "top", new PixelValueBounds(80, 164), new TransitionTiming(800, TimingCurve.EaseOut), 200),

                new Transition(operatorsCollectionView.HtmlElement, "top", new PixelValueBounds(176, 236), new TransitionTiming(800)),
                new Transition(operatorsCollectionView.HtmlElement, "opacity", new DoubleValueBounds(1, 0), new TransitionTiming(800)),
                new Transition(targetLabel.HtmlElement, "top", new PixelValueBounds(280, 340), new TransitionTiming(800)),
                new Transition(targetLabel.HtmlElement, "opacity", new DoubleValueBounds(1, 0), new TransitionTiming(800)));


            solveDisappearAnimation = new ParallelTransition(
                new Keyframe(targetBackgroundOverlay1.HtmlElement, "visibility", "visible"),
                new Transition(targetBackgroundOverlay1.HtmlElement, "opacity", new DoubleValueBounds(0, 0.4), new TransitionTiming(1000)),
                new MultiplePropertyTransition(targetBackgroundOverlay1.HtmlElement, new[] { "transform", "-webkit-transform" }, new ScaleValueBounds(1, 10), new TransitionTiming(2000)),

                new Keyframe(targetBackgroundOverlay2.HtmlElement, "visibility", "visible", 200),
                new Transition(targetBackgroundOverlay2.HtmlElement, "opacity", new DoubleValueBounds(0, 1), new TransitionTiming(800), 200),
                new MultiplePropertyTransition(targetBackgroundOverlay2.HtmlElement, new[] { "transform", "-webkit-transform" }, new ScaleValueBounds(.1, 10), new TransitionTiming(1800), 200));
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            if (!toolbarView.IsHintPressed)
            {
                viewModel.TryCalculate();
            }
        }

        public void Run()
        {
            numbersCollectionView.StartAppearAnimation(600);
            operatorsCollectionView.StartAppearAnimation(600);
            targetLabelAppearAnimation.Start();
            toolbarView.StartAppearAnimation();
        }

        private void NewGame()
        {
            if (newGameRequested)
            {
                return;
            }

            newGameRequested = true;

            if (solved)
            {
                solveDisappearAnimation.Start();
                Window.SetTimeout(viewModel.NewGame, 2000);
            }
            else
            {
                targetLabelDisappearAnimation.Start();
                toolbarView.StartDisappearAnimation();
                numbersCollectionView.StartDisappearAnimation(600);
                operatorsCollectionView.StartDisappearAnimation(600);
                Window.SetTimeout(viewModel.NewGame, 1000);
            }
        }

        private void OnSolved(object sender, EventArgs e)
        {
            solved = true;
            solveAppearAnimation.Start();
        }
    }
}
