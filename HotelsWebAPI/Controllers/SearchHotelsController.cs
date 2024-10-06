using HotelsWebAPI.Features.Hotels.Models;
using HotelsWebAPI.Features.Hotels.Queries;
using HotelsWebAPI.Features.Hotels.Validators;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        // GET api/SearchHotels/5
        [HttpGet("longitude={Longitude}&latitude={Latitude}&pageNumber={PageNumber}&pageSize={PageSize}")]
        public async Task<ActionResult<List<SearchedHotelResponseModel>>> GetHotelsByDistanceGet(double Latitude, double Longitude, int PageNumber, int PageSize)
        {
            var query = new GetHotelsByDistanceQuery(Latitude, Longitude, PageNumber, PageSize);

            var commandValidation = new GetHotelsByDistanceQueryValidator().Validate(query);
            if (!commandValidation.IsValid) return BadRequest(commandValidation.Errors.Select(x => x.ErrorMessage));

            return Ok(await _sender.Send(query));
        }

        // POST api/SearchHotels
        [HttpPost]
        public async Task<ActionResult<List<SearchedHotelResponseModel>>> GetHotelsByDistancePost(GetHotelsByDistanceQuery query)
        {
            var commandValidation = new GetHotelsByDistanceQueryValidator().Validate(query);
            if (!commandValidation.IsValid) return BadRequest(commandValidation.Errors.Select(x => x.ErrorMessage));

            return Ok(await _sender.Send(new GetHotelsByDistanceQuery(query.Latitude, query.Longitude, query.PageNumber, query.PageSize)));
        }
    }
}
