using Domain.Entities;
using Repository.Abstractions;
using Repository.Interfaces;
using System.Data;

namespace Repository.Classes
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        public async Task DeleteAsync(int id)
        {
            _sqlCommand = $"delete from {Table} where idroom = @idroom;";

            _params.Add("@idroom", id, DbType.Int32, direction: ParameterDirection.Input);

            await WriteJustOneRecordAsync(_sqlCommand, param: _params, commandType: CommandType.Text);
        }

        public async Task<Room?> GetAsync(int id)
        {
            _params.Add("@id", id, DbType.Int32, direction: ParameterDirection.Input);

            _sqlCommand = $"select idroom, number, isavaible from {Table} " +
                          "where idroom = @id;";

            return (await ReadAsync<Room>(_sqlCommand, param: _params, commandType: CommandType.Text))?.FirstOrDefault();
        }

        public async Task<int> InsertAsync(Room room)
        {
            _sqlCommand = $"insert into {Table} (number, isavaible) " +
               $"values (@number, @isavaible) " +
               $"returning idroom;";

            return await WriteAsync(_sqlCommand, room, commandType: CommandType.Text);
        }

        public async Task UpdateAsync(Room room)
        {
            _sqlCommand = $"update {Table} set number = @number, isavaible = @isavaible " +               
               $"where idroom = @idroom;";

            await WriteJustOneRecordAsync(_sqlCommand, room, commandType: CommandType.Text);
        }

        public async Task<List<Room>> GetAllAsync()
        {
            _sqlCommand = $"select idroom, number, isavaible from {Table};";                          

            return (await ReadAsync<Room>(_sqlCommand, param: _params, commandType: CommandType.Text)).ToList();
        }
    }
}
