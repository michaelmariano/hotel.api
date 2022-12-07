using Application.Classes;
using Application.Interfaces;
using Bogus;
using Domain.Entities;
using FluentAssertions;
using Hotel.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Repository.Interfaces;

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

            var booking = new Booking
            {
                IdBooking = idBooking,
                IdClient = idClient,
                IdRoom = idRoom,
                Checkin = DateTime.Now.AddDays(1),
                Checkout = DateTime.Now.AddDays(3),
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
            var result = await _bookingController.Insert(booking);

            //Assert
            result.Should().NotBeNull();

            var objectResult = result as CreatedResult;

            objectResult.Should().BeOfType<CreatedResult>();

            var objResult = result as ObjectResult;

            int idBookingCurrent = Convert.ToInt32(objResult?.Value);

            Assert.Equal(idBooking, idBookingCurrent);
        }
    }
}
