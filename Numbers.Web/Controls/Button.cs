using System;
using System.Html;
using Numbers.Web.Transitions;

namespace Numbers.Web.Controls
{
    public class Button : Control
    {
        private const int CheckAnimationDurationMilliseconds = 100;

        public bool IsEnabled { get; set; }

        public event EventHandler IsCheckedChanged;
        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    OnIsCheckChanged();
                    Window.SetTimeout(RaiseIsCheckedChanged, CheckAnimationDurationMilliseconds);
                }
            }
        }

        public Control Shadow { get; private set; }

        private Control overlay;

        private ITransition checkedAnimation;
        private ITransition uncheckedAnimation;
        private ITransition overlayAnimation;

        public Button(bool initialCheck, params string[] classesName) :
            base("button")
        {
            this.isChecked = initialCheck;
            this.IsEnabled = true;

            foreach (string className in classesName)
            {
                HtmlElement.ClassList.Add(className);
            }

            HtmlElement.SetAttribute("data-is-checked", IsChecked.ToString());

            Shadow = new Control("button-shadow");
            overlay = new Control("button-overlay");

            AppendChild(overlay);

            if (WindowExtensions.IsTouchAvailable())
            {
                Window.AddEventListener("touchstart", OnPointerDown, false);
            }
            else
            {
                Window.AddEventListener("mousedown", OnPointerDown, false);
            }

            IValueBounds transformValueBounds = new ScaleValueBounds(1, 1.08);
            IValueBounds opacityValueBounds = new DoubleValueBounds(0, 1);

            TransitionTiming transitionTiming = new TransitionTiming(CheckAnimationDurationMilliseconds);

            checkedAnimation = new ParallelTransition(
                new MultiplePropertyTransition(HtmlElement, new [] { "transform", "-webkit-transform" }, transformValueBounds, transitionTiming),
                new MultiplePropertyTransition(Shadow.HtmlElement, new[] { "transform", "-webkit-transform" }, transformValueBounds, transitionTiming),
                new Transition(Shadow.HtmlElement, "opacity", opacityValueBounds, transitionTiming));

            uncheckedAnimation = new ParallelTransition(
                new MultiplePropertyTransition(HtmlElement, new [] { "transform", "-webkit-transform" }, transformValueBounds.Reverse(), transitionTiming),
                new MultiplePropertyTransition(Shadow.HtmlElement, new[] { "transform", "-webkit-transform" }, transformValueBounds.Reverse(), transitionTiming),
                new Transition(Shadow.HtmlElement, "opacity", opacityValueBounds.Reverse(), transitionTiming));

            overlayAnimation = new ParallelTransition(
                new MultiplePropertyTransition(overlay.HtmlElement, new[] { "transform", "-webkit-transform" }, new ScaleValueBounds(0, 1.5), new TransitionTiming(400, TimingCurve.EaseOut)),
                new Transition(overlay.HtmlElement, "opacity", new DoubleValueBounds(0, 1), new TransitionTiming(100, TimingCurve.EaseIn), 0, Transition.ContinuationMode.ContinueValueAndTime),
                new Transition(overlay.HtmlElement, "opacity", new DoubleValueBounds(1, 0), new TransitionTiming(200, TimingCurve.EaseOut), 100, Transition.ContinuationMode.ContinueValueAndTime));

            if (IsChecked)
            {
                HtmlElement.Style["transform"] = transformValueBounds.FormattedEndValue;
                HtmlElement.Style["-webkit-transform"] = transformValueBounds.FormattedEndValue;
                Shadow.HtmlElement.Style["transform"] = transformValueBounds.FormattedEndValue;
                Shadow.HtmlElement.Style["-webkit-transform"] = transformValueBounds.FormattedEndValue;
            }
        }

        public void StartAppearAnimation()
        {
            ITransition animation = new ParallelTransition(
                new MultiplePropertyTransition(HtmlElement, new[] { "transform", "-webkit-transform" }, new ScaleValueBounds(0, 1), new TransitionTiming(400)),
                new Transition(HtmlElement, "opacity", new DoubleValueBounds(0, 1), new TransitionTiming(400))
            );

            animation.Start();
        }

        public void StartDisappearAnimation()
        {
            ITransition animation = new ParallelTransition(
                new Keyframe(Shadow.HtmlElement, "visibility", "hidden"),
                new MultiplePropertyTransition(HtmlElement, new[] { "transform", "-webkit-transform" }, new ScaleValueBounds(1, 0), new TransitionTiming(400)),
                new Transition(HtmlElement, "opacity", new DoubleValueBounds(1, 0), new TransitionTiming(400))
            );

            animation.Start();
        }

        public void StartCreateAnimation()
        {
            ITransition transformTransition = new MultiplePropertyTransition(HtmlElement, new[] { "transform", "-webkit-transform" }, new ScaleValueBounds(1.4, IsChecked ? 1.08 : 1), new TransitionTiming(400));
            ITransition opacityTransition = new Transition(HtmlElement, "opacity", new DoubleValueBounds(0, 1), new TransitionTiming(400));
            ITransition shadowTransformTransition = new MultiplePropertyTransition(Shadow.HtmlElement, new[] { "transform", "-webkit-transform" }, new ScaleValueBounds(1.4, IsChecked ? 1.08 : 1), new TransitionTiming(400));
            ITransition shadowOpacityTransition = new Transition(Shadow.HtmlElement, "opacity", new DoubleValueBounds(0, 1), new TransitionTiming(400));

            ITransition scaleOutAnimation = IsChecked ?
                new ParallelTransition(transformTransition, opacityTransition, shadowTransformTransition, shadowOpacityTransition) :
                new ParallelTransition(transformTransition, opacityTransition);

            scaleOutAnimation.Start();
        }

        public void StartFadeInAnimation()
        {
            ITransition animation = new Transition(HtmlElement, "opacity", new DoubleValueBounds(0, 1), new TransitionTiming(1400));

            animation.Start();
        }

        private void OnPointerDown(Event e)
        {
            EventTarget target = e.Target ?? e.GetSrcElement();

            if (target != this.HtmlElement)
            {
                return;
            }


            if (IsEnabled)
            {
                IsChecked = !IsChecked;

                UIEvent uiEvent = e as UIEvent;
                overlay.HtmlElement.Style["transformOrigin"] = uiEvent.LayerY == 0 && uiEvent.LayerY == 0 ? "50% 50%" : String.Format("{0}px {1}px", uiEvent.LayerX, uiEvent.LayerY);
                overlayAnimation.Start();
            }

            e.PreventDefault();
        }

        private void OnIsCheckChanged()
        {
            if (IsChecked)
            {
                uncheckedAnimation.Stop();
                checkedAnimation.Start();
            }
            else
            {
                checkedAnimation.Stop();
                uncheckedAnimation.Start();
            }

            HtmlElement.SetAttribute("data-is-checked", IsChecked.ToString());
        }

        private void RaiseIsCheckedChanged()
        {
            if (IsCheckedChanged != null)
            {
                IsCheckedChanged(this, EventArgs.Empty);
            }
        }
    }
}
