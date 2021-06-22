using System;
using System.Collections.Generic;
using System.Text;

namespace SampleDomain.Infrastructure
{
    public class Link
    {
        public string Rel { get; set; }
        public string Href { get; set; }
        public string Type { get; set; } = "GET";
    }
}
