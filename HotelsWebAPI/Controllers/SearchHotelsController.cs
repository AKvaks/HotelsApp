using HotelsWebAPI.Features.Hotels.Models;
using HotelsWebAPI.Features.Hotels.Queries;
using HotelsWebAPI.Features.Hotels.Validators;
using HotelsWebAPI.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchHotelsController : ControllerBase
    {
        private readonly ISender _sender;
        public SearchHotelsController(ISender sender)
        {
            _sender = sender;
        }

        //It wasn't clear from the assignment whether to use GET or POST method, so I made both
        //In this case, I would prefer to use POST method, because I don't like long urls with too much information

        // GET api/SearchHotels?longitude=5.0&latitude=6.0&pageNumber=1&pageSize=10
        [HttpGet]
        [SwaggerOperation(Summary = "Returns a list of hotels depending on provided geolocation", Description = "Returns a paged list of hotels ordered by price and distance from provided geolocation")]
        public async Task<ActionResult<List<SearchedHotelResponseModel>>> GetHotelsByDistanceGet(double latitude, double longitude, int? pageNumber, int? pageSize)
        {
            var query = new GetHotelsByDistanceQuery(latitude, longitude, pageNumber, pageSize);

            var commandValidation = new GetHotelsByDistanceQueryValidator().Validate(query);
            if (!commandValidation.IsValid) return BadRequest(commandValidation.Errors.Select(x => x.ErrorMessage));

            var result = await _sender.Send(query);
            if (result.StatusCode == 500) return StatusCode(result.StatusCode, result.Message);
            return Ok(result.Value);
        }

        // POST api/SearchHotels
        [HttpPost]
        [SwaggerOperation(Summary = "Returns a list of hotels depending on provided geolocation", Description = "Returns a paged list of hotels ordered by price and distance from provided geolocation")]
        public async Task<ActionResult<BaseResponse<PagedResponse<SearchedHotelResponseModel>?>>> GetHotelsByDistancePost(GetHotelsByDistanceQuery query)
        {
            var commandValidation = new GetHotelsByDistanceQueryValidator().Validate(query);
            if (!commandValidation.IsValid) return BadRequest(commandValidation.Errors.Select(x => x.ErrorMessage));

            var result = await _sender.Send(query);
            if (result.StatusCode == 500) return StatusCode(result.StatusCode, result.Message);
            return Ok(result.Value);
        }
    }
}
