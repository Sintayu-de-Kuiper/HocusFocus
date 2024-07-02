using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HocusFocus.Core
{
    public class AppManager
    {
        private const string RegistryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        public List<AppInfo> GetAllInstalledApps()
        {
            var apps = new List<AppInfo>();
            using var key = Registry.LocalMachine.OpenSubKey(RegistryKeyPath);
            if (key != null)
            {
                foreach (var subkeyName in key.GetSubKeyNames())
                {
                    var appInfo = GetAppInfoFromSubKey(key, subkeyName);
                    if (appInfo != null)
                    {
                        apps.Add(appInfo);
                    }
                }
            }
            return apps;
        }

        private AppInfo GetAppInfoFromSubKey(RegistryKey parentKey, string subKeyName)
        {
            using var subKey = parentKey.OpenSubKey(subKeyName);
            if (subKey == null) return null;

            var displayName = subKey.GetValue("DisplayName") as string;
            if (string.IsNullOrEmpty(displayName))
                return null;

            var execPath = subKey.GetValue("DisplayIcon") as string;
            return new AppInfo { Name = displayName, ExecutablePath = execPath };
        }
    }
}
