using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using PCI.SafetyTestService.Config;

namespace PCI.SafetyTestService.Entity
{
    public class SafetyTest
    {
        [Index(0)]
        public string Step { get; set; }
        [Index(1)]
        public string Value { get; set; }
    }

    public sealed class SafetyTestMap : ClassMap<SafetyTest>
    {
        public SafetyTestMap()
        {
            Map(m => m.Step).Index(AppSettings.StepST);
            Map(m => m.Value).Index(AppSettings.Value);
        }
    }
}
