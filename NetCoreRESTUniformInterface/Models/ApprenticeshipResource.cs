using Microsoft.AspNetCore.Mvc;
using NetCoreRESTUniformInterface.Infrastructure;
using SampleDomain;
using SampleDomain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreRESTUniformInterface.Models
{
    public class ApprenticeshipResource : Apprenticeship
    {
        public IEnumerable<Link> Links { get; private set; }

        public ApprenticeshipResource(Apprenticeship appr, IUrlHelper urlHelper)
        {
            Id = appr.Id;
            ApprenticeId = appr.ApprenticeId;
            Employer = appr.Employer;
            Provider = appr.Provider;
            StandardName = appr.StandardName;
            Level = appr.Level;
            StartDate = appr.StartDate;
            EndDate = appr.EndDate;
            ConfirmedSection1 = appr.ConfirmedSection1;
            ConfirmedSection2 = appr.ConfirmedSection2;
            ConfirmedSection3 = appr.ConfirmedSection3;
            ApprenticeshipConfirmedOn = appr.ApprenticeshipConfirmedOn;

            var links = new List<Link>();
            links.Add(new Link { Rel = "self", Href = urlHelper.RouteUrl(RouteNames.ApprenticeApprenticeship, new { appId = ApprenticeId, id = Id }, "HTTPS"), Type = "GET" });
            links.Add(new Link { Rel = "apprentice", Href = urlHelper.RouteUrl(RouteNames.Apprentice, new { Id = ApprenticeId }, "HTTPS"), Type = "GET" });
            if (!ConfirmedSection1)
                links.Add(new Link { Rel = RouteNames.ConfirmApprenticeshipSection1, Href = urlHelper.RouteUrl(RouteNames.ConfirmApprenticeshipSection1, new { appId = ApprenticeId, Id = Id }, "HTTPS"), Type = "POST" });
            if (!ConfirmedSection2)
                links.Add(new Link { Rel = RouteNames.ConfirmApprenticeshipSection2, Href = urlHelper.RouteUrl(RouteNames.ConfirmApprenticeshipSection2, new { appId = ApprenticeId, Id = Id }, "HTTPS"), Type = "POST" });
            if (!ConfirmedSection3)
                links.Add(new Link { Rel = RouteNames.ConfirmApprenticeshipSection3, Href = urlHelper.RouteUrl(RouteNames.ConfirmApprenticeshipSection3, new { appId = ApprenticeId, Id = Id }, "HTTPS"), Type = "POST" });
            if (ConfirmedSection1 && ConfirmedSection2 && ConfirmedSection3 && ApprenticeshipConfirmedOn == null)
                links.Add(new Link { Rel = RouteNames.ConfirmApprenticeship, Href = urlHelper.RouteUrl(RouteNames.ConfirmApprenticeship, new { appId = ApprenticeId, Id = Id }, "HTTPS"), Type = "POST" });

            Links = links;
        }
    }
}
