﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PCI.SafetyTestService.Config;
using PCI.SafetyTestService.Util;

namespace PCI.SafetyTestService
{
    public class ProcessFile
    {
        public bool BackupTheFile(string sourceFiles, string destinationFiles)
        {
            if (!File.Exists(sourceFiles))
            {
                EventLogUtil.LogEvent($"{sourceFiles} doesn't exists", System.Diagnostics.EventLogEntryType.Warning);
                return false;
            }

            try
            {
                File.Copy(sourceFiles, destinationFiles);
            }
            catch (Exception ex)
            {
                EventLogUtil.LogErrorEvent(AppSettings.AssemblyName == ex.Source ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "." + ex.Source, ex);
            }

            return true;
        }
    }
}
