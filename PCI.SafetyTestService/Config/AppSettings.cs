using Camstar.WCF.ObjectStack;
using PCI.SafetyTestService.Util;
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
        public static string AssemblyName
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            }
        }

        #region SOURCE_FOLDER
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
        public static string FailedFolderSafetyTest
        {
            get
            {
                return ConfigurationManager.AppSettings["FailedFolderSafetyTest"];
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
        public static string FailedFolderDailyCheck
        {
            get
            {
                return ConfigurationManager.AppSettings["FailedFolderDailyCheck"];
            }
        }
        public static string DailyCheckFileName
        {
            get
            {
                return ConfigurationManager.AppSettings["DailyCheckFileName"];
            }
        }

        #endregion

        #region TIME
        public static TimeSpan UTCOffset
        {
            get
            {
                string sUTCOffset = ConfigurationManager.AppSettings["UTCOffset"];
                string[] aUTCOffset = sUTCOffset.Split(':');
                return new TimeSpan(Int32.Parse(aUTCOffset[0]), Int32.Parse(aUTCOffset[1]), Int32.Parse(aUTCOffset[2]));
            }
        }
        public static ulong TimerPollingInterval
        {
            get
            {
                return Convert.ToUInt64(ConfigurationManager.AppSettings["TimerPollingInterval"]);
            }
        }
        #endregion

        #region CONFIG ACCOUNT ExCore
        private static string ExCoreHost
        {
            get
            {
                return ConfigurationManager.AppSettings["ExCoreHost"];
            }
        }
        private static string ExCorePort
        {
            get
            {
                return ConfigurationManager.AppSettings["ExCorePort"];
            }
        }
        private static string ExCoreUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["ExCoreUsername"];
            }
        }
        private static string ExCorePassword
        {
            get
            {

                Simple3Des oSimple3Des = new Simple3Des(ConfigurationManager.AppSettings["ExCorePasswordKey"]);
                return oSimple3Des.DecryptData(ConfigurationManager.AppSettings["ExCorePassword"]);
            }
        }
        private static UserProfile _ExCoreUserProfile = null;
        public static UserProfile ExCoreUserProfile
        {
            get
            {
                if (_ExCoreUserProfile == null)
                {
                    _ExCoreUserProfile = new UserProfile(ExCoreUsername, ExCorePassword, UTCOffset);
                }
                if (_ExCoreUserProfile.Name != ExCoreUsername || _ExCoreUserProfile.Password.Value != ExCorePassword)
                {
                    _ExCoreUserProfile = new UserProfile(ExCoreUsername, ExCorePassword, UTCOffset);
                }
                return _ExCoreUserProfile;
            }
        }
        #endregion

        #region MODELLING METADATA
        public static string UserDataCollectionSafetyTestName
        {
            get
            {
                return ConfigurationManager.AppSettings["UserDataCollectionSafetyTestName"];
            }
        }
        public static string UserDataCollectionSafetyTestRevision
        {
            get
            {
                return ConfigurationManager.AppSettings["UserDataCollectionSafetyTestRevision"];
            }
        }
        public static string ResourceName
        {
            get
            {
                return ConfigurationManager.AppSettings["ResourceName"];
            }
        }
        public static string UserDataCollectionDailyCheckName
        {
            get
            {
                return ConfigurationManager.AppSettings["UserDataCollectionDailyCheckName"];
            }
        }
        public static string UserDataCollectionDailyCheckRevision
        {
            get
            {
                return ConfigurationManager.AppSettings["UserDataCollectionDailyCheckRevision"];
            }
        }
        #endregion

        #region CONFIG FIELD SAFETY TEST
        public static int StepST
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["StepST"]);
            }
        }
        public static int Value
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["Value"]);
            }
        }
        #endregion

        #region CONFIG FIELD DAILY CHECK
        public static int StepDC
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["StepDC"]);
            }
        }
        public static int TestType
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["TestType"]);
            }
        }
        public static int ExpResult
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["ExpResult"]);
            }
        }
        public static int InstrResult
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["InstrResult"]);
            }
        }
        public static int FinalResult
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["FinalResult"]);
            }
        }
        public static int Meter1
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["Meter1"]);
            }
        }
        public static int DataResult
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["DataResult"]);
            }
        }
        public static int Meter3
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["Meter3"]);
            }
        }
        public static int Meter4
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["Meter4"]);
            }
        }
        public static int Meter5
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["Meter5"]);
            }
        }
        public static int Timer
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["Timer"]);
            }
        }
        public static int StationName
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["StationName"]);
            }
        }
        public static int User
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["User"]);
            }
        }
        public static int Date
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["Date"]);
            }
        }
        public static int Time
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["Time"]);
            }
        }
        public static int DUTModel
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["DUTModel"]);
            }
        }
        public static int Serial
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["Serial"]);
            }
        }
        public static int InstrumentModel
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["InstrumentModel"]);
            }
        }
        public static int InstrumentSerial
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["InstrumentSerial"]);
            }
        }
        public static int FileType
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["FileType"]);
            }
        }
        public static int CalibrationDueDate
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["CalibrationDueDate"]);
            }
        }
        public static int Probe
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["Probe"]);
            }
        }
        public static int MeasuringDevice
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["MeasuringDevice"]);
            }
        }
        public static int PCFileName
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["PCFileName"]);
            }
        }
        public static int InstrumentFileName
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["InstrumentFileName"]);
            }
        }
        public static int File
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["File"]);
            }
        }
        public static int TestStartTime
        {
            get
            {
                return Int32.Parse(ConfigurationManager.AppSettings["TestStartTime"]);
            }
        }
        #endregion
    }
}