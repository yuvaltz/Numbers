using System;

namespace Numbers.Web.Transitions
{
    public class ParallelTransition : ITransition
    {
        public event EventHandler Completed;

        private ITransition[] transitions;
        private int completedCount;

        public ParallelTransition(params ITransition[] transitions)
        {
            this.transitions = transitions.Clone();

            foreach (ITransition transition in transitions)
            {
                transition.Completed += OnTransitionCompleted;
            }
        }

        public void Start()
        {
            completedCount = 0;

            foreach (ITransition transition in transitions)
            {
                transition.Start();
            }
        }

        public void Stop()
        {
            foreach (ITransition transition in transitions)
            {
                transition.Stop();
            }
        }

        private void OnTransitionCompleted(object sender, EventArgs e)
        {
            completedCount++;

            if (completedCount == transitions.Length)
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
