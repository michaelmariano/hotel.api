using Domain.Entities;

namespace Application.Interfaces
{
    public interface IBookingApp
    {
        Task<int> InsertAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(int id);
        Task<Booking?> GetAsync(int id);
    }
}
