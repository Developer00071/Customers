using Domain.Models;

namespace Domain.Interfaces;

public interface IOrdersRepository
{
    Task AddOrderAsync(Orders order);
    Task UpdateOrderAsync(Orders order);
    Task DeleteOrderAsync(int orderId);
    Task<Orders?> GetOrderByIdAsync(int orderId);
    Task<List<Orders>> GetOrdersByCustomerIdAsync(int customerId);
    Task<List<Orders>> GetOrdersWithCustomerAsync();
    Task<double> GetTotalOrderAmountAsync();
    Task<List<Customers>> GetOrderCountsByCustomerAsync();
    Task<List<Customers>> GetTopThreeCustomersByOrderAmountAsync();
}
