namespace Domain.Models;

public class Customers
{
    public int Id {get; set;}
    public string ? FullName {get;set;}
    public long OrderCount { get; set; }
    public double TotalAmount { get; set; }
}
