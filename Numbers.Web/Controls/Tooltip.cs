using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Numbers.Web.Transitions;

namespace Numbers.Web.Controls
{
    public enum Direction { Left, Top, Right, Bottom }

    public class Tooltip : Control
    {
        public const int AppearDuration = 300;
        public const int DisappearDuration = 400;

        private Direction arrowsDirection;
        private IEnumerable<Control> arrows;
        private IEnumerable<int> arrowsOffset;

        private Label label;

        private ITransition appearTransition;
        private ITransition disappearTransition;

        public Tooltip(string text, Direction arrowsDirection, params int[] arrowsOffset) :
            base("tooltip")
        {
            this.arrowsDirection = arrowsDirection;
            this.arrowsOffset = arrowsOffset.Clone();

            Control container = new Control("tooltip-container");

            label = new Label("tooltip-label") { Text = text };
            label.HtmlElement.Style.MinWidth = String.Format("{0}px", arrowsOffset.Max() + 8);

            this.AppendChild(container);
            this.AppendChild(label);

            arrows = arrowsOffset.Select(offset => new Control("tooltip-arrow")).ToArray();

            foreach (Control arrow in arrows)
            {
                this.AppendChild(arrow);
            }

            this.HtmlElement.Style.Visibility = "hidden";
            this.HtmlElement.Style.Opacity = "0";

            int topMargin = arrowsDirection == Direction.Top ? -15 : (arrowsDirection == Direction.Bottom ? 15 : 0);
            int leftMargin = arrowsDirection == Direction.Left ? -15 : (arrowsDirection == Direction.Right ? 15 : 0);

            string appearMargin = String.Format("{0}px 0px 0px {1}px", topMargin, leftMargin);
            string disappearMargin = String.Format("{0}px 0px 0px {1}px", -topMargin, -leftMargin);

            this.HtmlElement.AddEventListener("mousedown", () => StartDisappearAnimation());

            appearTransition = new SequentialTransition(
                new Keyframe(this.HtmlElement, "visibility", "visible"),
                new ParallelTransition(
                    new Transition(this.HtmlElement, "opacity", new DoubleValueBounds(0, 1), new TransitionTiming(AppearDuration, TimingCurve.EaseIn)),
                    new Transition(this.HtmlElement, "margin", new ValueBounds(appearMargin, "0px 0px 0px 0px"), new TransitionTiming(AppearDuration, TimingCurve.EaseOut))));

            disappearTransition = new SequentialTransition(
                new ParallelTransition(
                    new Transition(this.HtmlElement, "opacity", new DoubleValueBounds(1, 0), new TransitionTiming(DisappearDuration, TimingCurve.EaseOut)),
                    new Transition(this.HtmlElement, "margin", new ValueBounds("0px 0px 0px 0px", disappearMargin), new TransitionTiming(DisappearDuration, TimingCurve.EaseOut))),
                new Keyframe(this.HtmlElement, "visibility", "hidden"));
        }

        public void UpdateLayout()
        {
            int width = label.HtmlElement.ClientWidth;
            int height = label.HtmlElement.ClientHeight;

            this.HtmlElement.Style.Width = String.Format("{0}px", width);
            this.HtmlElement.Style.Height = String.Format("{0}px", height);

            foreach (Tuple<Control, int> tuple in arrows.Zip(arrowsOffset, (arrow, offset) => Tuple.Create(arrow, offset)))
            {
                tuple.Item1.Left = arrowsDirection == Direction.Left ? 0 : (arrowsDirection == Direction.Right ? width : tuple.Item2);
                tuple.Item1.Top = arrowsDirection == Direction.Top ? 0 : (arrowsDirection == Direction.Bottom ? height : tuple.Item2);
            };
        }

        public void StartAppearAnimation()
        {
            UpdateLayout();
            appearTransition.Start();
        }

        public void StartDisappearAnimation()
        {
            disappearTransition.Start();
        }
    }
}
