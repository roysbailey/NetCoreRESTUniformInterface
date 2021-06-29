using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreRESTUniformInterface.Infrastructure;
using NetCoreRESTUniformInterface.Models;
using SampleDomain;
using SampleDomain.Infrastructure;
using SampleDomain.Infrastructure.Services;
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
        private readonly IApprenticeCache _apprenticeCache;
        private readonly IApprenticeshipsCache _apprenticeshipsCache;

        public ApprenticeApprenticeshipsController(ILogger<ApprenticeApprenticeshipsController> logger, IApprenticeCache apprenticeCache, IApprenticeshipsCache apprenticeshipsCache)
        {
            _logger = logger;
            _apprenticeCache = apprenticeCache;
            _apprenticeshipsCache = apprenticeshipsCache;
        }

        [HttpPost("", Name = RouteNames.ApprenticeApprenticeships)]
        public IActionResult Post(int appId, Apprenticeship apprenticeship)
        {
            if (_apprenticeshipsCache.Apprenticeships.Any(a => a.Id == apprenticeship.Id))
            {
                ModelState.AddModelError("Id", $"The Id of {apprenticeship.Id} is already in use.  Dont not include Id in your request, and Id will be allocated on creation");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            apprenticeship.Id = _apprenticeshipsCache.Apprenticeships.Max(a => a.Id) + 1;
            apprenticeship.ApprenticeId = appId;
            _apprenticeshipsCache.Apprenticeships.Add(apprenticeship);

            return new CreatedResult(Url.RouteUrl(RouteNames.ApprenticeApprenticeship, new { appId = appId, id = apprenticeship.Id }, "HTTPS"), new ApprenticeshipResource(apprenticeship, Url));
        }

        [HttpGet("", Name = RouteNames.ApprenticeApprenticeships)]
        public IActionResult GetApprenticeships(int appId)
        {
            if (!_apprenticeCache.Apprentices.Any(a => a.Id == appId))
                return NotFound();

            var apprsR = _apprenticeshipsCache.Apprenticeships
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
            var appr = _apprenticeshipsCache.Apprenticeships.Where(a => a.Id == id && a.ApprenticeId == appId).FirstOrDefault();
            if (appr == default(Apprenticeship))
                return NotFound();

            var apprR = new ApprenticeshipResource(appr, Url);
            return Ok(apprR);
        }

        [HttpPatch("{Id}", Name = RouteNames.ApprenticeApprenticeship)]
        public IActionResult PatchApprenticeship(int id, [FromBody] JsonPatchDocument<Apprenticeship> apprenticeshipPatch)
        {
            var app = _apprenticeshipsCache.Apprenticeships.FirstOrDefault(a => a.Id == id);
            if (app == default(Apprenticeship))
                return NotFound();
            var tmpApp = new Apprenticeship(app);

            // Apply the PATCH instruction to the entity
            try
            {
                apprenticeshipPatch.ApplyTo(tmpApp);
                TryValidateModel(tmpApp);
            }
            catch (JsonPatchException jpe)
            {
                ModelState.AddModelError("error", jpe.Message);
            }
            catch (ArgumentNullException ane)
            {
                ModelState.AddModelError("error", ane.Message);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            app = tmpApp;
            var appR = new ApprenticeshipResource(app, Url);
            return Ok(appR);
        }

        [HttpPost("{Id}/confirm-section-1", Name = RouteNames.ConfirmApprenticeshipSection1)]
        public IActionResult ConfirmSection1Post(int appId, int id)
        {
            var apprenticeship = _apprenticeshipsCache.Apprenticeships.FirstOrDefault(a => a.Id == id && a.ApprenticeId == appId);
            if (apprenticeship == default(Apprenticeship))
                return NotFound();

            apprenticeship.ConfirmedSection1 = true;
            return Ok(new ApprenticeshipResource(apprenticeship, Url));
        }

        [HttpPost("{Id}/confirm-section-2", Name = RouteNames.ConfirmApprenticeshipSection2)]
        public IActionResult ConfirmSection2Post(int appId, int id)
        {
            var apprenticeship = _apprenticeshipsCache.Apprenticeships.FirstOrDefault(a => a.Id == id && a.ApprenticeId == appId);
            if (apprenticeship == default(Apprenticeship))
                return NotFound();

            apprenticeship.ConfirmedSection2 = true;
            return Ok(new ApprenticeshipResource(apprenticeship, Url));
        }

        [HttpPost("{Id}/confirm-section-3", Name = RouteNames.ConfirmApprenticeshipSection3)]
        public IActionResult ConfirmSection3Post(int appId, int id)
        {
            var apprenticeship = _apprenticeshipsCache.Apprenticeships.FirstOrDefault(a => a.Id == id && a.ApprenticeId == appId);
            if (apprenticeship == default(Apprenticeship))
                return NotFound();

            apprenticeship.ConfirmedSection3 = true;
            return Ok(new ApprenticeshipResource(apprenticeship, Url));
        }

        [HttpPost("{Id}/confirm", Name = RouteNames.ConfirmApprenticeship)]
        public IActionResult ConfirmApprenticeshipPost(int appId, int id)
        {
            var apprenticeship = _apprenticeshipsCache.Apprenticeships.FirstOrDefault(a => a.Id == id && a.ApprenticeId == appId);
            if (apprenticeship == default(Apprenticeship))
                return NotFound();

            if (!apprenticeship.ConfirmedSection1 || !apprenticeship.ConfirmedSection2 || !apprenticeship.ConfirmedSection3)
                return Conflict("Sections must be confirmed BEFORE confirming the apprenticeship");

            apprenticeship.ApprenticeshipConfirmedOn = DateTime.Now;
            return Ok(new ApprenticeshipResource(apprenticeship, Url));
        }
    }
}
