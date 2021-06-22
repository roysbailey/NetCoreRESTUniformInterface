using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreRESTUniformInterface.Infrastructure;
using NetCoreRESTUniformInterface.Infrastructure.Services;
using NetCoreRESTUniformInterface.Models;
using SampleDomain;
using SampleDomain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreRESTUniformInterface.Controllers
{
    [Route("apprentices/{appId}/apprenticeships")]
    [ApiController]
    public class ApprenticeApprenticeshipsController : ControllerBase
    {
        private readonly ILogger<ApprenticeApprenticeshipsController> _logger;

        public ApprenticeApprenticeshipsController(ILogger<ApprenticeApprenticeshipsController> logger)
        {
            _logger = logger;
        }

        [HttpPost("", Name = RouteNames.ApprenticeApprenticeships)]
        public IActionResult Post(int appId, Apprenticeship apprenticeship)
        {
            if (!ApprenticeCache.Apprentices.Any(a => a.Id == appId))
                return BadRequest();

            apprenticeship.Id = ApprenticeshipsCache.Apprenticeships.Max(a => a.Id) + 1;
            apprenticeship.ApprenticeId = appId;
            ApprenticeshipsCache.Apprenticeships.Add(apprenticeship);

            return new CreatedResult(Url.RouteUrl(RouteNames.ApprenticeApprenticeship, new { appId = appId, id = apprenticeship.Id }, "HTTPS"), new ApprenticeshipResource(apprenticeship, Url));
        }

        [HttpGet("", Name = RouteNames.ApprenticeApprenticeships)]
        public IActionResult GetApprenticeships(int appId)
        {
            if (!ApprenticeCache.Apprentices.Any(a => a.Id == appId))
                return NotFound();

            var apprsR = ApprenticeshipsCache.Apprenticeships
                .Where(a => a.ApprenticeId == appId)
                .Select(a => new ApprenticeshipResource(a, Url));

            var wrappedResponse = new
            {
                Items = apprsR,
                Links = new List<Link> { new Link { Rel = "self", Href = Url.RouteUrl(RouteNames.ApprenticeApprenticeships, new { appId = appId }, "HTTPS"), Type = "GET" } }
            };
            return Ok(wrappedResponse);
        }

        [HttpGet("{Id}", Name = RouteNames.ApprenticeApprenticeship)]
        public IActionResult GetApprenticeship(int appId, int id)
        {
            var appr = ApprenticeshipsCache.Apprenticeships.Where(a => a.Id == id && a.ApprenticeId == appId).FirstOrDefault();
            if (appr == default(Apprenticeship))
                return NotFound();

            var apprR = new ApprenticeshipResource(appr, Url);
            return Ok(apprR);
        }

        [HttpPost("{Id}/confirm-section-1", Name = RouteNames.ConfirmApprenticeshipSection1)]
        public IActionResult ConfirmSection1Post(int appId, int id)
        {
            var apprenticeship = ApprenticeshipsCache.Apprenticeships.FirstOrDefault(a => a.Id == id && a.ApprenticeId == appId);
            if (apprenticeship == default(Apprenticeship))
                return NotFound();

            apprenticeship.ConfirmedSection1 = true;
            return Ok(new ApprenticeshipResource(apprenticeship, Url));
        }

        [HttpPost("{Id}/confirm-section-2", Name = RouteNames.ConfirmApprenticeshipSection2)]
        public IActionResult ConfirmSection2Post(int appId, int id)
        {
            var apprenticeship = ApprenticeshipsCache.Apprenticeships.FirstOrDefault(a => a.Id == id && a.ApprenticeId == appId);
            if (apprenticeship == default(Apprenticeship))
                return NotFound();

            apprenticeship.ConfirmedSection2 = true;
            return Ok(new ApprenticeshipResource(apprenticeship, Url));
        }

        [HttpPost("{Id}/confirm-section-3", Name = RouteNames.ConfirmApprenticeshipSection3)]
        public IActionResult ConfirmSection3Post(int appId, int id)
        {
            var apprenticeship = ApprenticeshipsCache.Apprenticeships.FirstOrDefault(a => a.Id == id && a.ApprenticeId == appId);
            if (apprenticeship == default(Apprenticeship))
                return NotFound();

            apprenticeship.ConfirmedSection3 = true;
            return Ok(new ApprenticeshipResource(apprenticeship, Url));
        }

        [HttpPost("{Id}/confirm", Name = RouteNames.ConfirmApprenticeship)]
        public IActionResult ConfirmApprenticeshipPost(int appId, int id)
        {
            var apprenticeship = ApprenticeshipsCache.Apprenticeships.FirstOrDefault(a => a.Id == id && a.ApprenticeId == appId);
            if (apprenticeship == default(Apprenticeship))
                return NotFound();

            if (!apprenticeship.ConfirmedSection1 || !apprenticeship.ConfirmedSection2 || !apprenticeship.ConfirmedSection3)
                return Conflict("Sections must be confirmed BEFORE confirming the apprenticeship");

            apprenticeship.ApprenticeshipConfirmedOn = DateTime.Now;
            return Ok(new ApprenticeshipResource(apprenticeship, Url));
        }
    }
}
