namespace OpenFlow_Core.Management
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class RegisteredUIs
    {
        private readonly Dictionary<string, Dictionary<string, object>> registeredUIs = new();

        public bool AddUI<T>(string name, T ui)
        {
            Debug.WriteLine($"Adding UI {name}");
            if (registeredUIs.TryGetValue(name, out Dictionary<string, object> reg))
            {
                if (reg.ContainsKey(typeof(T).FullName))
                {
                    return false;
                }

                reg.Add(typeof(T).FullName, ui);
            }
            else
            {
                registeredUIs.Add(name, new Dictionary<string, object>() { { typeof(T).FullName, ui } });
            }

            return true;
        }

        public bool TryGetUI<T>(string name, out T ui)
        {
            if (registeredUIs.TryGetValue(name, out Dictionary<string, object> reg) && reg.TryGetValue(typeof(T).FullName, out object uiObj))
            {
                ui = (T)uiObj;
                return true;
            }

            ui = default;
            return false;
        }

        public bool TryGetUIs(string name, out Dictionary<string, object> uis)
        {
            if (name != null && registeredUIs.TryGetValue(name, out Dictionary<string, object> myUIs))
            {
                uis = myUIs;
                return true;
            }

            uis = default;
            return false;
        }
    }
}
