namespace BuildingBlocks.Messaging.Messages;

public class ProductNameChangedMessage
{
    public string ProductId { get; set; }
    public string UpdatedName { get; set; }
}
