using Domain.Entities;
using Domain.Models;

namespace Application.Interfaces
{
    public interface IClientApp
    {
        Task<int> Insert(Client client);
        Task<Client> Login(LoginModel login);
    }
}
