using Domain.Entities;
using Domain.Models;

namespace Application.Interfaces
{
    public interface IClientApp
    {
        Task<int> InsertAsync(Client client);
        Task<Client> LoginAsync(LoginModel login);
    }
}
