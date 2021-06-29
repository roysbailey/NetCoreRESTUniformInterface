using SampleDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleDomain.Infrastructure.Services
{
    public class ApprenticeshipsCache : IApprenticeshipsCache
    {
        private object cacheLock = new object();
        private IList<Apprenticeship> apprenticeships;
        public IList<Apprenticeship> Apprenticeships
        {
            get
            {
                lock (cacheLock)
                {
                    if (apprenticeships == null)
                    {
                        apprenticeships = new List<Apprenticeship>();
                        SeedData();
                    }
                    return apprenticeships;
                }
            }
        }
        public void SeedData()
        {
            apprenticeships.Add(new Apprenticeship { Id = 1, ApprenticeId = 1, Employer = "Silvertouch Technology Ltd", Provider = "University of Birmingham", StandardName = "Digital Tech Solution Professional", Level = 6, StartDate = new DateTime(2021, 07, 03), EndDate = new DateTime(2025, 07, 21), ApprenticeshipConfirmedOn = null, ConfirmedSection1 = false, ConfirmedSection2 = false, ConfirmedSection3 = false });
        }
    }
}
