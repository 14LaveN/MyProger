namespace MyProger.Micro.CompanyAPI.Database.Interfaces;

public interface IUnitOfWork
{
    ICompanyRepository CompanyRepository { get; }
}