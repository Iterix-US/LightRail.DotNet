using System;
using System.Collections.Generic;
using System.Text;

namespace SeroGlint.DotNet.ElevatedAgent
{
    public class EaConfigurationResponse
    {
        public string ExecutablePath { get; private set; }
        public bool RunAsAdministrator { get; private set; }
    }
}
