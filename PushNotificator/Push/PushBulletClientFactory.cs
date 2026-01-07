using PushbulletSharp;

namespace PushNotificator.Push;

public interface IPushBulletClientFactory
{
    public PushbulletClient Create();
}

public class PushBulletClientFactory(PushSettings pushSettings) : IPushBulletClientFactory
{
    public PushbulletClient Create() => new(pushSettings.ApiKey);
}

public class PushSettings
{
    public string ApiKey { get; set; }
}