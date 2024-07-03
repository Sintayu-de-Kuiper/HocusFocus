using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using HocusFocus.Core;
using System.Diagnostics;

namespace HocusFocus.Service
{
    public class ProcessWatcher
    {
        private ManagementEventWatcher watcher;
        private ConfigurationManager configManager;

        public ProcessWatcher(ConfigurationManager configManager)
        {
            this.configManager = configManager;
            StartWatching();
        }

        public void StartWatching()
        {
            try
            {
                string query = "SELECT * FROM __InstanceCreationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_Process'";
                watcher = new ManagementEventWatcher(new WqlEventQuery(query));
                watcher.EventArrived += new EventArrivedEventHandler(ProcessStarted);
                watcher.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting up process watcher: {ex.Message}");
            }
        }

        private void ProcessStarted(object sender, EventArrivedEventArgs e)
        {
            try
            {
                var newProcess = (ManagementBaseObject)e.NewEvent["TargetInstance"];
                string processName = newProcess["Name"].ToString();

                if (configManager.LoadConfiguration().BlockedApps.Contains(processName))
                {
                    int processId = Convert.ToInt32(newProcess["ProcessId"]);
                    Process.GetProcessById(processId).Kill();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling process start: {ex.Message}");
            }
        }

        public void StopWatching()
        {
            watcher.Stop();
            watcher.Dispose();
        }
    }
}
