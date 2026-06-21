using AggregateApi.Application.Interfaces;
using AggregateApi.Domain;
using AggregateApi.Model;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AggregateApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AggregateController(IAggregateService aggregateService,IOptions<ApiOptions> options) : ControllerBase
    {

        [HttpGet]
        [SwaggerOperation(Summary = "Fetches weather and news data from external APIs",
         Description = "This endpoint calls two external APIs (weather and news), handles them asynchronously, and returns an aggregate response with weather and news data.")]
        public async Task<ActionResult<AggregateResponse>> Get([FromQuery] AggregateRequest request)
        {
            try
            {
                var response = await aggregateService.GetAggregateDataAsync(request.Date,
                    request.SortBy,
                    request.Company,
                    request.Country,
                    request.Category,
                    request.Url);

                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Something went wrong");
            }
        }
    }
}
