using System.Text;
using EmailNotificator.Models;
using Newtonsoft.Json;
using RabbitMqModule.Common;

namespace EmailNotificator.Serializers;

public class EmailSerializer : IRpcMessageSerializer<SendMailNotificationResponse, SendMailNotificationRequest>
{
    public byte[] Serialize(SendMailNotificationResponse message)
    {
        var serializedMessage = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(serializedMessage);

        return body;
    }

    public SendMailNotificationRequest Deserialize(byte[] body)
    {
        var serializedMessage = Encoding.UTF8.GetString(body);
        var message = JsonConvert.DeserializeObject<SendMailNotificationRequest>(serializedMessage);

        return message;
    }
}