using System;
using System.Collections.Generic;
using System.Html;
using System.Linq;

namespace Numbers.Web.Transitions
{
    public class MultiplePropertyTransition : ITransition
    {
        public event EventHandler Completed;

        private ParallelTransition transition;

        public MultiplePropertyTransition(Element targetElement, IEnumerable<string> targetProperties, IValueBounds bounds, TransitionTiming timing, int delayMilliseconds = 0, Transition.ContinuationMode continuationMode = Transition.ContinuationMode.Restart)
        {
            transition = new ParallelTransition(targetProperties.Select(targetProperty => new Transition(targetElement, targetProperty, bounds, timing, delayMilliseconds, continuationMode)).ToArray());
            transition.Completed += (sender, e) => RaiseCompleted();
        }

        public void Start()
        {
            transition.Start();
        }

        public void Stop()
        {
            transition.Stop();
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
