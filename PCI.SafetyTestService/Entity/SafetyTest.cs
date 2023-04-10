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
        [Index(0)]
        public string Step { get; set; }
        [Index(1)]
        public string Value { get; set; }
        

    }
}
