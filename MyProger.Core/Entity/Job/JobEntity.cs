namespace MyProger.Core.Entity.Job;

public class JobEntity
{
    public Guid Id { get; set; }

    public string CompanyName { get; set; } = null!;

    public int ViewsCount { get; set; }

    public string Description { get; set; } = null!;

    public bool IsClosed { get; set; }

    public string Title { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public int LikesCount { get; set; }

    public string Wage { get; set; } = null!; //! Decimal to string

    public string PhoneNumber { get; set; } = null!;
}