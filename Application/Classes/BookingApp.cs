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

        public BookingApp(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
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
            CheckStay(booking);

            return await _bookingRepository.InsertAsync(booking);
        }

        public async Task UpdateAsync(Booking booking)
        {
            CheckStay(booking);

            await _bookingRepository.UpdateAsync(booking);
        }

        private void CheckStay(Booking booking)
        {
            int stayDays = Math.Abs((booking.Checkout - booking.Checkin).Days);            
            int advanceDays = Math.Abs((booking.Checkin - DateTime.Now).Days);

            if (booking.Checkin.Date == DateTime.Today)
                throw new CustomException(HttpStatusCode.UnprocessableEntity, "Checkin date cannot be today.");
            else if (advanceDays > 30)
                throw new CustomException(HttpStatusCode.UnprocessableEntity, "Booking cannot be made more than 30 days in advance.");
            else if (stayDays > 3)
                throw new CustomException(HttpStatusCode.UnprocessableEntity, "The stay cannot exceed 3 days.");
        }
    }
}