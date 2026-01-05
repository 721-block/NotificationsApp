namespace RabbitMqModule.Common;

public interface IRpcMessageSerializer<in TSentMessage, out TReceivedMessage>
{
    byte[] Serialize(TSentMessage message);
    TReceivedMessage Deserialize(byte[] body);
}