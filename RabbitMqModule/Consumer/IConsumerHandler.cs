namespace RabbitMqModule.Consumer;

public interface IConsumerHandler<TRequestMessage, TResponseMessage>
{
    Task<ResponseData<TResponseMessage>> Handle(TRequestMessage requestMessage, ResponseData<TResponseMessage> responseData);
}