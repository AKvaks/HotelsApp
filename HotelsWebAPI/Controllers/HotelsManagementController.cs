using HotelsWebAPI.Features.Hotels.Commands;
using HotelsWebAPI.Features.Hotels.Models;
using HotelsWebAPI.Features.Hotels.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsManagementController : ControllerBase
    {
        private readonly ISender _sender;
        public HotelsManagementController(ISender sender)
        {
            _sender = sender;
        }

        // GET: api/HotelsManagement
        [HttpGet]
        public async Task<ActionResult<List<HotelResponseModel>>> GetAllHotels()
        {
            return Ok(await _sender.Send(new GetAllHotelsQuery()));
        }

        // GET api/HotelsManagement/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelResponseModel?>> GetHotelById(int id)
        {
            var result = await _sender.Send(new GetHotelByIdQuery(id));

            if (result == null) return NotFound($"Hotel with Id: {id} not found!");

            return Ok(result);
        }

        // POST api/HotelsManagement
        [HttpPost]
        public async Task<ActionResult<int>> AddHotel(AddHotelCommand command)
        {
            var hotelId = await _sender.Send(command);
            return Ok(hotelId);
        }

        // PUT api/HotelsManagement
        [HttpPut]
        public async Task<ActionResult<int>> UpdateHotelById(EditHotelCommand command)
        {
            var hotelId = await _sender.Send(command);
            return Ok(hotelId);
        }

        // DELETE api/HotelsManagement
        [HttpDelete]
        public async Task<ActionResult<int>> DeleteHotelById(DeleteHotelCommand command)
        {
            var hotelId = await _sender.Send(command);
            return Ok(hotelId);
        }
    }
}
