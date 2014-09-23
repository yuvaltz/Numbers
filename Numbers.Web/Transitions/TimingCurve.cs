using System;

namespace Numbers.Web.Transitions
{
    public class TimingCurve
    {
        public static readonly TimingCurve Ease = new TimingCurve(0.25, 0.1, 0.25, 1, "ease");
        public static readonly TimingCurve Linear = new TimingCurve(0, 0, 1, 1, "linear");
        public static readonly TimingCurve EaseIn = new TimingCurve(0.42, 0, 1, 1, "ease-in");
        public static readonly TimingCurve EaseOut = new TimingCurve(0, 0, 0.58, 1, "ease-out");
        public static readonly TimingCurve EaseInOut = new TimingCurve(0.42, 0, 0.58, 1, "ease-in-out");

        public double X1 { get; private set; }
        public double Y1 { get; private set; }
        public double X2 { get; private set; }
        public double Y2 { get; private set; }

        public string Name { get; private set; }

        private TimingCurve(double x1, double y1, double x2, double y2, string name = null)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return !String.IsNullOrEmpty(Name) ? Name : String.Format("cubic-bezier({0}, {1}, {2}, {3})", X1, Y1, X2, Y2);
        }

        public static TimingCurve CubicBezier(double x1, double y1, double x2, double y2)
        {
            return new TimingCurve(x1, y1, x2, y2);
        }

        public double GetProgress(double timing)
        {
            double resultTiming;
            double resultProgress;

            FindCurvePoint((currentTiming, currentProgress) => currentTiming.CompareTo(timing), out resultTiming, out resultProgress);

            return resultProgress;
        }

        public double GetTiming(double progress)
        {
            double resultTiming;
            double resultProgress;

            FindCurvePoint((currentTiming, currentProgress) => currentProgress.CompareTo(progress), out resultTiming, out resultProgress);

            return resultProgress;
        }

        private void FindCurvePoint(Func<double, double, int> comparer, out double x, out double y)
        {
            double t = 0.5;
            double step = 0.5;
            x = 0;
            y = 0;

            for (int i = 0; i < 10; i++)
            {
                GetCurvePoint(t, out x, out y);

                if (comparer(x, y) < 0)
                {
                    t += step;
                }
                else if (comparer(x, y) > 0)
                {
                    t -= step;
                }

                step = step / 2;
            }
        }

        private void GetCurvePoint(double t, out double x, out double y)
        {
            x = (3 * X1 - 3 * X2 + 1) * t * t * t + (-6 * X1 + 3 * X2) * t * t + (3 * X1) * t;
            y = (3 * Y1 - 3 * Y2 + 1) * t * t * t + (-6 * Y1 + 3 * Y2) * t * t + (3 * Y1) * t;
        }
    }
}
