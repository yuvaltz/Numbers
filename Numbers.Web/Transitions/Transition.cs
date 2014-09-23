using System;
using System.Html;

namespace Numbers.Web.Transitions
{
    public interface ITransition
    {
        event EventHandler Completed;
        void Start();
        void Stop();
    }

    public class Transition : ITransition
    {
        public enum ContinuationMode { Restart, ContinueValue, ContinueValueAndTime }

        private enum State { Stopped, Pending, Running }

        public event EventHandler Completed;

        private Element targetElement;
        private string targetProperty;
        private IValueBounds valueBounds;
        private TransitionTiming timing;
        private int delay;
        private ContinuationMode continuationMode;

        private State state;
        private int cancellationToken;

        public Transition(Element targetElement, string targetProperty, IValueBounds bounds, TransitionTiming timing, int delayMilliseconds = 0, ContinuationMode continuationMode = ContinuationMode.Restart)
        {
            this.targetElement = targetElement;
            this.targetProperty = targetProperty;
            this.valueBounds = bounds;
            this.timing = timing;
            this.delay = delayMilliseconds;
            this.continuationMode = continuationMode;
        }

        public void Start()
        {
            if (state != State.Stopped)
            {
                Stop();
            }

            state = State.Pending;

            cancellationToken = Window.SetTimeout(() =>
            {
                if (state != State.Pending)
                {
                    return;
                }

                state = State.Running;

                string currentValue = valueBounds.FormattedStartValue;
                TransitionTiming currentTiming = timing;

                if (continuationMode == ContinuationMode.ContinueValue || continuationMode == ContinuationMode.ContinueValueAndTime)
                {
                    currentValue = WindowExtensions.GetComputedStyle(targetElement).GetPropertyValue(targetProperty);
                }

                if (continuationMode == ContinuationMode.ContinueValueAndTime)
                {
                    double currentProgress = valueBounds.GetProgress(currentValue);
                    double currentProgressTiming = timing.Timing.GetTiming(currentProgress);

                    currentTiming = timing.AddDuration((int)(-currentProgressTiming * timing.Duration)); // also, a truncated timing curve is needed here;
                }

                targetElement.Style.GetTransitionDictionary().Clear(targetProperty);
                targetElement.Style[targetProperty] = currentValue;

                WindowExtensions.RequestAnimationFrame(() =>
                {
                    WindowExtensions.RequestAnimationFrame(() =>
                    {
                        if (state == State.Running)
                        {
                            targetElement.Style.GetTransitionDictionary().Set(targetProperty, currentTiming.ToString());
                            targetElement.Style[targetProperty] = valueBounds.FormattedEndValue;
                        }
                    });
                });

                Window.SetTimeout(() =>
                {
                    if (state == State.Running)
                    {
                        RaiseCompleted();
                    }
                }, currentTiming.Delay + currentTiming.Duration);
            }, delay);
        }

        public void Stop()
        {
            if (state == State.Stopped)
            {
                return;
            }

            if (state == State.Pending)
            {
                Window.ClearTimeout(cancellationToken);
            }
            else if (state == State.Running)
            {
                string currentValue = WindowExtensions.GetComputedStyle(targetElement).GetPropertyValue(targetProperty);
                targetElement.Style.GetTransitionDictionary().Clear(targetProperty);
                targetElement.Style[targetProperty] = currentValue;
            }
            else
            {
                throw new Exception("Unsupported animation state");
            }

            state = State.Stopped;
        }

        private void RaiseCompleted()
        {
            if (Completed != null)
            {
                Completed(this, EventArgs.Empty);
            }
        }
    }
}
