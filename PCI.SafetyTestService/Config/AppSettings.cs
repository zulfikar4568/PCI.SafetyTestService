﻿using Camstar.WCF.ObjectStack;
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
        public static string AssemblyName { get; set; }

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
    }
}