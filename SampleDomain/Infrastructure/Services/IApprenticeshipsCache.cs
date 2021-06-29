using SampleDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleDomain.Infrastructure.Services
{
    public interface IApprenticeshipsCache
    {
        public IList<Apprenticeship> Apprenticeships { get; }
        public void SeedData();
    }
}
