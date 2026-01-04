namespace RabbitMqModule.RpcClient;

public interface IRpcClient<in TRequestMessage, TResponseMessage>
{
    Task Start();
    Task<TResponseMessage> Call(TRequestMessage requestMessage, string? routingKey = null);
}