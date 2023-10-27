using Dapper;
using MyProger.Core.Entity.Company;
using MyProger.Core.Entity.Job;
using MyProger.Micro.CompanyAPI.Database.Interfaces;

namespace MyProger.Micro.CompanyAPI.Database.Repository;

public class CompanyRepository
    : ICompanyRepository
{
    private readonly CompanyDbContext _companyDbContext;

    public CompanyRepository(CompanyDbContext companyDbContext)
    {
        _companyDbContext = companyDbContext;
    }

    public async Task<IEnumerable<CompanyEntity>> GetAllCompanies()
    {
        using var conn = _companyDbContext.CreateConnection();
        conn.Open();
        using var transaction = conn.BeginTransaction();

        try
        {
            var companies = await conn.QueryAsync<CompanyEntity>("SELECT * FROM companies");
            transaction.Commit();
            return companies;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
    
    public async Task<CompanyEntity> GetCompanyByName(string companyName)
    {
        using var conn = _companyDbContext.CreateConnection();
        conn.Open();
        using var transaction = conn.BeginTransaction();

        try
        {
            var company = await conn.QueryFirstOrDefaultAsync<CompanyEntity>("SELECT * FROM companies " +
                                                                          "WHERE CompanyName = @CompanyName",
                new {CompanyName = companyName});
            transaction.Commit();
            return company;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
    
    public async Task<CompanyEntity> CreateCompany(CompanyEntity companyEntity)
    {
        using var conn = _companyDbContext.CreateConnection();
        
        conn.Open();
        
        using var transaction = conn.BeginTransaction();

        try
        {
            var company = await conn.QuerySingleOrDefaultAsync<CompanyEntity>(
            "INSERT INTO companies (NameCompany, ReviewsCount, CreationDate, JobsCount, Description) " +
            "VALUES(@NameCompany, @ReviewsCount, @CreationDate, @JobsCount, @Description)", companyEntity);

            transaction.Commit();
            return company;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
    
    public async Task<CompanyEntity> UpdateCompany(Guid id, CompanyEntity companyEntity)
    {
        using var conn =  _companyDbContext.CreateConnection();

        conn.Open();
        
        using var transaction = conn.BeginTransaction();

        try
        {
            var company =  await conn.QueryFirstOrDefaultAsync<CompanyEntity>("UPDATE companies SET NameCompany = @NameCompany, " +
                                                                        "Description = @Description, " +
                                                                        "CreationDate = @CreationDate, " +
                                                                        "ReviewsCount = @ReviewsCount, " +
                                                                        "JobsCount = @JobsCount," +
                                                                        "WHERE Id = @Id", companyEntity );
            
            transaction.Commit();
            return company;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
    
    public async Task DeleteCompany(Guid id)
    {
        using var conn = _companyDbContext.CreateConnection();
        conn.Open();
        using var transaction = conn.BeginTransaction();

        try
        {
            await conn.QueryFirstOrDefaultAsync<CompanyEntity>("DELETE FROM companies WHERE Id = @Id", new { Id = id });
            transaction.Commit();
        }
        catch (Exception) 
        { 
            transaction.Rollback();
            throw; 
        }
    }

    public async Task<CompanyEntity> GetById(Guid id)
    {
        using var conn = _companyDbContext.CreateConnection();
        conn.Open();
        
        using var transaction = conn.BeginTransaction();

        try
        {
            var company = await conn.QueryFirstOrDefaultAsync<CompanyEntity>("SELECT * FROM companies " +
                                                                             "WHERE Id = @Id", 
                new { Id = id });
            transaction.Commit();
            return company;
        }
        catch (Exception) 
        { 
            transaction.Rollback();
            throw; 
        }
    }
}