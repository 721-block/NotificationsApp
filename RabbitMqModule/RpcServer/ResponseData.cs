namespace RabbitMqModule.RpcServer;

public class ResponseData<TMessage>
{
    public TMessage Message { get; set; }
}