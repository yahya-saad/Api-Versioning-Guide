using Api.Models;
using Api.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.V2
{
    [ApiController]
    [ApiVersion(2)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CitiesController(IJsonService _jsonService) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var cities = _jsonService.GetAll<City>("cities.json");
            var governments = _jsonService.GetAll<Government>("governments.json");
            var governmentDict = governments.ToDictionary(g => g.Id);

            var citiesWithGovernments = cities.Select(city => new City
            {
                Id = city.Id,
                CityNameAr = city.CityNameAr,
                CityNameEn = city.CityNameEn,
                Government = governmentDict.TryGetValue(city.GovernmentId, out var government) ? government : null
            }).ToList();


            return Ok(citiesWithGovernments);
        }
    }
}
