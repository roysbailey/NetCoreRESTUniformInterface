using System;
using System.Collections.Generic;
using System.Text;

namespace SampleDomain
{
    public class Apprenticeship
    {
        public int Id { get; set; }
        public int ApprenticeId { get; set; }
        public string StandardName { get; set; }
        public int Level { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Employer { get; set; }
        public string Provider { get; set; }
        public bool ConfirmedSection1 { get; set; }
        public bool ConfirmedSection2 { get; set; }
        public bool ConfirmedSection3 { get; set; }
        public DateTime? ApprenticeshipConfirmedOn { get; set; }
    }
}
