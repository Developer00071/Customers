using Domain.Models;

namespace Domain.Interfaces;

public interface ICustomersRepository
{
    Task AddCustomerAsync(Customers customer);
    Task UpdateCustomerAsync(Customers customer);
    Task DeleteCustomerAsync(int customerId);
    Task<Customers?> GetCustomerByIdAsync(int customerId);
    Task<List<Customers>> GetAllCustomersAsync();
}
