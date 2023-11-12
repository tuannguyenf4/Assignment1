using api.Models;

namespace api.Services
{
    public interface IClientService
    {
        Task CreateClient(Client client);
        Task UpdateClient(Client client);
        Task<Client[]> GetClients();
        Task<Client[]> SearchClients(string term);
    }
}
