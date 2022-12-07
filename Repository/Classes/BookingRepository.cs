using Domain.Entities;
using Repository.Abstractions;
using Repository.Interfaces;
using System.Data;

namespace Repository.Classes
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        public async Task DeleteAsync(int id)
        {
            _sqlCommand = $"delete from {Table} where idbooking = @idbooking;";

            _params.Add("@id", id, DbType.Int32, direction: ParameterDirection.Input);

            await WriteJustOneRecordAsync(_sqlCommand, param: _params, commandType: CommandType.Text);
        }

        public async Task<Booking?> GetAsync(int id)
        {
            _params.Add("@id", id, DbType.Int32, direction: ParameterDirection.Input);

            _sqlCommand = $"select idbooking, idroom, idclient, checkin, checkout from {Table} " +
                          "where idbooking = @id;";

            return (await ReadAsync<Booking>(_sqlCommand, param: _params, commandType: CommandType.Text))?.FirstOrDefault();
        }

        public async Task<int> InsertAsync(Booking booking)
        {
            _sqlCommand = $"insert into {Table} (idroom, idclient, checkin, checkout) " +
               $"values (@idroom, @idclient, @checkin, @checkout) " +
               $"returning idbooking;";

            return await WriteAsync(_sqlCommand, booking, commandType: CommandType.Text);
        }

        public async Task UpdateAsync(Booking booking)
        {
            _sqlCommand = $"update {Table} set idroom = @idroom, idclient = @idclient, checkin = @checkin, checkout = @checkout " +  
               $"where idbooking = @idbooking;";               

            await WriteJustOneRecordAsync(_sqlCommand, booking, commandType: CommandType.Text);
        }
    }
}
