using HotelsWebAPI.Features.Hotels.Models;
using HotelsWebAPI.Features.Hotels.Queries;
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
            return Ok(await _sender.Send(new GetHotelsByDistanceQuery(Latitude, Longitude, PageNumber, PageSize)));
        }

        // POST api/SearchHotels
        [HttpPost]
        public async Task<ActionResult<List<SearchedHotelResponseModel>>> GetHotelsByDistancePost(GetHotelsByDistanceQuery query)
        {
            return Ok(await _sender.Send(new GetHotelsByDistanceQuery(query.Latitude, query.Longitude, query.PageNumber, query.PageSize)));
        }
    }
}
