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


            var request = new ProductRequest { ProductId = "2" };
            var response = await _catalogClient.GetProductDetailsAsync(request);
            // var r= new Catalog.CatalogClient.GetProductDetailsAsync(request);

            Console.WriteLine($"Product ID: {response.ProductId}");
            Console.WriteLine($"Name: {response.Name}");
            Console.WriteLine($"Description: {response.Description}");

            Console.ReadLine();
        }
    }
}

