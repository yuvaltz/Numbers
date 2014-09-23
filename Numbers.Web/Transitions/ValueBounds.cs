using System;
namespace Numbers.Web.Transitions
{
    public interface IValueBounds
    {
        string FormattedStartValue { get; }
        string FormattedEndValue { get; }
        double GetProgress(string formattedValue);
    }

    public static class ValueBoundsExtensions
    {
        private class ReversedValueBounds : IValueBounds
        {
            private IValueBounds source;

            public ReversedValueBounds(IValueBounds source)
	        {
                this.source = source;
	        }

            public string FormattedStartValue { get { return source.FormattedEndValue; } }
            public string FormattedEndValue { get { return source.FormattedStartValue; } }

            public double GetProgress(string formattedValue)
            {
                return 1 - source.GetProgress(formattedValue);
            }
        }

        public static IValueBounds Reverse(this IValueBounds valueBounds)
        {
            return new ReversedValueBounds(valueBounds);
        }
    }
}
