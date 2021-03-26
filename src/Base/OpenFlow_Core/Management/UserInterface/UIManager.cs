using OpenFlow_Core.Management;
using OpenFlow_PluginFramework.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFlow_Core.Management.UserInterface
{
    public class UIManager
    {
        private OpenFlowValue _childValue;
        private readonly Dictionary<string, ObservableObject> _userInterfacePerType = new();

        public object this[string key]
        {
            get
            {
                if (!_userInterfacePerType.ContainsKey(key))
                {
                    _userInterfacePerType.Add(key, new ObservableObject(GetUIOfType(key)));
                }

                return _userInterfacePerType[key];
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
            RefreshUserInterfaces();
        }

        private void ChildValue_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(OpenFlowValue.IsUserEditable) or nameof(OpenFlowValue.TypeDefinition))
            {
                RefreshUserInterfaces();
            }
        }

        private void RefreshUserInterfaces()
        {
            foreach (KeyValuePair<string, ObservableObject> observable in _userInterfacePerType)
            {
                observable.Value.Observable = GetUIOfType(observable.Key);
            }
        }


        private object GetUIOfType(string AQNOfType)
        {
            if (_childValue.TypeDefinition is not null)
            {
                if (_childValue.IsUserEditable)
                {
                    if (Instance.Current.RegisteredEditors.TryGetUserInterface(AQNOfType, _childValue.TypeDefinition.EditorName, out object userInterface))
                    {
                        return userInterface;
                    }

                    if (Instance.Current.RegisteredEditors.TryGetUserInterface(AQNOfType, Instance.Current.GetTypeInfo(_childValue.TypeDefinition.ValueType).DefaultEditor, out object defaultTypeUserInterface))
                    {
                        return defaultTypeUserInterface;
                    }
                }
                else
                {
                    if (Instance.Current.RegisteredDisplays.TryGetUserInterface(AQNOfType, _childValue.TypeDefinition.DisplayName, out object userInterface))
                    {
                        return userInterface;
                    }

                    if (Instance.Current.RegisteredDisplays.TryGetUserInterface(AQNOfType, Instance.Current.GetTypeInfo(_childValue.TypeDefinition.ValueType).DefaultDisplay, out object defaultTypeUserInterface))
                    {
                        return defaultTypeUserInterface;
                    }
                }
            }

            if (Instance.Current.RegisteredDisplays.TryGetUserInterface(AQNOfType, "DefaultDisplay", out object defaultDisplay))
            {
                return defaultDisplay;
            }

            return null;
        }

    }
}
