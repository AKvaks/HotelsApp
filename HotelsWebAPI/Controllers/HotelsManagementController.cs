using HotelsWebAPI.Features.Hotels.Commands;
using HotelsWebAPI.Features.Hotels.Models;
using HotelsWebAPI.Features.Hotels.Queries;
using HotelsWebAPI.Features.Hotels.Validators;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Gets all hotels", Description = "Returns the list of all hotels in database")]
        public async Task<ActionResult<List<HotelResponseModel>>> GetAllHotels()
        {
            var result = await _sender.Send(new GetAllHotelsQuery());
            if (result.StatusCode == 500) return StatusCode(result.StatusCode, result.Message);
            return Ok(result.Value);
        }

        // GET api/HotelsManagement/5
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets a hotel specified by Id", Description = "Returns the hotel details for the provided Id")]
        public async Task<ActionResult<HotelResponseModel?>> GetHotelById(int id)
        {
            var command = new GetHotelByIdQuery(id);

            var commandValidation = new GetHotelByIdQueryValidator().Validate(command);
            if (!commandValidation.IsValid) return BadRequest(commandValidation.Errors.Select(x => x.ErrorMessage));

            var result = await _sender.Send(command);

            if (result.StatusCode == 500 || result.StatusCode == 404) return StatusCode(result.StatusCode, result.Message);

            return Ok(result.Value);
        }

        // POST api/HotelsManagement
        [HttpPost]
        [SwaggerOperation(Summary = "Adds a provided hotel", Description = "Adds a provided hotel to database and returns a message")]
        public async Task<ActionResult<string>> AddHotel(AddHotelCommand command)
        {
            var commandValidation = new AddHotelCommandValidator().Validate(command);
            if (!commandValidation.IsValid) return BadRequest(commandValidation.Errors.Select(x => x.ErrorMessage));

            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }

        // PUT api/HotelsManagement
        [HttpPut]
        [SwaggerOperation(Summary = "Updates a provided hotel", Description = "Updates a provided hotel in database and returns a message")]
        public async Task<ActionResult<string>> UpdateHotelById(EditHotelCommand command)
        {
            var commandValidation = new EditHotelCommandValidator().Validate(command);
            if (!commandValidation.IsValid) return BadRequest(commandValidation.Errors.Select(x => x.ErrorMessage));

            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }

        // DELETE api/HotelsManagement
        [HttpDelete]
        [SwaggerOperation(Summary = "Deletes a hotel specified by Id", Description = "Deletes a hotel specified by Id from database and returns a message")]
        public async Task<ActionResult<int>> DeleteHotelById(DeleteHotelCommand command)
        {
            var commandValidation = new DeleteHotelCommandValidator().Validate(command);
            if (!commandValidation.IsValid) return BadRequest(commandValidation.Errors.Select(x => x.ErrorMessage));

            var result = await _sender.Send(command);
            return StatusCode(result.StatusCode, result.Message);
        }
    }
}
