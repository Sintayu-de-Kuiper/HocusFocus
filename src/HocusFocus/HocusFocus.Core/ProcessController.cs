using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace HocusFocus.Core
{
    public class ProcessController
    {
        private ConfigurationManager _configManager;

        public ProcessController(ConfigurationManager configManager)
        {
            _configManager = configManager;
        }

        // Method to enforce the blacklist by terminating blocked apps
        public void EnforceBlacklist()
        {
            var config = _configManager.LoadConfiguration();
            var blockedApps = config.BlockedApps;

            // Retrieve all running processes
            Process[] runningProcesses = Process.GetProcesses();

            foreach (var process in runningProcesses)
            {
                try
                {
                    // Check if the process name (without extension) is on the blacklist
                    if (blockedApps.Contains(process.ProcessName))
                    {
                        Console.WriteLine($"Terminating blacklisted process: {process.ProcessName}");
                        process.Kill();
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that might occur when attempting to kill the process
                    Console.WriteLine($"Error terminating process {process.ProcessName}: {ex.Message}");
                }
            }
        }
    }
}
