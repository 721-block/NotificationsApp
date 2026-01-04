namespace RabbitMqModule;

public interface IRpcSerializer<out TRequestMessage, in TResponseMessage>
{
    byte[] Serialize(TResponseMessage message);
    TRequestMessage Deserialize(byte[] body);
}