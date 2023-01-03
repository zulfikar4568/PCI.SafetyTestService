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
        public static string SourceFolderSafetyTest
        {
            get
            {
                return ConfigurationManager.AppSettings["SourceFolderSafetyTest"];
            }
        }
        public static string TargetFolderSafetyTest
        {
            get
            {
                return ConfigurationManager.AppSettings["TargetFolderSafetyTest"];
            }
        }

        public static string SourceFolderDailyCheck
        {
            get
            {
                return ConfigurationManager.AppSettings["SourceFolderDailyCheck"];
            }
        }
        public static string TargetFolderDailyCheck
        {
            get
            {
                return ConfigurationManager.AppSettings["TargetFolderDailyCheck"];
            }
        }
    }
}