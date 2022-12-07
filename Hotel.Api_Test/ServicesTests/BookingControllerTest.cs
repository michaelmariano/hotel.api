using Application.Classes;
using Application.Interfaces;
using Bogus;
using Domain.Entities;
using Domain.Exceptions;
using Domain.UnitTests;
using FluentAssertions;
using Hotel.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Repository.Interfaces;
using System.Net;

namespace Hotel.Api_Test.ServicesTests
{
    public class BookingControllerTest
    {
        private readonly BookingController _bookingController;

        private readonly IBookingApp _bookingApp;
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomApp _roomApp;
        private readonly IRoomRepository _roomRepository;

        private readonly Faker _faker;

        public BookingControllerTest()
        {
            _faker = new Faker();

            _bookingRepository = Substitute.For<IBookingRepository>();
            _roomRepository = Substitute.For<IRoomRepository>();

            _roomApp = new RoomApp(_roomRepository);

            _bookingApp = new BookingApp(_bookingRepository, _roomApp);

            _bookingController = new BookingController(_bookingApp);
        }

        [Fact(DisplayName = "InsertBookingShouldReturnOkResultAndIdValue")]
        public async Task InsertBookingShouldReturnOkResultAndIdValue()
        {
            //Arrange
            int idBooking = _faker.Random.Int(1, 1000);
            int idClient = _faker.Random.Int(1, 1000);
            int idRoom = _faker.Random.Int(1, 1000);
            DateTime checkin = DateTime.Now.AddDays(1);
            DateTime checkout = DateTime.Now.AddDays(3);

            var booking = new Booking
            {
                IdBooking = idBooking,
                IdClient = idClient,
                IdRoom = idRoom,
                Checkin = checkin,
                Checkout = checkout
            };

            _bookingRepository.InsertAsync(booking).Returns(idBooking);

            var room = new Room
            {
                IdRoom = idRoom,
                IsAvaible = true,
                Number = "140"
            };

            _roomRepository.GetAsync(booking.IdRoom).Returns(room);

            //Act
            var result = await _bookingController.InsertAsync(booking);

            //Assert
            result.Should().NotBeNull();

            var objectResult = result as CreatedResult;

            objectResult.Should().BeOfType<CreatedResult>();

            var objResult = result as ObjectResult;

            int idBookingCurrent = Convert.ToInt32(objResult?.Value);

            Assert.Equal(idBooking, idBookingCurrent);
        }

        [Fact(DisplayName = "InsertBookingShouldReturnRoomNotFounded")]
        public async Task InsertBookingShouldReturnRoomNotFounded()
        {
            //Arrange
            DateTime checkin = DateTime.Now;
            DateTime checkout = DateTime.Now;
            int expectedStatusCode = 422;
            string expectedMessage = "Room not founded.";
            int idBooking = _faker.Random.Int(1, 1000);
            int idClient = _faker.Random.Int(1, 1000);
            int idRoom = _faker.Random.Int(1, 1000);
            int currentStatusCode = 0;
            string currentMessage = "";

            var booking = new Booking
            {
                IdBooking = idBooking,
                IdClient = idClient,
                IdRoom = idRoom,
                Checkin = checkin,
                Checkout = checkout,
            };

            _bookingRepository.InsertAsync(booking).Returns(idBooking);

            Room room = null;

            _roomRepository.GetAsync(booking.IdRoom).Returns(room);


            //Act
            IActionResult? result;

            try
            {
                result = await _bookingController.InsertAsync(booking);
            }
            catch (CustomException cx)
            {
                currentStatusCode = cx.StatusCode;
                currentMessage = cx.Message;
            }
            catch (Exception ex)
            {
                currentStatusCode = 500;
                currentMessage = ex.Message;
            }

            //Assert    
            Assert.Equal(expectedStatusCode, currentStatusCode);
            Assert.Equal(expectedMessage, currentMessage);
        }

        [Fact(DisplayName = "InsertBookingShouldReturnRoomUnavaible")]
        public async Task InsertBookingShouldReturnRoomUnavaible()
        {
            //Arrange
            DateTime checkin = DateTime.Now;
            DateTime checkout = DateTime.Now;
            int expectedStatusCode = 422;


            int idBooking = _faker.Random.Int(1, 1000);
            int idClient = _faker.Random.Int(1, 1000);
            int idRoom = _faker.Random.Int(1, 1000);

            var booking = new Booking
            {
                IdBooking = idBooking,
                IdClient = idClient,
                IdRoom = idRoom,
                Checkin = checkin,
                Checkout = checkout,
            };

            _bookingRepository.InsertAsync(booking).Returns(idBooking);

            var room = new Room
            {
                IdRoom = idRoom,
                IsAvaible = false,
                Number = "140"
            };

            string expectedMessage = $"Room {room.Number} unavaible, please choose other.";

            _roomRepository.GetAsync(booking.IdRoom).Returns(room);

            int currentStatusCode = 0;
            string currentMessage = "";

            //Act
            IActionResult? result;

            try
            {
                result = await _bookingController.InsertAsync(booking);
            }
            catch (CustomException cx)
            {
                currentStatusCode = cx.StatusCode;
                currentMessage = cx.Message;
            }
            catch (Exception ex)
            {
                currentStatusCode = 500;
                currentMessage = ex.Message;
            }

            //Assert    
            Assert.Equal(expectedStatusCode, currentStatusCode);
            Assert.Equal(expectedMessage, currentMessage);
        }

        [Theory(DisplayName = "InsertBookingShouldReturnUnprocessableEntityAndMessage")]
        [InlineData(EnumBookingCase.CheckinCannotBeToday)]
        [InlineData(EnumBookingCase.BookingCannotBeMadeMoreThan30DaysInAdvance)]
        [InlineData(EnumBookingCase.TheStayCannotExceed3Days)]
        [InlineData(EnumBookingCase.CheckinCannotBeGreaterThanCheckout)]
        public async Task InsertBookingShouldReturnUnprocessableEntityAndMessage(EnumBookingCase bookingCase)
        {
            //Arrange
            DateTime checkin = DateTime.Now;
            DateTime checkout = DateTime.Now;
            int expectedStatusCode = 422;
            string expectedMessage = "";

            switch (bookingCase)
            {
                case EnumBookingCase.CheckinCannotBeToday:
                    checkout = checkin.AddDays(1);
                    expectedMessage = "Checkin date cannot be today.";
                    break;
                case EnumBookingCase.BookingCannotBeMadeMoreThan30DaysInAdvance:
                    checkin = checkin.AddDays(-31);
                    checkout = checkin.AddDays(2);
                    expectedMessage = "Booking cannot be made more than 30 days in advance.";
                    break;
                case EnumBookingCase.TheStayCannotExceed3Days:
                    checkin = checkin.AddDays(1);
                    checkout = checkin.AddDays(4);
                    expectedMessage = "The stay cannot exceed 3 days.";
                    break;
                case EnumBookingCase.CheckinCannotBeGreaterThanCheckout:
                    checkin = checkin.AddDays(3);
                    checkout.AddDays(2);
                    expectedMessage = "Checkin cannot be greater than checkout.";
                    break;
            }

            int idBooking = _faker.Random.Int(1, 1000);
            int idClient = _faker.Random.Int(1, 1000);
            int idRoom = _faker.Random.Int(1, 1000);

            var booking = new Booking
            {
                IdBooking = idBooking,
                IdClient = idClient,
                IdRoom = idRoom,
                Checkin = checkin,
                Checkout = checkout,
            };

            _bookingRepository.InsertAsync(booking).Returns(idBooking);

            var room = new Room
            {
                IdRoom = idRoom,
                IsAvaible = true,
                Number = "140"
            };

            _roomRepository.GetAsync(booking.IdRoom).Returns(room);

            int currentStatusCode = 0;
            string currentMessage = "";

            //Act
            IActionResult? result;

            try
            {
                result = await _bookingController.InsertAsync(booking);
            }
            catch (CustomException cx)
            {
                currentStatusCode = cx.StatusCode;
                currentMessage = cx.Message;
            }
            catch (Exception ex)
            {
                currentStatusCode = 500;
                currentMessage = ex.Message;
            }

            //Assert    
            Assert.Equal(expectedStatusCode, currentStatusCode);
            Assert.Equal(expectedMessage, currentMessage);
        }

        [Theory(DisplayName = "UpdateBookingShouldReturnUnprocessableEntityAndMessage")]
        [InlineData(EnumBookingCase.CheckinCannotBeToday)]
        [InlineData(EnumBookingCase.BookingCannotBeMadeMoreThan30DaysInAdvance)]
        [InlineData(EnumBookingCase.TheStayCannotExceed3Days)]
        [InlineData(EnumBookingCase.CheckinCannotBeGreaterThanCheckout)]
        public async Task UpdateBookingShouldReturnUnprocessableEntityAndMessage(EnumBookingCase bookingCase)
        {
            //Arrange
            DateTime checkin = DateTime.Now;
            DateTime checkout = DateTime.Now;
            int expectedStatusCode = 422;
            string expectedMessage = "";

            switch (bookingCase)
            {
                case EnumBookingCase.CheckinCannotBeToday:
                    checkout = checkin.AddDays(1);
                    expectedMessage = "Checkin date cannot be today.";
                    break;
                case EnumBookingCase.BookingCannotBeMadeMoreThan30DaysInAdvance:
                    checkin = checkin.AddDays(-31);
                    checkout = checkin.AddDays(2);
                    expectedMessage = "Booking cannot be made more than 30 days in advance.";
                    break;
                case EnumBookingCase.TheStayCannotExceed3Days:
                    checkin = checkin.AddDays(1);
                    checkout = checkin.AddDays(4);
                    expectedMessage = "The stay cannot exceed 3 days.";
                    break;
                case EnumBookingCase.CheckinCannotBeGreaterThanCheckout:
                    checkin = checkin.AddDays(3);
                    checkout.AddDays(2);
                    expectedMessage = "Checkin cannot be greater than checkout.";
                    break;
            }

            int idBooking = _faker.Random.Int(1, 1000);
            int idClient = _faker.Random.Int(1, 1000);
            int idRoom = _faker.Random.Int(1, 1000);

            var booking = new Booking
            {
                IdBooking = idBooking,
                IdClient = idClient,
                IdRoom = idRoom,
                Checkin = checkin,
                Checkout = checkout,
            };

            await _bookingRepository.UpdateAsync(booking);

            var room = new Room
            {
                IdRoom = idRoom,
                IsAvaible = true,
                Number = "140"
            };

            _roomRepository.GetAsync(booking.IdRoom).Returns(room);

            int currentStatusCode = 0;
            string currentMessage = "";

            //Act
            IActionResult? result;

            try
            {
                result = await _bookingController.UpdateAsync(booking);
            }
            catch (CustomException cx)
            {
                currentStatusCode = cx.StatusCode;
                currentMessage = cx.Message;
            }
            catch (Exception ex)
            {
                currentStatusCode = 500;
                currentMessage = ex.Message;
            }

            //Assert    
            Assert.Equal(expectedStatusCode, currentStatusCode);
            Assert.Equal(expectedMessage, currentMessage);
        }

        [Fact(DisplayName = "UpdateBookingShouldReturnRoomNotFounded")]
        public async Task UpdateBookingShouldReturnRoomNotFounded()
        {
            //Arrange
            DateTime checkin = DateTime.Now;
            DateTime checkout = DateTime.Now;
            int expectedStatusCode = 422;
            string expectedMessage = "Room not founded.";

            int idBooking = _faker.Random.Int(1, 1000);
            int idClient = _faker.Random.Int(1, 1000);
            int idRoom = _faker.Random.Int(1, 1000);

            var booking = new Booking
            {
                IdBooking = idBooking,
                IdClient = idClient,
                IdRoom = idRoom,
                Checkin = checkin,
                Checkout = checkout,
            };

            await _bookingRepository.UpdateAsync(booking);

            Room room = null;

            _roomRepository.GetAsync(booking.IdRoom).Returns(room);

            int currentStatusCode = 0;
            string currentMessage = "";

            //Act
            IActionResult? result;

            try
            {
                result = await _bookingController.UpdateAsync(booking);
            }
            catch (CustomException cx)
            {
                currentStatusCode = cx.StatusCode;
                currentMessage = cx.Message;
            }
            catch (Exception ex)
            {
                currentStatusCode = 500;
                currentMessage = ex.Message;
            }

            //Assert    
            Assert.Equal(expectedStatusCode, currentStatusCode);
            Assert.Equal(expectedMessage, currentMessage);
        }

        [Fact(DisplayName = "UpdateBookingShouldReturnRoomUnavaible")]
        public async Task UpdateBookingShouldReturnRoomUnavaible()
        {
            //Arrange
            DateTime checkin = DateTime.Now;
            DateTime checkout = DateTime.Now;
            int expectedStatusCode = 422;

            int idBooking = _faker.Random.Int(1, 1000);
            int idClient = _faker.Random.Int(1, 1000);
            int idRoom = _faker.Random.Int(1, 1000);

            var booking = new Booking
            {
                IdBooking = idBooking,
                IdClient = idClient,
                IdRoom = idRoom,
                Checkin = checkin,
                Checkout = checkout,
            };

            await _bookingRepository.UpdateAsync(booking);

            var room = new Room
            {
                IdRoom = idRoom,
                IsAvaible = false,
                Number = "140"
            };

            string expectedMessage = $"Room {room.Number} unavaible, please choose other.";

            _roomRepository.GetAsync(booking.IdRoom).Returns(room);

            int currentStatusCode = 0;
            string currentMessage = "";

            //Act
            IActionResult? result;

            try
            {
                result = await _bookingController.UpdateAsync(booking);
            }
            catch (CustomException cx)
            {
                currentStatusCode = cx.StatusCode;
                currentMessage = cx.Message;
            }
            catch (Exception ex)
            {
                currentStatusCode = 500;
                currentMessage = ex.Message;
            }

            //Assert    
            Assert.Equal(expectedStatusCode, currentStatusCode);
            Assert.Equal(expectedMessage, currentMessage);
        }

        [Fact(DisplayName = "GetByIdShouldOkResultAndObjectWithValue")]
        public async Task GetByIdShouldOkResultAndObjectWithValue()
        {
            //Arrange            
            int idBooking = _faker.Random.Int(1, 1000);
            int idClient = _faker.Random.Int(1, 1000);
            int idRoom = _faker.Random.Int(1, 1000);
            DateTime checkin = DateTime.Now.AddDays(1);
            DateTime checkout = checkin.AddDays(2);

            var booking = new Booking
            {
                IdBooking = idBooking,
                IdClient = idClient,
                IdRoom = idRoom,
                Checkin = checkin,
                Checkout = checkout,
            };

            _bookingRepository.GetAsync(idBooking).Returns(booking);

            //Act
            var result = await _bookingController.GetAsync(idBooking);

            //Assert
            result.Should().NotBeNull();

            var objectResult = result as OkObjectResult;

            objectResult.Should().BeOfType<OkObjectResult>();

            var objResult = result as ObjectResult;

            var bookingResult = objResult?.Value as Booking;

            Assert.Equal(booking, bookingResult);
        }

        [Fact(DisplayName = "DeleteByIdShouldOkResultAndObjectWithValue")]
        public async Task DeleteByIdShouldOkResultAndObjectWithValue()
        {
            //Arrange            
            int idBooking = _faker.Random.Int(1, 1000);

            await _bookingRepository.DeleteAsync(idBooking);

            //Act
            var result = await _bookingController.DeleteAsync(idBooking);

            //Assert
            result.Should().NotBeNull();

            var objectResult = result as OkResult;

            objectResult.Should().BeOfType<OkResult>();
        }
    }
}
