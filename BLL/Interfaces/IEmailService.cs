namespace BLL.Interfaces;

public interface IEmailService
{
    Task SendPasswordEmailAsync(string toEmail, string playerName, string password);
}
