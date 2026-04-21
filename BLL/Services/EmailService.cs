using BLL.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace BLL.Services;

public class EmailService : IEmailService
{
    private readonly string smtpServer = "smtp.gmail.com";
    private readonly int smtpPort = 587;
    private readonly string smtpUser = "tonemail@gmail.com";
    private readonly string smtpPass = "tonMotDePasseOuAppPassword";

    public async Task SendPasswordEmailAsync(string toEmail, string playerName, string password)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Tournoi d'échecs", smtpUser));
        message.To.Add(new MailboxAddress(playerName, toEmail));
        message.Subject = "Votre mot de passe";

        message.Body = new TextPart("plain")
        {
            Text = $"Bonjour {playerName},\n\nVoici votre mot de passe : {password}\n\nBonne chance pour le tournoi !"
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(smtpUser, smtpPass);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

}
