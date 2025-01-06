namespace Play.Catalog.Service.Entities
{
    public class Item{

        public Guid Id{get; set;}

        public required string Name{get; set;}

        public required string Description{get; set;}

        public decimal Price{get; set;}

        public DateTimeOffset CreatedDate{get;set;}
    }
}