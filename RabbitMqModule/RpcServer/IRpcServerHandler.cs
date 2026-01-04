namespace RabbitMqModule.RpcServer;

public interface IRpcServerHandler<in TRequestMessage, TResponseMessage>
{
    Task<ResponseData<TResponseMessage>> Handle(TRequestMessage requestMessage, ResponseData<TResponseMessage> responseData);
}