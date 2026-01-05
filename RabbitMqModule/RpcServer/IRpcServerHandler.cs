namespace RabbitMqModule.RpcServer;

public interface IRpcServerHandler<in TRequestMessage, TResponseMessage>
{
    Task<TResponseMessage> Handle(TRequestMessage requestMessage);
}