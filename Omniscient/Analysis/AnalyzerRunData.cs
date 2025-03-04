﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscient
{
    public class AnalyzerRunData
    {
        public Declaration Declaration { get; set; }
        public Event Event { get; set; }
        public Dictionary<string, CustomParameter> CustomParameters { get; set; }
        public AnalyzerReport Report { get; set; }
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> ImportedReports { get; set; }
        public AnalyzerRunData(Event eve, Dictionary<string, CustomParameter> parameters) 
        {
            Declaration = null;
            Event = eve;
            CustomParameters = parameters;
            Report = null;
            ImportedReports = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
        }
    }
}
