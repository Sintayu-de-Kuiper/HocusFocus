using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HocusFocus.Core
{
    public class AppInfo(string name, string executablePath, bool isRunning = false)
    {
        public string Name { get; set; } = name;
        public string ExecutablePath { get; set; } = executablePath;
        public bool IsRunning { get; set; } = isRunning;
    }
}
