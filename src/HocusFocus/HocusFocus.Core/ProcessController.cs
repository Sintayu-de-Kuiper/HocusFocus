using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Threading;

namespace HocusFocus.Core
{
    public class ProcessController(ConfigurationManager configManager)
    {
        private ConfigurationManager _configManager = configManager;
        private CancellationTokenSource _cancellationTokenSource;

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

        // Method to start monitoring processes asynchronously
        public void StartMonitoring()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Task.Run(async () => await MonitorProcesses(_cancellationTokenSource.Token));
        }

        // Asynchronous loop checking for blacklisted applications
        private async Task MonitorProcesses(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    EnforceBlacklist();
                    await Task.Delay(1000, cancellationToken);  // Delay for 1 second (adjust as needed)
                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Monitoring has been stopped.");
            }
        }

        // Method to stop monitoring
        public void StopMonitoring()
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}
