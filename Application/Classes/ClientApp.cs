using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Helpers;
using Domain.Models;
using Repository.Interfaces;
using System.Net;

namespace Application.Classes
{
    public class ClientApp : IClientApp
    {
        private readonly IClientRepository _clientRepository;

        public ClientApp(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<int> InsertAsync(Client client)
        {
            client.Password = client.Password.EncryptPasswordWithSHA256();

            return await _clientRepository.InsertAsync(client);
        }

        public async Task<Client> LoginAsync(LoginModel login)
        {
            var client = await _clientRepository.GetByEmailAsync(login.Email);

            if (client == null)
                throw new CustomException(HttpStatusCode.Unauthorized, "Access denied.");
            else
            {
                string passwordEncrypted = login.Password.EncryptPasswordWithSHA256();

                if (client.Password.Equals(passwordEncrypted))
                {
                    client.Password = string.Empty;

                    return client;
                }
                else
                    throw new CustomException(HttpStatusCode.Unauthorized, "Access denied.");
            }
        }
    }
}
