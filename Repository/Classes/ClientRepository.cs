using Domain.Entities;
using Repository.Abstractions;
using Repository.Interfaces;
using System.Data;

namespace Repository.Classes
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public async Task<int> Insert(Client client)
        {
            _sqlCommand = $"insert into {Table} (fullname, document, phonenumber, email, password) " +
                $"values (@fullname, @document, @phonenumber, @email, @password) " +
                $"returning idclient;";

            return await WriteAsync(_sqlCommand, client, commandType: CommandType.Text);
        }

        public async Task<Client> GetByEmailAsync(string email)
        {
            _params.Add("@email", email, DbType.String, direction: ParameterDirection.Input);

            _sqlCommand = $"select idclient, fullname, document, phonenumber, email, password from {Table} " +
                          "where email = @email;";

            var result = (await ReadAsync<Client>(_sqlCommand, param: _params, commandType: CommandType.Text)).FirstOrDefault();

            return result;
        }
    }
}
