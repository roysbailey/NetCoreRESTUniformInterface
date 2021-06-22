using SampleDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreRESTUniformInterface.Infrastructure.Services
{
    public static class ApprenticeCache
    {
        private static object cacheLock = new object();
        private static IList<Apprentice> apprentices;
        public static IList<Apprentice> Apprentices
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

        public static void SeedData()
        {
            apprentices.Add(new Apprentice { Id=1, FirstName="Roy", LastName="Bailey", DateOfBirth=new DateTime(1999,2,17), Email="rb@bobbins.com", PersonalDetailsConfirmedOn=null});
        }
    }
}
