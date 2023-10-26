using System.Data;
using Npgsql;

namespace MyProger.Micro.JobListAPI.Database;

public class JobDbContext
{
    private readonly string? _connectionString = "Server=localhost;Port=5433;Database=JobListDbDevelopment;User Id=postgres;Password=1111;";

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}