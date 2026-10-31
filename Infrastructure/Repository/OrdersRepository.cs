using Dapper;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repository;

public class OrdersRepository : IOrdersRepository
{
    private readonly AppDbContext _dbContext;

    public OrdersRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddOrderAsync(Orders order)
    {
        var query = "INSERT INTO orders (customer_id, amount, created_at) VALUES (@Customer_Id, @Amount, @Created_At);";

        using var connection = _dbContext.Connection();
        await connection.ExecuteAsync(query, order);
    }

    public async Task UpdateOrderAsync(Orders order)
    {
        var query = "UPDATE orders SET customer_id = @Customer_Id, amount = @Amount, created_at = @Created_At WHERE id = @Id;";

        using var connection = _dbContext.Connection();
        await connection.ExecuteAsync(query, order);
    }

    public async Task DeleteOrderAsync(int orderId)
    {
        var query = "DELETE FROM orders WHERE id = @OrderId;";

        using var connection = _dbContext.Connection();
        await connection.ExecuteAsync(query, new { OrderId = orderId });
    }

    public async Task<Orders?> GetOrderByIdAsync(int orderId)
    {
        var query = "SELECT id , customer_id , amount , created_at  FROM orders WHERE id = @OrderId;";

        using var connection = _dbContext.Connection();
        var result =  await connection.QuerySingleOrDefaultAsync<Orders>(query, new { OrderId = orderId });
        return result;
    }

    public async Task<List<Orders>> GetOrdersByCustomerIdAsync(int customerId)
    {
        var query = "SELECT id, customer_id , amount , created_at FROM orders WHERE customer_id = @CustomerId ORDER BY id;";

        using var connection = _dbContext.Connection();
        var result = await connection.QueryAsync<Orders>(query, new { CustomerId = customerId });
        return result.ToList();
    }

    public async Task<List<Orders>> GetOrdersWithCustomerAsync()
    {
        const string query = """
            SELECT
                o.id AS "Id",
                c.fullname AS "CustomerName",
                o.amount AS "Amount",
                o.created_at AS "Created_At"
            FROM orders o
            INNER JOIN customers c ON c.id = o.customer_id
            ORDER BY o.id;
            """;

        using var connection = _dbContext.Connection();
        var result = await connection.QueryAsync<Orders>(query);
        return result.ToList();
    }

    public async Task<double> GetTotalOrderAmountAsync()
    {
        const string query = "SELECT COALESCE(SUM(amount), 0) FROM orders;";

        using var connection = _dbContext.Connection();
        return await connection.ExecuteScalarAsync<double>(query);
    }

    public async Task<List<Customers>> GetOrderCountsByCustomerAsync()
    {
        const string query = """
            SELECT
                c.id AS "Id",
                c.fullname AS "FullName",
                COUNT(o.id) AS "OrderCount"
            FROM customers c
            LEFT JOIN orders o ON o.customer_id = c.id
            GROUP BY c.id, c.fullname
            ORDER BY c.id;
            """;

        using var connection = _dbContext.Connection();
        var result = await connection.QueryAsync<Customers>(query);
        return result.ToList();
    }

    public async Task<List<Customers>> GetTopThreeCustomersByOrderAmountAsync()
    {
        const string query = """
            SELECT
                c.id AS "Id",
                c.fullname AS "FullName",
                SUM(o.amount) AS "TotalAmount"
            FROM customers c
            INNER JOIN orders o ON o.customer_id = c.id
            GROUP BY c.id, c.fullname
            ORDER BY SUM(o.amount) DESC, c.id
            LIMIT 3;
            """;

        using var connection = _dbContext.Connection();
        var result = await connection.QueryAsync<Customers>(query);
        return result.ToList();
    }
}
