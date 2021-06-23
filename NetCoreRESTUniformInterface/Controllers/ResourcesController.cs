using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreRESTUniformInterface.Infrastructure;
using SampleDomain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreRESTUniformInterface.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly ILogger<ResourcesController> _logger;

        public ResourcesController(ILogger<ResourcesController> logger)
        {
            _logger = logger;
        }


        [HttpGet()]
        public IActionResult Get()
        {
            var wrappedResponse = new
            {
                Links = new List<Link> { new Link { Rel = RouteNames.Apprentices, Href = Url.RouteUrl(RouteNames.Apprentices, new { }, "HTTPS"), Type = "GET" } }
            };
            return Ok(wrappedResponse);
        }
    }
}
