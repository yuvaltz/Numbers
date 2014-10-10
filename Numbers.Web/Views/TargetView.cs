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

        public TargetView(int targetValue, int solutionsCount) :
            base("target-panel")
        {
            Control solutionsLabel = new Label("target-solutions-label") { Text = String.Format("{0} {1}", solutionsCount, solutionsCount == 1 ? "solution" : "solutions") };
            solutionsLabel.HtmlElement.Style.Color = GetSolutionsCountColor(solutionsCount);

            this.AppendChild(new Label("target-label") { Text = targetValue.ToString() });
            this.AppendChild(solutionsLabel);

            appearAnimation = new ParallelTransition(
                new Transition(HtmlElement, "top", new PixelValueBounds(336, 272), new TransitionTiming(800)),
                new Transition(HtmlElement, "opacity", new DoubleValueBounds(0, 1), new TransitionTiming(800)),
                new Transition(solutionsLabel.HtmlElement, "color", new ValueBounds(GetSolutionsCountColor(solutionsCount), "rgba(0, 0, 0, 0.26)"), new TransitionTiming(2000), 4000));

            disappearAnimation = new ParallelTransition(
                new Transition(HtmlElement, "top", new PixelValueBounds(272, 336), new TransitionTiming(800)),
                new Transition(HtmlElement, "opacity", new DoubleValueBounds(1, 0), new TransitionTiming(800)));

        }

        public void StartAppearAnimation()
        {
            appearAnimation.Start();
        }

        public void StartDisappearAnimation()
        {
            disappearAnimation.Start();
        }

        private const int GradientStopCount = 4;
        private static readonly int[,] GradientStopColor = new int[,]
        {
            { 117, 117, 117 },
            { 18, 199, 0 },
            { 255, 179, 0 },
            { 229, 28, 35 },
        };

        private static string GetSolutionsCountColor(int solutionsCount)
        {
            double normalizedLevel = 1 - (double)Math.Min(solutionsCount, 100) / 100;

            double[] weight = new double[GradientStopCount];
            for (int i = 0; i < GradientStopCount; i++)
            {
                double stopPosition = (double)i / (GradientStopCount - 1);
                weight[i] = Math.Max(0, 1 - Math.Abs(stopPosition - normalizedLevel) * (GradientStopCount - 1));
            }

            double[] color = new double[3];
            for (int componentIndex = 0; componentIndex < 3; componentIndex++)
            {
                color[componentIndex] = 0;

                for (int i = 0; i < GradientStopCount; i++)
                {
                    color[componentIndex] += weight[i] * GradientStopColor[i, componentIndex];
                }
            }

            return String.Format("rgba({0}, {1}, {2}, 1)", (int)color[0], (int)color[1], (int)color[2]);
        }
    }
}
