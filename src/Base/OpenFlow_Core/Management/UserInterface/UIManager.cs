using OpenFlow_Core.Management;
using OpenFlow_PluginFramework.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_Core.Management.UserInterface
{
    public class UIManager : INotifyPropertyChanged
    {
        private OpenFlowValue _childValue;

        public event PropertyChangedEventHandler PropertyChanged;

        public object this[string key]
        {
            get
            {
                if (_childValue.IsUserEditable)
                {
                    if (Instance.Current.RegisteredEditors.TryGetUserInterface(key, _childValue.TypeDefinition.EditorName, out object userInterface))
                    {
                        return userInterface;
                    }

                    if (Instance.Current.RegisteredEditors.TryGetUserInterface(key, Instance.Current.GetTypeInfo(_childValue.TypeDefinition.ValueType).DefaultEditor, out object defaultTypeUserInterface))
                    {
                        return defaultTypeUserInterface;
                    }
                }
                else
                {
                    if (Instance.Current.RegisteredDisplays.TryGetUserInterface(key, _childValue.TypeDefinition.DisplayName, out object userInterface))
                    {
                        return userInterface;
                    }

                    if (Instance.Current.RegisteredDisplays.TryGetUserInterface(key, Instance.Current.GetTypeInfo(_childValue.TypeDefinition.ValueType).DefaultDisplay, out object defaultTypeUserInterface))
                    {
                        return defaultTypeUserInterface;
                    }
                }

                if (Instance.Current.RegisteredDisplays.TryGetUserInterface(key, "DefaultDisplay", out object defaultDisplay))
                {
                    return defaultDisplay;
                }

                return null;
            }
        }

        public void SetChildValue(OpenFlowValue childValue)
        {
            if (_childValue != null)
            {
                _childValue.PropertyChanged -= ChildValue_PropertyChanged;
            }

            _childValue = childValue;
            _childValue.PropertyChanged += ChildValue_PropertyChanged;
        }

        private void ChildValue_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(OpenFlowValue.IsUserEditable) or nameof(OpenFlowValue.TypeDefinition))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
            }
        }
    }
}
