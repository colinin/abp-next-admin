using System;
using System.Collections.Generic;

namespace LINGYUN.Abp.AspNetCore.Mvc.Results
{
    public class TestResultObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public int Integer { get; set; }
        public double Double { get; set; }
        public Dictionary<string , string> Properties { get; set; }
        public TestResultObject() { }
    }
}
