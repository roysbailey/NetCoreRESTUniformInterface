using SampleDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreRESTUniformInterface.Infrastructure.Services
{
    public static class ApprenticeshipsCache
    {
        private static object cacheLock = new object();
        private static IList<Apprenticeship> apprenticeships;
        public static IList<Apprenticeship> Apprenticeships
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
        public static void SeedData()
        {
            apprenticeships.Add(new Apprenticeship { Id = 1, ApprenticeId = 1, Employer = "Silvertouch Technology Ltd", Provider = "University of Birmingham", StandardName = "Digital Tech Solution Professional", Level = 6, StartDate = new DateTime(2021, 07, 03), EndDate = new DateTime(2025, 07, 21), ApprenticeshipConfirmedOn = null, ConfirmedSection1 = false, ConfirmedSection2 = false, ConfirmedSection3 = false });
        }
    }
}
