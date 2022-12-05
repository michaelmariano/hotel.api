using Domain.Entities;
using Repository.Abstractions;
using Repository.Interfaces;
using System.Data;

namespace Repository.Classes
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {  
        public async Task<User> GetByEmailAsync(string email)
        {
            _params.Add("@email", email, DbType.String, direction: ParameterDirection.Input);

            _sqlCommand = $"select email, iduser, password from {Table} " +
                          "where email = @email;";

            var result = (await QueryAsync<User>(_sqlCommand, param: _params, commandType: CommandType.Text)).FirstOrDefault();

            return result;
        }
    }
}
