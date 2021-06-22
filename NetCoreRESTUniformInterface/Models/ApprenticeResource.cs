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
    public class ApprenticeResource : Apprentice
    {
        public IEnumerable<Link> Links { get; private set; }

        public ApprenticeResource(Apprentice app, IUrlHelper urlHelper)
        {
            Id = app.Id;
            FirstName = app.FirstName;
            LastName = app.LastName;
            DateOfBirth = app.DateOfBirth;
            Email = app.Email;
            PersonalDetailsConfirmedOn = app.PersonalDetailsConfirmedOn;

            var links = new List<Link>();
            links.Add(new Link { Rel = "self", Href = urlHelper.RouteUrl(RouteNames.Apprentice, new { Id = Id }, "HTTPS"), Type = "GET" });
            if (!PersonalDetailsConfirmedOn.HasValue)
                links.Add(new Link { Rel = RouteNames.ConfirmApprenticePersonalDetails, Href = urlHelper.RouteUrl(RouteNames.ConfirmApprenticePersonalDetails, new { Id = Id }, "HTTPS"), Type = "POST" });
            else
                links.Add(new Link { Rel = RouteNames.ApprenticeApprenticeships, Href = urlHelper.RouteUrl(RouteNames.ApprenticeApprenticeships, new { appId = Id }, "HTTPS"), Type = "GET" });

            Links = links;
        }
    }
}
