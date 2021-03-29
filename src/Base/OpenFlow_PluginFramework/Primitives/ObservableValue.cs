using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_PluginFramework.Primitives
{
    public class ObservableValue<T> : INotifyPropertyChanged
    {
        private T _value;

        public ObservableValue(T initialValue)
        {
            _value = initialValue;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public T Value
        {
            get => _value;
            set
            {
                if (!value.Equals(_value))
                {
                    _value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
        }

        public static implicit operator T(ObservableValue<T> observable) => observable.Value;
    }
}
