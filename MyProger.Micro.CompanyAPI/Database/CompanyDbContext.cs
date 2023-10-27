using System.Data;
using Npgsql;

namespace MyProger.Micro.CompanyAPI.Database;

public class CompanyDbContext
{
    private readonly string? _connectionString = "Server=localhost;Port=5433;Database=CompanyDbDevelopment;User Id=postgres;Password=1111;";

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}