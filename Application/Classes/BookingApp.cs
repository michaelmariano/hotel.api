using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Repository.Interfaces;
using System.Net;

namespace Application.Classes
{
    public class BookingApp : IBookingApp
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomApp _roomApp;

        public BookingApp(IBookingRepository bookingRepository, IRoomApp roomApp)
        {
            _bookingRepository = bookingRepository;
            _roomApp = roomApp;
        }

        public async Task DeleteAsync(int id)
        {
            await _bookingRepository.DeleteAsync(id);
        }

        public async Task<Booking?> GetAsync(int id)
        {
            return await _bookingRepository.GetAsync(id);
        }

        public async Task<int> InsertAsync(Booking booking)
        {
            await CheckStay(booking);

            return await _bookingRepository.InsertAsync(booking);
        }

        public async Task UpdateAsync(Booking booking)
        {
            await CheckStay(booking);

            await _bookingRepository.UpdateAsync(booking);
        }

        private async Task CheckStay(Booking booking)
        {
            var room = await _roomApp.GetAsync(booking.IdRoom);

            if (room == null)
                throw new CustomException(HttpStatusCode.UnprocessableEntity, "Room not founded.");
            else if (!room.IsAvaible)
                throw new CustomException(HttpStatusCode.UnprocessableEntity, $"Room {room.Number} unavaible, please choose other.");

            int stayDays = Math.Abs((booking.Checkout - booking.Checkin).Days);
            int advanceDays = Math.Abs((booking.Checkin - DateTime.Now).Days);

            if (booking.Checkin.Date == DateTime.Today)
                throw new CustomException(HttpStatusCode.UnprocessableEntity, "Checkin date cannot be today.");
            else if (advanceDays > 30)
                throw new CustomException(HttpStatusCode.UnprocessableEntity, "Booking cannot be made more than 30 days in advance.");
            else if (stayDays > 3)
                throw new CustomException(HttpStatusCode.UnprocessableEntity, "The stay cannot exceed 3 days.");
            else if(booking.Checkin > booking.Checkout)
                throw new CustomException(HttpStatusCode.UnprocessableEntity, "Checkin cannot be greater than checkout.");
        }
    }
}