//Inter-services Communication using gRCP
using Grpc.Net.Client;
using Play.Catalog;
using System.Threading.Tasks;
using Grpc.Core;

namespace Play.Inventory.Service.Clients
{
    public class CatalogClientGrpc
    {
        private readonly Catalog.Catalog.CatalogClient _catalogClient;

        public CatalogClientGrpc(Catalog.Catalog.CatalogClient catalogClient)
        {
            _catalogClient = catalogClient;
        }

        public async Task GetProductFromCatalog(string productId)
        {
            // var request = new ProductRequest { ProductId = "2" };
            // var response = await _catalogClient.GetProductDetailsAsync(request);

            var handler = new HttpClientHandler
            {
                // Bypass SSL certificate validation (only for deployment)
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            var channel = GrpcChannel.ForAddress("https://localhost:5205", new GrpcChannelOptions { HttpHandler = handler });
            var client = new Catalog.Catalog.CatalogClient(channel);

            var response = await client.GetProductDetailsAsync(new ProductRequest { ProductId = "2" });

            Console.WriteLine($"Product ID: {response.ProductId}");
            Console.WriteLine($"Name: {response.Name}");
            Console.WriteLine($"Description: {response.Description}");

            Console.WriteLine();
            Console.WriteLine($"All Products");
            Console.WriteLine();
            
            using (var call = client.GetAllProductDetails(new AllProductRequest()))
            {
                // Read all responses from the server
                //wait for the next items in the list untill there is no item left
                while (await call.ResponseStream.MoveNext())
                {
                    var currentProduct = call.ResponseStream.Current;
                    Console.WriteLine($"Product ID: {currentProduct.ProductId}");
                    Console.WriteLine($"Name: {currentProduct.Name}");
                    Console.WriteLine($"Description: {currentProduct.Description}");
                }
            }

            Console.ReadLine();
        }
    }
}

