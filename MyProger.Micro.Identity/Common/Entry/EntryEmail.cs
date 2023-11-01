using MyProger.Email;

namespace MyProger.Micro.Identity.Common.Entry;

public static class EntryEmail
{
    public static IServiceCollection AddEmail(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSingleton<System.String>();
        services.AddTransient<EmailService>(provider => new EmailService("smtp.gmail.com", 587, "Ponomareff.55555@gmail.com", "Sasha_2008!"));
        
        return services;
    }
}