using System;

namespace Numbers.Web.ViewModels
{
    public class OperatorViewModel : SelectableViewModel
    {
        public Operator Operator { get; private set; }

        private Func<Number, Number, Number> calculation;

        public OperatorViewModel(Operator @operator, Func<Number, Number, Number> calculation)
        {
            this.Operator = @operator;
            this.calculation = calculation;
        }

        public Number Calculate(NumberViewModel a, NumberViewModel b)
        {
            return calculation(a.Model, b.Model);
        }
    }
}
