using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_Pluginframework.Primatives
{
    class Opacity : INotifyPropertyChanged
    {
        private readonly List<Opacity> opacityFactors = new();
        private double _myValue = 1.0;
        private double _factorValue = 1.0;

        public event PropertyChangedEventHandler PropertyChanged;

        public double Value
        {
            get => _myValue * _factorValue;
            set
            {
                _myValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        public void AddOpacityFactor(Opacity factor)
        {
            factor.PropertyChanged += Factor_PropertyChanged;
            opacityFactors.Add(factor);
        }

        private void Factor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _factorValue = 1.0;
            foreach (Opacity factor in opacityFactors)
            {
                _factorValue *= factor.Value;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
    }
}
