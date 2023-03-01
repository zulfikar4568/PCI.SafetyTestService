using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PCI.SafetyTestService.Config;
using PCI.SafetyTestService.Util;

namespace PCI.SafetyTestService.Util
{
    public interface IProcessFile
    {
        void CheckAndCreateDirectory(string sourceFolder);
        bool BackupTheFile(string sourceFiles, string destinationFiles);
        bool MoveTheFile(string sourceFiles, string destinationFiles);
    }
    public class ProcessFile  : IProcessFile
    {
        public void CheckAndCreateDirectory(string sourceFolder)
        {
            if (!Directory.Exists(sourceFolder))
            {
                bool status = Directory.CreateDirectory(sourceFolder).Exists;
                if (status) EventLogUtil.LogEvent($"Folder {sourceFolder} created successfully!", System.Diagnostics.EventLogEntryType.Information, 3);
            } else
            {
                EventLogUtil.LogEvent($"Folder {sourceFolder} alredy exists!", System.Diagnostics.EventLogEntryType.Information, 3);
            }
        }
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
        public bool MoveTheFile(string sourceFiles, string destinationFiles)
        {
            if (!File.Exists(sourceFiles))
            {
                EventLogUtil.LogEvent($"{sourceFiles} doesn't exists", System.Diagnostics.EventLogEntryType.Warning);
                return false;
            }

            try
            {
                File.Move(sourceFiles, destinationFiles);
            }
            catch (Exception ex)
            {
                EventLogUtil.LogErrorEvent(AppSettings.AssemblyName == ex.Source ? MethodBase.GetCurrentMethod().Name : MethodBase.GetCurrentMethod().Name + "." + ex.Source, ex);
            }

            return true;
        }
    }
}
