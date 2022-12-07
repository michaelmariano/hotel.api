using Application.Interfaces;
using Domain.Entities;
using Repository.Interfaces;

namespace Application.Classes
{
    public class RoomApp : IRoomApp
    {
        private readonly IRoomRepository _roomRepository;

        public RoomApp(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task DeleteAsync(int id)
        {
            await _roomRepository.DeleteAsync(id);
        }

        public async Task<List<Room>> GetAllAsync()
        {
            return await _roomRepository.GetAllAsync();
        }

        public async Task<Room?> GetAsync(int id)
        {
            return await _roomRepository.GetAsync(id);
        }

        public async Task<int> InsertAsync(Room room)
        {
            return await _roomRepository.InsertAsync(room);
        }

        public async Task UpdateAsync(Room room)
        {
            await _roomRepository.UpdateAsync(room);
        }
    }
}
