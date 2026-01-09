using System.Text;
using Newtonsoft.Json;
using RabbitMqModule.Common;
using SmsNotificator.Models;

namespace SmsNotificator.Serializers;

public class SmsSerializer : IRpcMessageSerializer<SendSmsNotificationRequest, SendSmsNotificationResponse>
{
    public byte[] Serialize(SendSmsNotificationRequest message)
    {
        var serializedMessage = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(serializedMessage);

        return body;
    }

    public SendSmsNotificationResponse Deserialize(byte[] body)
    {
        var serializedMessage = Encoding.UTF8.GetString(body);
        var message = JsonConvert.DeserializeObject<SendSmsNotificationResponse>(serializedMessage);

        return message;
    }
}