using Dapper;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repository;

public class CustomersRepository : ICustomersRepository
{
    private readonly AppDbContext _dbContext;

    public CustomersRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddCustomerAsync(Customers customer)
    {
        const string query = "INSERT INTO customers (fullname) VALUES (@FullName);";

        using var connection = _dbContext.Connection();
        await connection.ExecuteAsync(query, customer);
    }

    public async Task DeleteCustomerAsync(int customerId)
    {
        const string query = "DELETE FROM customers WHERE id = @CustomerId;";

        using var connection = _dbContext.Connection();
        await connection.ExecuteAsync(query, new { CustomerId = customerId });
    }

    public async Task<List<Customers>> GetAllCustomersAsync()
    {
        const string query = "SELECT id , fullname  FROM customers ORDER BY id;";

        using var connection = _dbContext.Connection();
        var result = await connection.QueryAsync<Customers>(query);
        return result.ToList();
    }

    public async Task<Customers?> GetCustomerByIdAsync(int customerId)
    {
        const string query = "SELECT id , fullname  FROM customers WHERE id = @CustomerId;";

        using var connection = _dbContext.Connection();
        return await connection.QuerySingleOrDefaultAsync<Customers>(query, new { CustomerId = customerId });
    }

    public async Task UpdateCustomerAsync(Customers customer)
    {
        const string query = "UPDATE customers SET fullname = @FullName WHERE id = @Id;";

        using var connection = _dbContext.Connection();
        await connection.ExecuteAsync(query, customer);
    }
}

