using AggregateApi.Application.Interfaces;
using AggregateApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text.Json;

namespace AggregateApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AggregateController(IAggregateService aggregateService) : ControllerBase
    {

        [HttpGet]
        [SwaggerOperation(Summary = "Fetches weather and news data from external APIs",
         Description = "This endpoint calls two external APIs (weather and news), handles them asynchronously, and returns an aggregate response with weather and news data.")]
        public async Task<ActionResult<AggregateResponse>> Get(
            [FromQuery,Required, SwaggerParameter(Description = "The date for the news articles (used for filtering).")] string? date,
            [FromQuery, Required, SwaggerParameter(Description = "Field to sort news articles by.")] string? sortBy = null,
            [FromQuery, Required, SwaggerParameter(Description = "Filter the news data by company'.")] string? company = "Apple",
            [FromQuery, Required, SwaggerParameter(Description = "Filter weather by country.")] string? country = "Greece")
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newdate = DateTime.Parse(date);
            var formattedDate = newdate.ToString("yyyy-MM-ddTHH:mm:ssZ"); // Adjust format as required
            var encodedDate = Uri.EscapeDataString(formattedDate);

            var effectiveSortBy = string.IsNullOrWhiteSpace(sortBy) ? "relevance" : sortBy;
            var effectiveCountry = string.IsNullOrWhiteSpace(country) ? "us" : country;

            // Call service with validated/defaulted inputs
            var response = await aggregateService.GetAggregateDataAsync(encodedDate, effectiveSortBy, company, effectiveCountry);

            return Ok(response);
        }

    }
}
