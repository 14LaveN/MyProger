namespace MyProger.Core.Entity.Company;

public class CompanyEntity
{
    public Guid Id { get; set; }

    public string NameCompany { get; set; } = null!;
    
    public string Description { get; set; } = null!;

    public int ReviewsCount { get; set; }

    public DateTime CreationDate { get; set; }

    public int JobsCount { get; set; }
}