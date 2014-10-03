using System;
using System.Collections.Generic;
using System.Text;
using Numbers.Web.Controls;
using Numbers.Web.Transitions;
using System.Html;

namespace Numbers.Web.Views
{
    public class TargetView : Control
    {
        private ITransition appearAnimation;
        private ITransition disappearAnimation;

        private ITransition solutionsCountAppearAnimation;
        private ITransition solutionsCountDisappearAnimation;

        public TargetView(int targetValue, int solutionsCount) :
            base("target-panel")
        {
            Control solutionsLabel = new Label("target-solutions-label") { Text = String.Format("{0} solutions", solutionsCount), Top = 48 };

            Control targetLabelContainer = new Control
            {
                new Label("target-label") { Text = targetValue.ToString() },
                solutionsLabel
            };

            targetLabelContainer.HtmlElement.Style.Position = "absolute";
            targetLabelContainer.Top = 0;

            appearAnimation = new ParallelTransition(
                new Transition(HtmlElement, "top", new PixelValueBounds(336, 272), new TransitionTiming(800)),
                new Transition(HtmlElement, "opacity", new DoubleValueBounds(0, 1), new TransitionTiming(800)));

            disappearAnimation = new ParallelTransition(
                new Transition(HtmlElement, "top", new PixelValueBounds(272, 336), new TransitionTiming(800)),
                new Transition(HtmlElement, "opacity", new DoubleValueBounds(1, 0), new TransitionTiming(800)));

            solutionsCountAppearAnimation = new ParallelTransition(
                new Transition(targetLabelContainer.HtmlElement, "top", new PixelValueBounds(0, -24), new TransitionTiming(300, TimingCurve.EaseOut), 50, Transition.ContinuationMode.ContinueValueAndTime),
                new Transition(solutionsLabel.HtmlElement, "opacity", new DoubleValueBounds(0, 1), new TransitionTiming(300, TimingCurve.EaseOut), 50, Transition.ContinuationMode.ContinueValueAndTime));

            solutionsCountDisappearAnimation = new ParallelTransition(
                new Transition(targetLabelContainer.HtmlElement, "top", new PixelValueBounds(-24, 0), new TransitionTiming(300, TimingCurve.EaseIn), 0, Transition.ContinuationMode.ContinueValueAndTime),
                new Transition(solutionsLabel.HtmlElement, "opacity", new DoubleValueBounds(1, 0), new TransitionTiming(300, TimingCurve.EaseIn), 0, Transition.ContinuationMode.ContinueValueAndTime));

            HtmlElement.AddEventListener("touchstart", OnTouchStart, false);
            HtmlElement.AddEventListener("mouseenter", OnMouseEnter, false);
            HtmlElement.AddEventListener("mouseleave", OnMouseLeave, false);

            this.AppendChild(targetLabelContainer);
        }

        public void StartAppearAnimation()
        {
            appearAnimation.Start();
        }

        public void StartDisappearAnimation()
        {
            disappearAnimation.Start();
        }

        public void OnMouseEnter()
        {
            solutionsCountDisappearAnimation.Stop();
            solutionsCountAppearAnimation.Start();
        }

        public void OnMouseLeave()
        {
            solutionsCountAppearAnimation.Stop();
            solutionsCountDisappearAnimation.Start();
        }

        public void OnTouchStart()
        {
            solutionsCountAppearAnimation.Start();
            Window.SetTimeout(solutionsCountDisappearAnimation.Start, 2000);
        }
    }
}
