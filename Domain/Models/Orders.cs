namespace Domain.Models;

public class Orders
{
    public int Id {get; set;}
    public int Customer_Id {get; set;}
    public string? CustomerName { get; set; }
    public double Amount {get; set;}
    public DateOnly Created_At {get; set;}
}
