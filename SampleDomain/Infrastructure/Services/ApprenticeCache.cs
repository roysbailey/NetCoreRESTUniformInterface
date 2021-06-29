using SampleDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleDomain.Infrastructure.Services
{
    public class ApprenticeCache : IApprenticeCache
    {
        private object cacheLock = new object();
        private IList<Apprentice> apprentices;
        public IList<Apprentice> Apprentices
        {
            get
            {
                lock (cacheLock)
                {
                    if (apprentices == null)
                    {
                        apprentices = new List<Apprentice>();
                        SeedData();
                    }
                    return apprentices;
                }
            }
        }

        public void SeedData()
        {
            apprentices.Add(new Apprentice { Id=1, FirstName="Roy", LastName="Bailey", DateOfBirth=new DateTime(1999,2,17), Email="rb@bobbins.com", PersonalDetailsConfirmedOn=null});
        }
    }
}
