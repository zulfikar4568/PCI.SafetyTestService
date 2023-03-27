using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace PCI.SafetyTestService.Entity
{
    public class SafetyTest
    {
        [Name("Step")]
        public string Step { get; set; }
        [Name("Test Type")]
        public string TestType { get; set; }
        [Name("Exp Result")]
        public string ExpResult { get; set; }
        [Name("Instr. Result")]
        public string InstrResult { get; set; }
        [Name("Final Result")]
        public string FinalResult { get; set; }
        [Name("Meter 1")]
        public string Meter1 { get; set; }
        [Name("Meter 2")]
        public string DataResult { get; set; }
        [Name("Meter 3")]
        public string Meter3 { get; set; }
        [Name("Meter 4")]
        public string Meter4 { get; set; }
        [Name("Meter 5")]
        public string Meter5 { get; set; }
        [Name("Timer")]
        public string Timer { get; set; }
        [Name("Station Name")]
        public string StationName { get; set; }
        [Name("User")]
        public string User { get; set; }
        [Name("Date")]
        public string Date { get; set; }
        [Name("Time")]
        public string Time { get; set; }
        [Name("DUT Model #")]
        public string DUTModel { get; set; }
        [Name("DUT Serial #")]
        public string Serial { get; set; }
        [Name("Instrument Model #")]
        public string InstrumentModel { get; set; }
        [Name("Instrument Serial #")]
        public string InstrumentSerial { get; set; }
        [Name("File Type")]
        public string FileType { get; set; }
        [Name("Calibration Due Date")]
        public string CalibrationDueDate { get; set; }
        [Name("Probe")]
        public string Probe { get; set; }
        [Name("Measuring Device")]
        public string MeasuringDevice { get; set; }
        [Name("PC File Name")]
        public string PCFileName { get; set; }
        [Name("Instrument File Name")]
        public string InstrumentFileName { get; set; }
        [Name("File #")]
        public string File { get; set; }
        [Name("Test Start Time")]
        public string TestStartTime { get; set; }

    }
}
