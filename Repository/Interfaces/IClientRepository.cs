﻿using Domain.Entities;

namespace Repository.Interfaces
{
    public interface IClientRepository
    {
        Task<int> InsertAsync(Client client);
        Task<Client> GetByEmailAsync(string email);
    }
}
