using FakeItEasy;
using FluentAssertions;
using HotelsWebAPI.Controllers;
using HotelsWebAPI.Features.Hotels.Commands;
using HotelsWebAPI.Features.Hotels.Models;
using HotelsWebAPI.Features.Hotels.Queries;
using HotelsWebAPI.Features.Hotels.Validators;
using HotelsWebAPI.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HotelsWebAPI.Tests.Controller
{
    public class HotelsManagementControllerTests
    {
        private readonly HotelsManagementController _controller;
        private readonly ISender _sender;
        public HotelsManagementControllerTests()
        {
            // Arrange: Create a fake ISender instance
            _sender = A.Fake<ISender>();

            // Initialize the controller with the fake ISender
            _controller = new HotelsManagementController(_sender);
        }

        [Fact]
        public async Task HotelsManagement_GetAllHotels_ReturnsOk()
        {
            // Arrange
            var fakeHotels = new List<HotelResponseModel>
            {
                new HotelResponseModel { Id = 1, HotelName = "Hotel A" },
                new HotelResponseModel { Id = 2, HotelName = "Hotel B" }
            };

            var fakeResult = new BaseResponse<List<HotelResponseModel>>
            {
                StatusCode = 200,
                Message = "Hotel list successfully retrieved.",
                Value = fakeHotels
            };

            A.CallTo(() => _sender.Send(A<GetAllHotelsQuery>.Ignored, A<CancellationToken>.Ignored))
                .Returns(fakeResult);

            // Act
            var actionResult = await _controller.GetAllHotels();

            // Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(fakeHotels);
        }

        [Fact]
        public async Task HotelsManagement_GetAllHotels_Returns500WithMesssagek()
        {
            // Arrange
            var errorMessage = "Error during communication with database!";
            var fakeResult = new BaseResponse<List<HotelResponseModel>>
            {
                StatusCode = 500,
                Message = errorMessage,
            };

            A.CallTo(() => _sender.Send(A<GetAllHotelsQuery>.Ignored, A<CancellationToken>.Ignored))
                .Returns(fakeResult);

            // Act
            var actionResult = await _controller.GetAllHotels();

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);

            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().Be(errorMessage);
        }

        [Fact]
        public async Task HotelsManagement_GetHotelById_ReturnsOk()
        {
            // Arrange
            int hotelId = 5;
            var fakeHotel = new HotelResponseModel { Id = hotelId, HotelName = "Hotel A" };

            var fakeResult = new BaseResponse<HotelResponseModel?>
            {
                StatusCode = 200,
                Message = "Hotel retrieved successfully!",
                Value = fakeHotel
            };

            A.CallTo(() => _sender.Send(A<GetHotelByIdQuery>.That.Matches(q => q.Id == hotelId), A<CancellationToken>.Ignored))
                .Returns(fakeResult);

            // Act
            var actionResult = await _controller.GetHotelById(hotelId);

            // Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(fakeHotel);
        }

        [Fact]
        public async Task HotelsManagement_GetHotelById_ReturnsBadRequest()
        {
            // Arrange
            int invalidHotelId = -1;
            var command = new GetHotelByIdQuery(invalidHotelId);
            var validator = new GetHotelByIdQueryValidator();
            var validationResult = validator.Validate(command);

            // Act
            var actionResult = await _controller.GetHotelById(invalidHotelId);

            // Assert
            actionResult.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        [Fact]
        public async Task HotelsManagement_GetHotelById_Returns404WithMessage()
        {
            // Arrange
            int hotelId = 5;
            var errorMessage = $"Hotel with Id {hotelId} not found!";

            var fakeResult = new BaseResponse<HotelResponseModel?>
            {
                StatusCode = 404,
                Message = errorMessage
            };

            A.CallTo(() => _sender.Send(A<GetHotelByIdQuery>.That.Matches(q => q.Id == hotelId), A<CancellationToken>.Ignored))
                .Returns(fakeResult);

            // Act
            var actionResult = await _controller.GetHotelById(hotelId);

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(404);

            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().Be(errorMessage);
        }

        [Fact]
        public async Task HotelsManagement_GetHotelById_Returns500WithMessage()
        {
            // Arrange
            int hotelId = 5;
            var errorMessage = "Error during communication with database!";

            var fakeResult = new BaseResponse<HotelResponseModel?>
            {
                StatusCode = 500,
                Message = errorMessage
            };

            A.CallTo(() => _sender.Send(A<GetHotelByIdQuery>.That.Matches(q => q.Id == hotelId), A<CancellationToken>.Ignored))
                .Returns(fakeResult);

            // Act
            var actionResult = await _controller.GetHotelById(hotelId);

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);

            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().Be(errorMessage);
        }

        [Fact]
        public async Task HotelsManagement_AddHotel_ReturnsOk()
        {
            // Arrange
            var command = new AddHotelCommand("TestHotel", 100m, 45.123, 15.123);

            var fakeResult = new BaseResponse<int>
            {
                StatusCode = 201,
                Message = "Hotel was successfuly added!"
            };

            A.CallTo(() => _sender.Send(command, A<CancellationToken>.Ignored))
                .Returns(fakeResult);

            // Act
            var actionResult = await _controller.AddHotel(command);

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(201);

            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().Be("Hotel was successfuly added!");
        }

        [Fact]
        public async Task HotelsManagement_AddHotel_ReturnsBadRequest()
        {
            // Arrange
            // An invalid command (missing hotel name)
            var invalidCommand = new AddHotelCommand("", 150.0m, 37.7749, -122.4194);

            var commandValidation = new AddHotelCommandValidator().Validate(invalidCommand);

            // Act
            var actionResult = await _controller.AddHotel(invalidCommand);

            // Assert
            actionResult.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(commandValidation.Errors.Select(x => x.ErrorMessage));
        }

        [Fact]
        public async Task HotelsManagement_AddHotel_Returns500WithMessage()
        {
            // Arrange
            var command = new AddHotelCommand("New Hotel", 150.0m, 45.7749, 15.4194);

            var fakeResult = new BaseResponse<int>
            {
                StatusCode = 500,
                Message = "Error during communication with database!"
            };

            A.CallTo(() => _sender.Send(command, A<CancellationToken>.Ignored))
                .Returns(fakeResult);

            // Act
            var actionResult = await _controller.AddHotel(command);

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);

            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().Be("Error during communication with database!");
        }

        [Fact]
        public async Task HotelsManagement_UpdateHotelById_ReturnsOk()
        {
            // Arrange
            var command = new EditHotelCommand(1, "Updated Hotel", 200.0m, 45.7749, 15.4194);

            var fakeResult = new BaseResponse<int>
            {
                StatusCode = 200,
                Message = "Hotel was successfuly updated!"
            };

            A.CallTo(() => _sender.Send(command, A<CancellationToken>.Ignored))
                .Returns(fakeResult);

            // Act
            var actionResult = await _controller.UpdateHotelById(command);

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(200);

            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().Be("Hotel was successfuly updated!");
        }

        [Fact]
        public async Task HotelsManagement_UpdateHotelById_ReturnsBadRequest()
        {
            // Arrange
            // Invalid command (missing hotel name or invalid ID)
            var invalidCommand = new EditHotelCommand(0, "", 200.0m, 37.7749, -122.4194);
            var commandValidation = new EditHotelCommandValidator().Validate(invalidCommand);

            // Act
            var actionResult = await _controller.UpdateHotelById(invalidCommand);

            // Assert
            actionResult.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(commandValidation.Errors.Select(x => x.ErrorMessage));
        }

        [Fact]
        public async Task HotelsManagement_UpdateHotelById_Returns404WithMessage()
        {
            // Arrange
            var command = new EditHotelCommand(99, "Updated Hotel", 200.0m, 37.7749, -122.4194);
            var errorMessage = $"Hotel with Id {command.Id} not found!";

            var fakeResult = new BaseResponse<int>
            {
                StatusCode = 404,
                Message = errorMessage
            };

            A.CallTo(() => _sender.Send(command, A<CancellationToken>.Ignored))
                .Returns(fakeResult);

            // Act
            var actionResult = await _controller.UpdateHotelById(command);

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(404);

            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().Be(errorMessage);
        }

        [Fact]
        public async Task HotelsManagement_UpdateHotelById_Returns500WithMessage()
        {
            // Arrange
            var command = new EditHotelCommand(1, "Updated Hotel", 200.0m, 37.7749, -122.4194);

            var fakeResult = new BaseResponse<int>
            {
                StatusCode = 500,
                Message = "Error during communication with database!"
            };

            A.CallTo(() => _sender.Send(command, A<CancellationToken>.Ignored))
                .Returns(fakeResult);

            // Act
            var actionResult = await _controller.UpdateHotelById(command);

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);

            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().Be("Error during communication with database!");
        }

        [Fact]
        public async Task HotelsManagement_DeleteHotelById_ReturnsOk()
        {
            // Arrange
            var command = new DeleteHotelCommand(1);

            var fakeResult = new BaseResponse<int>
            {
                StatusCode = 200,
                Message = "Hotel was successfuly deleted!"
            };

            A.CallTo(() => _sender.Send(command, A<CancellationToken>.Ignored))
                .Returns(fakeResult);

            // Act
            var actionResult = await _controller.DeleteHotelById(command);

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(200);

            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().Be("Hotel was successfuly deleted!");
        }

        [Fact]
        public async Task HotelsManagement_DeleteHotelById_ReturnsBadRequest()
        {
            // Arrange
            // Invalid command (invalid ID)
            var invalidCommand = new DeleteHotelCommand(0);
            var commandValidation = new DeleteHotelCommandValidator().Validate(invalidCommand);

            // Act
            var actionResult = await _controller.DeleteHotelById(invalidCommand);

            // Assert
            actionResult.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(commandValidation.Errors.Select(x => x.ErrorMessage));
        }

        [Fact]
        public async Task HotelsManagement_DeleteHotelById_Returns404WithMessage()
        {
            // Arrange
            var command = new DeleteHotelCommand(99);
            var errorMessage = $"Hotel with Id {command.Id} not found!";

            var fakeResult = new BaseResponse<int>
            {
                StatusCode = 404,
                Message = errorMessage
            };

            A.CallTo(() => _sender.Send(command, A<CancellationToken>.Ignored))
                .Returns(fakeResult);

            // Act
            var actionResult = await _controller.DeleteHotelById(command);

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(404);

            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().Be(errorMessage);
        }

        [Fact]
        public async Task HotelsManagement_DeleteHotelById_Returns500WithMessage()
        {
            // Arrange
            var command = new DeleteHotelCommand(1);
            var errorMessage = "Error during communication with database!";

            var fakeResult = new BaseResponse<int>
            {
                StatusCode = 500,
                Message = errorMessage
            };

            A.CallTo(() => _sender.Send(command, A<CancellationToken>.Ignored))
                .Returns(fakeResult);

            // Act
            var actionResult = await _controller.DeleteHotelById(command);

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);

            actionResult.Result.Should().BeOfType<ObjectResult>()
                .Which.Value.Should().Be(errorMessage);
        }
    }
}
