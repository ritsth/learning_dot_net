namespace Play.Catalog.Service.Settings
{
    public class RabbitMQSettings
    {
        //using inti; instead of set; since we don't want to change the value after initialization
        public string HostName { get; init; }
    }
}