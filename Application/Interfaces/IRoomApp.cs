using Domain.Entities;

namespace Application.Interfaces
{
    public interface IRoomApp
    {
        Task<int> InsertAsync(Room room);
        Task UpdateAsync(Room room);
        Task DeleteAsync(int id);
        Task<Room?> GetAsync(int id);
        Task<List<Room>> GetAllAsync();
    }
}
