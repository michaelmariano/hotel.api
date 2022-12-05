using Domain.Entities;
using Domain.Models;

namespace Application.Interfaces
{
    public interface IUserApp
    {
        Task<int> Login(LoginModel login);
    }
}
