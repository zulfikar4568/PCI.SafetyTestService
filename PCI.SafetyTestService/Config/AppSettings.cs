using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.Config
{
    public static class AppSettings
    {
        public static string AssemblyName { get; set; }
        public static string SourceFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["SourceFolder"];
            }
        }
        public static string TargetFolder
        {
            get
            {
                return ConfigurationManager.AppSettings["TargetFolder"];
            }
        }
    }
}