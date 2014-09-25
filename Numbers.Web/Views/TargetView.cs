using System;
using System.Collections.Generic;
using System.Text;
using Numbers.Web.Controls;
using Numbers.Web.Transitions;

namespace Numbers.Web.Views
{
    public class TargetView : Control
    {
        private ITransition appearAnimation;
        private ITransition disappearAnimation;

        public TargetView(int targetValue) :
            base("target-panel")
        {
            Control targetLabel = new Label("target-label") { Text = targetValue.ToString() };

            appearAnimation = new ParallelTransition(
                new Transition(HtmlElement, "top", new PixelValueBounds(340, 280), new TransitionTiming(800)),
                new Transition(HtmlElement, "opacity", new DoubleValueBounds(0, 1), new TransitionTiming(800)));

            disappearAnimation = new ParallelTransition(
                new Transition(HtmlElement, "top", new PixelValueBounds(280, 340), new TransitionTiming(800)),
                new Transition(HtmlElement, "opacity", new DoubleValueBounds(1, 0), new TransitionTiming(800)));

            this.AppendChild(targetLabel);
        }

        public void StartAppearAnimation()
        {
            appearAnimation.Start();
        }

        public void StartDisappearAnimation()
        {
            disappearAnimation.Start();
        }
    }
}
