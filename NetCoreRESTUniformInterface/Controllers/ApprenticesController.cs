using Microsoft.AspNetCore.JsonPatch;
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
    [ApiController]
    [Route("[controller]")]
    public class ApprenticesController : ControllerBase
    {
        private readonly ILogger<ApprenticesController> _logger;

        public ApprenticesController(ILogger<ApprenticesController> logger)
        {
            _logger = logger;
        }

        [HttpPost("", Name = RouteNames.Apprentices)]
        public IActionResult Post(Apprentice apprentice)
        {
            apprentice.Id = ApprenticeCache.Apprentices.Max(a => a.Id) + 1;
            ApprenticeCache.Apprentices.Add(apprentice);
            return new CreatedResult(Url.RouteUrl(RouteNames.Apprentice, new { Id = apprentice.Id }, "HTTPS"), new ApprenticeResource(apprentice, Url));
        }

        [HttpGet("", Name = RouteNames.Apprentices)]
        public IActionResult Get()
        {
            var appResources = ApprenticeCache.Apprentices.Select(a => new ApprenticeResource(a, Url));
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
            var app = ApprenticeCache.Apprentices.Where(a => a.Id == id).FirstOrDefault();
            if (app == default(Apprentice))
                return NotFound();

            var appR = new ApprenticeResource(app, Url);
            return Ok(appR);
        }

        [HttpPatch("{Id}", Name = RouteNames.Apprentice)]
        public IActionResult PatchApprentice(int id, [FromBody] JsonPatchDocument<Apprentice> apprenticePatch)
        {
            var app = ApprenticeCache.Apprentices.Where(a => a.Id == id).FirstOrDefault();
            if (app == default(Apprentice))
                return NotFound();

            // Apply the PATCH instructio to the entity
            apprenticePatch.ApplyTo(app);

            var appR = new ApprenticeResource(app, Url);
            return Ok(appR);
        }

        [HttpPost("{Id}/confirm-personal-details", Name = RouteNames.ConfirmApprenticePersonalDetails)]
        public IActionResult ConfirmPersonalDetailsPost(int id)
        {
            var app = ApprenticeCache.Apprentices.FirstOrDefault(a => a.Id == id);
            if (app == default(Apprentice))
                return BadRequest();

            app.PersonalDetailsConfirmedOn = DateTime.Now;
            var appR = new ApprenticeResource(app, Url);
            return Ok(appR);
        }

    }
}
