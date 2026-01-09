using System.Text;
using EmailNotificator.Models;
using Newtonsoft.Json;
using RabbitMqModule.Common;

namespace EmailNotificator.Serializers;

public class EmailSerializer : IRpcMessageSerializer<SendMailNotificationRequest, SendMailNotificationResponse>
{
    public byte[] Serialize(SendMailNotificationRequest message)
    {
        var serializedMessage = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(serializedMessage);

        return body;
    }

    public SendMailNotificationResponse Deserialize(byte[] body)
    {
        var serializedMessage = Encoding.UTF8.GetString(body);
        var message = JsonConvert.DeserializeObject<SendMailNotificationResponse>(serializedMessage);

        return message;
    }
}