using api.Models;
using api.Repositories;

namespace api.Services
{
    public class ClientService: IClientService
    {
        private readonly IClientRepository clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public async Task CreateClient(Client client)
        {
            await clientRepository.Create(client);
        }

        public Task UpdateClient(Client client)
        {
            return clientRepository.Update(client);
        }

        public Task<Client[]> GetClients()
        {
            return clientRepository.Get();
        }

        public Task<Client[]> SearchClients(string term)
        {
            return clientRepository.SearchClients(term);
        }
    }
}
