using System.Text;
using EmailNotificator.Models;
using Newtonsoft.Json;
using RabbitMqModule.Common;

namespace EmailNotificator.Serializers;

public class EmailSerializer : IRpcMessageSerializer<SendNotificationResponse, SendNotificationRequest>
{
    public byte[] Serialize(SendNotificationResponse message)
    {
        var serializedMessage = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(serializedMessage);

        return body;
    }

    public SendNotificationRequest Deserialize(byte[] body)
    {
        var serializedMessage = Encoding.UTF8.GetString(body);
        var message = JsonConvert.DeserializeObject<SendNotificationRequest>(serializedMessage);

        return message;
    }
}