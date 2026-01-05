using MailKit.Net.Smtp;
using MailKit.Security;

namespace EmailNotificator;

public interface IMailService
{
    Task<bool> Send(MailData mailData);
}

public class MailService(MailSettings settings) : IMailService
{
    public async Task<bool> Send(MailData mailData)
    {
        var message = MessageBuilder
            .New()
            .SetSender(settings.SenderDisplayName, settings.SenderAddress)
            .SetRecipient(mailData.RecipientAddresses)
            .SetContent(mailData.Subject, mailData.Body)
            .Build();

        using var client = new SmtpClient();
        try
        {
            await client
                .ConnectAsync(settings.Host, settings.Port, SecureSocketOptions.SslOnConnect)
                .ConfigureAwait(false);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(settings.UserName, settings.Password).ConfigureAwait(false);

            await client.SendAsync(message).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            return false;
        }
        finally
        {
            await client.DisconnectAsync(true).ConfigureAwait(false);
        }

        return true;
    }
}