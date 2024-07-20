using Api.Models;
using Api.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.V1
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GovernmentsController(IJsonService _jsonService) : ControllerBase
    {

        [HttpGet]
        public IActionResult GetAll()
        {
            var cities = _jsonService.GetAll<Government>("governments.json");

            return Ok(cities.Take(5));
        }
    }
}
