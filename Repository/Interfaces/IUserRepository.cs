using Domain.Entities;

namespace Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
    }
}
