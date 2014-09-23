using System;
using System.Html;

namespace Numbers.Web.Transitions
{
    public class Keyframe : ITransition
    {
        public event EventHandler Completed;

        private Element targetElement;
        private string targetProperty;
        private string keyframeValue;
        private int delay;
        private int cancellationToken;

        public Keyframe(Element targetElement, string targetProperty, string keyframeValue, int delayMilliseconds = 0)
        {
            this.targetElement = targetElement;
            this.targetProperty = targetProperty;
            this.keyframeValue = keyframeValue;
            this.delay = delayMilliseconds;
        }

        public void Start()
        {
            if (delay == 0)
            {
                SetKeyframeValue();
            }
            else
            {
                cancellationToken = Window.SetTimeout(SetKeyframeValue, delay);
            }
        }

        public void Stop()
        {
            Window.ClearTimeout(cancellationToken);
        }

        private void SetKeyframeValue()
        {
            targetElement.Style[targetProperty] = keyframeValue;
            RaiseCompleted();
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
