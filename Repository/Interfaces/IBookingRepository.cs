using Domain.Entities;

namespace Repository.Interfaces
{
    public interface IBookingRepository
    {
        Task<int> InsertAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task DeleteAsync(int id);
        Task<Booking?> GetAsync(int id);
    }
}
