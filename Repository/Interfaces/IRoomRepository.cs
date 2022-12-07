using Domain.Entities;

namespace Repository.Interfaces
{
    public interface IRoomRepository
    {
        Task<int> InsertAsync(Room room);
        Task UpdateAsync(Room room);
        Task DeleteAsync(int id);
        Task<Room?> GetAsync(int id);
        Task<List<Room>> GetAllAsync();
    }
}