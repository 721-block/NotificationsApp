using System.Text;
using Newtonsoft.Json;
using PushNotificator.Models;
using RabbitMqModule.Common;

namespace PushNotificator.Serializers;

public class PushSerializer : IRpcMessageSerializer<SendPushNotificationRequest, SendPushNotificationResponse>
{
    public byte[] Serialize(SendPushNotificationRequest message)
    {
        var serializedMessage = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(serializedMessage);

        return body;
    }

    public SendPushNotificationResponse Deserialize(byte[] body)
    {
        var serializedMessage = Encoding.UTF8.GetString(body);
        var message = JsonConvert.DeserializeObject<SendPushNotificationResponse>(serializedMessage);

        return message;
    }
}