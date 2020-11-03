using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Thornless.UI.Web.ViewModels.Version;

namespace Thornless.UI.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VersionController : BaseApiController
    {
        [HttpGet("lastupdate")]
        public IActionResult GetLastUpdate()
        {
            return ApiResponse(new LatestBuildViewModel
            {
                LatestBuildDate = GetLatestBuildDate(),
            });
        }

        private DateTimeOffset GetLatestBuildDate()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileDate = System.IO.File.GetCreationTime(assembly.Location);
            return fileDate;
        }
    }
}
