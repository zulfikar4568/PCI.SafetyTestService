using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using PCI.SafetyTestService.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCI.SafetyTestService.Entity
{
    public class DailyCheck
    {
        public string Step { get; set; }
        public string TestType { get; set; }
        public string ExpResult { get; set; }
        public string InstrResult { get; set; }
        public string FinalResult { get; set; }
        public string Meter1 { get; set; }
        public string DataResult { get; set; }
        public string Meter3 { get; set; }
        public string Meter4 { get; set; }
        public string Meter5 { get; set; }
        public string Timer { get; set; }
        public string StationName { get; set; }
        public string User { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string DUTModel { get; set; }
        public string Serial { get; set; }
        public string InstrumentModel { get; set; }
        public string InstrumentSerial { get; set; }
        public string FileType { get; set; }
        public string CalibrationDueDate { get; set; }
        public string Probe { get; set; }
        public string MeasuringDevice { get; set; }
        public string PCFileName { get; set; }
        public string InstrumentFileName { get; set; }
        public string File { get; set; }
        public string TestStartTime { get; set; }
    }

    public sealed class DailyCheckMap : ClassMap<DailyCheck>
    {
        public DailyCheckMap()
        {
            Map(m => m.Step ).Index(AppSettings.StepDC);
            Map(m => m.TestType ).Index(AppSettings.TestType);
            Map(m => m.ExpResult ).Index(AppSettings.ExpResult); 
            Map(m => m.InstrResult ).Index(AppSettings.InstrResult);
            Map(m => m.FinalResult ).Index(AppSettings.FinalResult);
            Map(m => m.Meter1 ).Index(AppSettings.Meter1);
            Map(m => m.DataResult ).Index(AppSettings.DataResult);
            Map(m => m.Meter3 ).Index(AppSettings.Meter3);
            Map(m => m.Meter4 ).Index(AppSettings.Meter4);
            Map(m => m.Meter5 ).Index(AppSettings.Meter5);
            Map(m => m.Timer ).Index(AppSettings.Timer);
            Map(m => m.StationName ).Index(AppSettings.StationName);
            Map(m => m.User ).Index(AppSettings.User); 
            Map(m => m.Date ).Index(AppSettings.Date);
            Map(m => m.Time ).Index(AppSettings.Time);
            Map(m => m.DUTModel ).Index(AppSettings.DUTModel);
            Map(m => m.Serial ).Index(AppSettings.Serial);
            Map(m => m.InstrumentModel ).Index(AppSettings.InstrumentModel);
            Map(m => m.InstrumentSerial ).Index(AppSettings.InstrumentSerial);
            Map(m => m.FileType ).Index(AppSettings.FileType);
            Map(m => m.CalibrationDueDate ).Index(AppSettings.CalibrationDueDate);
            Map(m => m.Probe ).Index(AppSettings.Probe);
            Map(m => m.MeasuringDevice ).Index(AppSettings.MeasuringDevice);
            Map(m => m.PCFileName ).Index(AppSettings.PCFileName);
            Map(m => m.InstrumentFileName ).Index(AppSettings.InstrumentFileName);
            Map(m => m.File ).Index(AppSettings.File);
            Map(m => m.TestStartTime).Index(AppSettings.TestStartTime);
        }
    }
}
