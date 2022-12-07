using Domain.Entities;

namespace Repository.Interfaces
{
    public interface IClientRepository
    {
        Task<int> Insert(Client client);
        Task<Client> GetByEmailAsync(string email);
    }
}
