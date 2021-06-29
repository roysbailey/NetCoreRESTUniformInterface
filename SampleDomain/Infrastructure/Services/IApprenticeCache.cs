using SampleDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleDomain.Infrastructure.Services
{
    public interface IApprenticeCache
    {
        IList<Apprentice> Apprentices { get; }
        void SeedData() { } 
    }
}
