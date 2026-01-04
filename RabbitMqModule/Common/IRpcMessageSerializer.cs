namespace RabbitMqModule.Common;

public interface IRpcMessageSerializer<in TSentMessage, out TReplyMessage>
{
    byte[] Serialize(TSentMessage message);
    TReplyMessage Deserialize(byte[] body);
}