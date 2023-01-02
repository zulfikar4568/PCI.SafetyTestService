using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using PCI.SafetyTestService.Config;

namespace PCI.SafetyTestService.Util
{
    public class EventLogUtil
    {
        public static EventLog EventLogRef;
        public static int TraceLevel = int.Parse(ConfigurationManager.AppSettings["TraceLevel"]);
        public static string LastLog;
        public static string LastLogError;

        private static string sEventSource = AppSettings.AssemblyName + "Source";
        private static string sEventLog = AppSettings.AssemblyName + "Log";
        public static void InitEventLog()
        {
            EventLogUtil.EventLogRef = new EventLog();

            foreach (EventLog oEventLog in EventLog.GetEventLogs())
            {
                if (sEventLog.Substring(0, 3).ToUpper() == oEventLog.Log.Substring(0, 3).ToUpper())
                {
                    sEventLog = oEventLog.Log;
                    break;
                }
            }
            if (!EventLog.SourceExists(sEventSource))
            {
                EventLog.CreateEventSource(sEventSource, sEventLog);
            }
        }
        public static void LogEvent(string EventMessage, EventLogEntryType EventType = EventLogEntryType.Information, int _EventId = 0)
        {
            if (EventLogUtil.EventLogRef is null) InitEventLog();
            EventLogUtil.EventLogRef.Source = sEventSource;
            EventLogUtil.EventLogRef.Log = sEventLog;
            if (_EventId <= TraceLevel)
            {
                EventLogUtil.EventLogRef.WriteEntry(EventMessage, EventType, _EventId);
                LastLog = EventMessage;
            }
        }
        public static void LogErrorEvent(string Location, Exception Ex, int _Event_Id = 0)
        {
            if (EventLogUtil.EventLogRef is null) InitEventLog();
            EventLogUtil.EventLogRef.Source = sEventSource;
            EventLogUtil.EventLogRef.Log = sEventLog;
            LastLogError = "Error Location: " + Location + "\r\n" + "Error Source: " + Ex.Source + "\r\n" + "Error Message: " + Ex.Message;
            EventLogUtil.EventLogRef.WriteEntry(LastLogError, EventLogEntryType.Error, _Event_Id);
        }
        public static void LogErrorEvent(string Location, string ExceptionMsg, int _Event_Id = 0)
        {
            if (EventLogUtil.EventLogRef is null) InitEventLog();
            EventLogUtil.EventLogRef.Source = sEventSource;
            EventLogUtil.EventLogRef.Log = sEventLog;
            LastLogError = "Error Location: " + Location + "\r\n" + "Error Source: " + Location + "\r\n" + "Error Message: " + ExceptionMsg;
            EventLogUtil.EventLogRef.WriteEntry(LastLogError, EventLogEntryType.Error, _Event_Id);
        }
    }
}
