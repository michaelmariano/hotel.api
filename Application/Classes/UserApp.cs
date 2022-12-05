using Application.Interfaces;
using Domain.Exceptions;
using Domain.Helpers;
using Domain.Models;
using Repository.Interfaces;
using System.Net;

namespace Application.Classes
{
    public class UserApp : IUserApp
    {
        private readonly IUserRepository _userRepository;

        public UserApp(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> Login(LoginModel login)
        {
            var user = await _userRepository.GetByEmailAsync(login.Email);

            if (user == null)
                throw new CustomException(HttpStatusCode.Unauthorized, "Access denied.");
            else
            {
                string passwordEncrypted = login.Password.EncryptPasswordWithSHA256();

                if (user.Password.Equals(passwordEncrypted))
                    return user.IdUser;
                else
                    throw new CustomException(HttpStatusCode.Unauthorized, "Access denied.");
            }
        }
    }
}
