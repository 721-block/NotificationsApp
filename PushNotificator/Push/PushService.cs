using PushbulletSharp.Models.Requests;

namespace PushNotificator.Push;

public interface IPushService
{
    Task<SendingResult> Send(PushData pushData);
}

public class PushService(IPushBulletClientFactory pushBulletClientFactory) : IPushService
{
    public Task<SendingResult> Send(PushData pushData)
    {
        var pushClient = pushBulletClientFactory.Create();

        var request = new PushNoteRequest
        {
            Email = pushData.RecipientEmail,
            Title = pushData.Subject,
            Body = pushData.Body
        };

        try
        {
            var response = pushClient.PushNote(request);
            return Task.FromResult(new SendingResult
            {
                IsSent = !response.Dismissed
            });
        }
        catch (Exception e)
        {
            return Task.FromResult(new SendingResult
            {
                IsSent = false,
                Message = e.Message
            });
        }
    }
}

public class SendingResult
{
    public bool IsSent { get; set; }
    public string? Message { get; set; }
}