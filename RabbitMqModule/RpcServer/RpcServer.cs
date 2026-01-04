using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqModule.Common;

namespace RabbitMqModule.RpcServer;

public class RpcServer<TRequestMessage, TResponseMessage>(
    IConsumerSettings consumerSettings,
    IRabbitMqConnectionProvider connectionProvider,
    IRpcMessageSerializer<TResponseMessage, TRequestMessage> rpcMessageSerializer,
    IEnumerable<IRpcServerHandler<TRequestMessage, TResponseMessage>> consumerHandlers
    ) : IRpcServer
{
    private IChannel channel;

    public async Task Start()
    {
        var connection = await connectionProvider.Get().ConfigureAwait(false);
        channel = await ChannelBuilder
            .New(connection)
            .AddExchange(consumerSettings.ExchangeName, ExchangeType.Direct)
            .DeclareQueue(consumerSettings.QueueName)
            .AddQos()
            .BindQueue(consumerSettings.RoutingKeys)
            .Build()
            .ConfigureAwait(false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = ea.Body.ToArray();
            var requestMessage = rpcMessageSerializer.Deserialize(body);

            var responseData = new ResponseData<TResponseMessage>();
            foreach (var handler in consumerHandlers)
                responseData = await handler.Handle(requestMessage, responseData).ConfigureAwait(false);

            var props = ea.BasicProperties;
            var replyProps = new BasicProperties
            {
                CorrelationId = props.CorrelationId,
            };
            var response = rpcMessageSerializer.Serialize(responseData.Message);

            await channel.BasicPublishAsync(string.Empty, props.ReplyTo!, mandatory: true, replyProps, response)
                .ConfigureAwait(false);
            await channel.BasicAckAsync(ea.DeliveryTag, false).ConfigureAwait(false);
        };

        await channel.BasicConsumeAsync(consumerSettings.QueueName, false, consumer).ConfigureAwait(false);
    }
}

public interface IConsumerSettings
{
    string ExchangeName { get; set; }
    string QueueName { get; set; }
    string[] RoutingKeys { get; set; }
}