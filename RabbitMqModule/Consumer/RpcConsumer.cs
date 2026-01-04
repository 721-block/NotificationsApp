using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqModule.Consumer;

public class RpcConsumer<TRequestMessage, TResponseMessage>(
    IConsumerSettings consumerSettings,
    IRabbitMqConnectionProvider connectionProvider,
    IRpcSerializer<TRequestMessage, TResponseMessage> rpcSerializer,
    IEnumerable<IConsumerHandler<TRequestMessage, TResponseMessage>> consumerHandlers
    ) : IConsumer
{
    private IChannel channel;

    public async Task Start()
    {
        var connection = await connectionProvider.Get().ConfigureAwait(false);
        channel = await connection.CreateChannelAsync().ConfigureAwait(false);

        await channel.ExchangeDeclareAsync(consumerSettings.ExchangeName, ExchangeType.Direct).ConfigureAwait(false);
        await channel.QueueDeclareAsync(consumerSettings.QueueName).ConfigureAwait(false);
        await channel.BasicQosAsync(0, 1, false).ConfigureAwait(false);
        foreach (var routingKey in consumerSettings.RoutingKeys)
            await channel.QueueBindAsync(consumerSettings.QueueName, consumerSettings.ExchangeName, routingKey)
                .ConfigureAwait(false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var requestMessage = rpcSerializer.Deserialize(body);

            var responseData = new ResponseData<TResponseMessage>();
            foreach (var handler in consumerHandlers)
                responseData = await handler.Handle(requestMessage, responseData).ConfigureAwait(false);

            var props = ea.BasicProperties;
            var replyProps = new BasicProperties
            {
                CorrelationId = props.CorrelationId,
            };
            var response = rpcSerializer.Serialize(responseData.Message);

            await channel.BasicPublishAsync(string.Empty, props.ReplyTo!, mandatory: true, replyProps, response)
                .ConfigureAwait(false);
            await channel.BasicAckAsync(ea.DeliveryTag, false);
        };
    }
}

public interface IConsumerSettings
{
    string ExchangeName { get; set; }
    string QueueName { get; set; }
    string[] RoutingKeys { get; set; }
}