// //Inter-services Communication using REST and HTTP
// using Play.Inventory.Service.Dtos;

// namespace Play.Inventory.Service.Clients
// {
//     public class CatalogClient{

//         private readonly HttpClient httpClient;

//         //Dependency injection 
//         public CatalogClient(HttpClient httpClient){
//             this.httpClient= httpClient;
//         }

//         public async Task<IReadOnlyCollection<CatalogItemDto>> GetCatalogItemsAsync(){
//             var items = await httpClient.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDto>>("/items");
//             return items;
//         }

//     }
// }

//Inter-services Communication using gRCP
using Grpc.Net.Client;
using Play.Catalog;
using System.Threading.Tasks;

namespace Play.Inventory.Service.Clients
{
    public class CatalogClientI
    {
        private readonly Catalog.Catalog.CatalogClient _catalogClient;

        public CatalogClientI(Catalog.Catalog.CatalogClient catalogClient)
        {
            _catalogClient = catalogClient;
        }

        public async Task GetProductFromCatalog(string productId)
        {


            var request = new ProductRequest { ProductId = "2"};
            var response = new Catalog.CatalogClient.GetProductDetails(request);

            Console.WriteLine($"Product ID: {response.ProductId}");
            Console.WriteLine($"Name: {response.Name}");
            Console.WriteLine($"Description: {response.Description}");
        }
    }    
}

