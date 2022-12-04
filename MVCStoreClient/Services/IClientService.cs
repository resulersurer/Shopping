using MVCStoreClient.Models;
using Newtonsoft.Json;

namespace MVCStoreClient.Services
{
    public interface IClientService
    {
        Task<IEnumerable<Rayon>> GetRayons();
        Task<Rayon> GetRayon(Guid id);
        Task<Category> GetCategory(Guid id);

    }

    public class ClientService : IClientService
    {
        public Task<Category> GetCategory(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Rayon> GetRayon(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Rayon>> GetRayons()
        {
            using var httpClient = new HttpClient();
            using var response = await  httpClient.GetAsync("http://localhost:5196/api/Client/rayons");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Rayon>>(responseBody);
            return result;
        }
    }
}
