//Inter-services Communication using REST and HTTP
using Play.Inventory.Service.Dtos;

namespace Play.Inventory.Service.Clients
{
    public class CatalogClient{

        private readonly HttpClient httpClient;

        //Dependency injection 
        public CatalogClient(HttpClient httpClient){
            this.httpClient= httpClient;
        }

        public async Task<IReadOnlyCollection<CatalogItemDto>> GetCatalogItemsAsync(){
            var items = await httpClient.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDto>>("/items");
            return items;
        }

    }
}