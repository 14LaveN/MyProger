namespace MyProger.Core.Entity.Mail;

public class MailMessageEntity
{
    public string Subject { get; set; }
    public string Body { get; set; }
    public List<string> To { get; set; }
}