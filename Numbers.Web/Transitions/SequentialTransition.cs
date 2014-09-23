using System;

namespace Numbers.Web.Transitions
{
    public class SequentialTransition : ITransition
    {
        public event EventHandler Completed;

        private ITransition[] transitions;
        private int currentIndex;

        public SequentialTransition(params ITransition[] transitions)
        {
            this.transitions = transitions.Clone();

            foreach (ITransition transition in transitions)
            {
                transition.Completed += OnTransitionCompleted;
            }
        }

        public void Start()
        {
            currentIndex = 0;

            if (currentIndex < transitions.Length)
            {
                transitions[currentIndex].Start();
            }
        }

        public void Stop()
        {
            if (currentIndex < transitions.Length)
            {
                transitions[currentIndex].Stop();
            }
        }

        private void OnTransitionCompleted(object sender, EventArgs e)
        {
            currentIndex++;

            if (currentIndex < transitions.Length)
            {
                transitions[currentIndex].Start();
            }
            else
            {
                RaiseCompleted();
            }
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
