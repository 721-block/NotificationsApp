using MimeKit;

namespace EmailNotificator;

public class MessageBuilder
{
    private Sender? sender;
    private Recipient? recipient;
    private Content? content;

    private MessageBuilder()
    {
    }

    public static MessageBuilder New() => new();

    public MessageBuilder SetSender(string displayName, string address)
    {
        sender = new Sender(displayName, address);

        return this;
    }

    public MessageBuilder SetRecipient(params string[] addresses)
    {
        recipient = new Recipient(addresses);

        return this;
    }

    public MessageBuilder SetContent(string subject, string body)
    {
        content = new Content(subject, body);

        return this;
    }

    public MimeMessage Build()
    {
        var message = new MimeMessage();

        if (sender != null)
            message.Sender = new MailboxAddress(sender.DisplayName, sender.Address);

        if (recipient != null)
        {
            var mailboxAddresses = recipient.Addresses.Select(MailboxAddress.Parse);
            message.To.AddRange(mailboxAddresses);
        }

        if (content != null)
        {
            message.Subject = content.Subject;
            message.Body = new BodyBuilder{ HtmlBody = content.Body }.ToMessageBody();
        }

        return message;
    }

    private record Sender(string DisplayName, string Address);
    private record Recipient(string[] Addresses);

    private record Content(string Subject, string Body);
}