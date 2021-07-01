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
    [ApiController]
    [Route("[controller]")]
    public class ApprenticesController : SampleControllerBase
    {
        private readonly ILogger<ApprenticesController> _logger;
        private IApprenticeCache _apprenticeCache;

        public ApprenticesController(ILogger<ApprenticesController> logger, IApprenticeCache apprenticeCache)
        {
            _logger = logger;
            _apprenticeCache = apprenticeCache;
        }

        [HttpPost("", Name = RouteNames.Apprentices)]
        public IActionResult Post(Apprentice apprentice)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            apprentice.Id = _apprenticeCache.Apprentices.Max(a => a.Id) + 1;
            _apprenticeCache.Apprentices.Add(apprentice);
            return new CreatedResult(Url.RouteUrl(RouteNames.Apprentice, new { Id = apprentice.Id }, "HTTPS"), new ApprenticeResource(apprentice, Url));
        }

        [HttpGet("", Name = RouteNames.Apprentices)]
        public IActionResult Get()
        {
            var appResources = _apprenticeCache.Apprentices.Select(a => new ApprenticeResource(a, Url));
            var wrappedResponse = new
            {
                Items = appResources,
                Links = new List<Link> { new Link { Rel = "self", Href = Url.RouteUrl(RouteNames.Apprentices,  new { }, "HTTPS"), Type = "GET" } }
            };
            return Ok(wrappedResponse);
        }

        [HttpGet("{Id}", Name = RouteNames.Apprentice)]
        public IActionResult GetApprentice(int id)
        {
            var app = _apprenticeCache.Apprentices.Where(a => a.Id == id).FirstOrDefault();
            if (app == default(Apprentice))
                return NotFound();

            var appR = new ApprenticeResource(app, Url);
            return Ok(appR);
        }

        [HttpPatch("{Id}", Name = RouteNames.Apprentice)]
        public IActionResult PatchApprentice(int id, [FromBody] JsonPatchDocument<Apprentice> apprenticePatch)
        {
            var app = _apprenticeCache.Apprentices.FirstOrDefault(a => a.Id == id);
            if (app == default(Apprentice))
                return NotFound();
            var tmpApp = new Apprentice(app);

            // Apply the PATCH instruction to the entity and validate
            ApplyToValidated(() =>
                { apprenticePatch.ApplyTo(tmpApp); TryValidateModel(tmpApp); }
            );

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            app = tmpApp;
            var appR = new ApprenticeResource(app, Url);
            return Ok(appR);
        }

        [HttpPost("{Id}/confirm-personal-details", Name = RouteNames.ConfirmApprenticePersonalDetails)]
        public IActionResult ConfirmPersonalDetailsPost(int id)
        {
            var app = _apprenticeCache.Apprentices.FirstOrDefault(a => a.Id == id);
            if (app == default(Apprentice))
                ModelState.AddModelError("Id", "The specificed ID was not found");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            app.PersonalDetailsConfirmedOn = DateTime.Now;
            var appR = new ApprenticeResource(app, Url);
            return Ok(appR);
        }

    }
}
