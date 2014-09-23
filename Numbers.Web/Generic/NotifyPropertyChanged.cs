using System;

namespace Numbers.Web.Generic
{
    public class PropertyChangedEventArgs : EventArgs
    {
        public string PropertyName { get; private set; }

        public PropertyChangedEventArgs(string propertyName)
        {
            this.PropertyName = propertyName;
        }
    }

    public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);

    public interface INotifyPropertyChanged
    {
        event PropertyChangedEventHandler PropertyChanged;
    }
}
