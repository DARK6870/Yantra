namespace Yantra.Mongo.Models;

public class OrderItem
{
    public required string ItemName { get; set; }
    
    public int Quantity { get; set; } = 1;
    
    public decimal? Price { get; set; }
}