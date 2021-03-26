namespace OpenFlow_Core.Nodes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using OpenFlow_Core.Management.UserInterface;
    using OpenFlow_Core.Nodes.Connectors;
    using OpenFlow_PluginFramework.NodeSystem;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents;
    using OpenFlow_PluginFramework.NodeSystem.NodeComponents.Fields;
    using OpenFlow_PluginFramework.Primitives;

    public class DisplayNodeField : INotifyPropertyChanged
    {
        private readonly NodeField _baseField;
        private Connector _input;
        private Connector _output;
        private HorizontalAlignment _alignment;

        public DisplayNodeField(NodeField baseField)
        {
            _baseField = baseField;
            baseField.GetFlowInput().SubscribeToChange(b => RefreshInput());
            baseField.GetFlowOutput().SubscribeToChange(b => RefreshOutput());

            if (baseField is ValueField valField)
            {
                valField.ValueStoreChanged += BaseField_ValueStoreChanged;
            }

            baseField.PropertyChanged += (o, e) =>
            {
                PropertyChanged?.Invoke(this, e);
                if (baseField is ValueField valField && e.PropertyName == nameof(ValueField.DisplayedValue))
                {
                    UpdateDisplayedValue();
                }
            };

            UIs = new UIManager();

            RefreshInput();
            RefreshOutput();
            UpdateDisplayedValue();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Connector Input
        {
            get => _input;
            set
            {
                if (value != _input)
                {
                    _input = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Input)));
                }
            }
        }

        public Connector Output
        {
            get => _output;
            set
            {
                if (value != _output)
                {
                    _output = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Output)));
                }
            }
        }

        public Opacity Opacity => _baseField.Opacity;

        public Action RemoveSelf => _baseField.RemoveSelf;

        public HorizontalAlignment Alignment
        {
            get => _alignment;
            set
            {
                _alignment = value;
                NotifyPropertyChanged();
            }
        }

        public OpenFlowValue DisplayedValue { get; private set; }

        public UIManager UIs { get; }

        private void RefreshInput()
        {
            if (_baseField.GetFlowInput().Val)
            {
                if (Input is not FlowConnector)
                {
                    Input = new FlowConnector(ConnectionTypes.Input);
                }
            }
            else if (_baseField is ValueField valField && valField.InputDisplayValue != null)
            {
                if (Input is not ValueConnector)
                {
                    Input = new ValueConnector(valField.GetDisplayValue(ValueField.InputKey), ConnectionTypes.Input);
                }
            }
            else
            {
                Input = null;
            }
            RecalculateAlignment();
        }

        private void RefreshOutput()
        {
            if (_baseField.GetFlowOutput().Val)
            {
                if (Output is not FlowConnector)
                {
                    Output = new FlowConnector(ConnectionTypes.Output);
                }
            }
            else if (_baseField is ValueField valField && valField.OutputDisplayValue != null)
            {
                if (Output is not ValueConnector)
                {
                    Output = new ValueConnector(valField.GetDisplayValue(ValueField.OutputKey), ConnectionTypes.Output);
                }
            }
            else
            {
                Output = null;
            }
            RecalculateAlignment();
        }

        private void RecalculateAlignment()
        {
            Alignment =
                Input is null && Output is not null ? HorizontalAlignment.Right : (
                Input is not null && Output is null ? HorizontalAlignment.Left : 
                HorizontalAlignment.Middle);
        }

        private void BaseField_ValueStoreChanged(object sender, object e)
        {
            if (e as string is ValueField.InputKey)
            {
                RefreshInput();
            }
            else if (e as string is ValueField.OutputKey)
            {
                RefreshOutput();
            }
        }

        private void UpdateDisplayedValue()
        {
            DisplayedValue = (_baseField as ValueField)?.DisplayedValue ?? new OpenFlowValue() { Name = _baseField.Name };
            UIs.SetChildValue(DisplayedValue);
            DisplayedValue.Name = _baseField.Name;
            NotifyPropertyChanged(nameof(DisplayedValue));
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
