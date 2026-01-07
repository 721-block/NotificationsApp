using System.Text;
using Newtonsoft.Json;
using PushNotificator.Models;
using RabbitMqModule.Common;

namespace PushNotificator.Serializers;

public class PushSerializer : IRpcMessageSerializer<SendPushNotificationResponse, SendPushNotificationRequest>
{
    public byte[] Serialize(SendPushNotificationResponse message)
    {
        var serializedMessage = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(serializedMessage);

        return body;
    }

    public SendPushNotificationRequest Deserialize(byte[] body)
    {
        var serializedMessage = Encoding.UTF8.GetString(body);
        var message = JsonConvert.DeserializeObject<SendPushNotificationRequest>(serializedMessage);

        return message;
    }
}