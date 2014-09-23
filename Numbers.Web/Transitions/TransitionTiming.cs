using System;

namespace Numbers.Web.Transitions
{
    public class TransitionTiming
    {
        public int Duration { get; private set; }
        public int Delay { get; private set; }
        public TimingCurve Timing { get; private set; }

        public TransitionTiming(int durationMilliseconds, TimingCurve timing = null, int delayMilliseconds = 0)
        {
            this.Duration = durationMilliseconds;
            this.Delay = delayMilliseconds;
            this.Timing = timing ?? TimingCurve.Ease;
        }

        public override string ToString()
        {
            return Delay == 0 ? String.Format("{0}ms {1}", Duration, Timing) :
                String.Format("{0}ms {1} {2}ms", Duration, Timing, Delay);
        }

        public TransitionTiming AddDuration(int durationMilliseconds)
        {
            return new TransitionTiming(Duration + durationMilliseconds, Timing, Delay);
        }

        public TransitionTiming AddDelay(int delayMilliseconds)
        {
            return new TransitionTiming(Duration, Timing, Delay + delayMilliseconds);
        }
    }
}
