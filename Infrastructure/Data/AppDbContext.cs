using Npgsql;

namespace Infrastructure.Data;

public class AppDbContext
{
    private string connectionString = "Host=localhost;Port=5432;Database=Customers;Username=postgres;Password=0510";
    public NpgsqlConnection Connection() => new(connectionString);
}
