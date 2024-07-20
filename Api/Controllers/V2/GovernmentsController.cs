using Api.Models;
using Api.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [ApiVersion(2)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GovernmentsController(IJsonService _jsonService) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var governments = _jsonService.GetAll<Government>("governments.json");
            return Ok(governments);
        }
    }
}
