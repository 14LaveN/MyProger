namespace MyProger.Micro.JobListAPI.Database.Interfaces;

public interface IUnitOfWork
{
    IJobRepository JobRepository { get; }
}