using System.Collections.Concurrent;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqModule.Common;

namespace RabbitMqModule.RpcClient;

public class RpcClient<TRequestMessage, TResponseMessage>(
    IRabbitMqConnectionProvider connectionProvider,
    IRpcClientSettings rpcClientSettings,
    IRpcMessageSerializer<TRequestMessage, TResponseMessage> rpcMessageSerializer
) : IRpcClient<TRequestMessage, TResponseMessage>
{
    private readonly ConcurrentDictionary<string, TaskCompletionSource<TResponseMessage>> callbackMapper = new();
    private readonly string replyQueueName = rpcClientSettings.ReplyQueueName;
    private IChannel consumerChannel;

    public async Task Start()
    {
        var connection = await connectionProvider.Get().ConfigureAwait(false);
        consumerChannel = await ChannelBuilder
            .New(connection)
            .DeclareQueue(replyQueueName)
            .Build()
            .ConfigureAwait(false);

        var consumer = new AsyncEventingBasicConsumer(consumerChannel);

        consumer.ReceivedAsync += (model, ea) =>
        {
            var correlationId = ea.BasicProperties.CorrelationId;
            if (!string.IsNullOrEmpty(correlationId) && callbackMapper.TryRemove(correlationId, out var tcs))
            {
                var response = rpcMessageSerializer.Deserialize(ea.Body.ToArray());
                tcs.TrySetResult(response);
            }

            return Task.CompletedTask;
        };

        await consumerChannel.BasicConsumeAsync(consumerChannel.CurrentQueue!, true, consumer).ConfigureAwait(false);
    }

    public async Task<TResponseMessage> Call(TRequestMessage requestMessage, string? routingKey = null)
    {
        var connection = await connectionProvider.Get().ConfigureAwait(false);
        var channel = await ChannelBuilder
            .New(connection)
            .AddExchange(rpcClientSettings.ExchangeName, ExchangeType.Direct)
            .Build()
            .ConfigureAwait(false);

        var correlationId = Guid.NewGuid().ToString();
        var props = new BasicProperties
        {
            CorrelationId = correlationId,
            ReplyTo = replyQueueName
        };

        var tcs = new TaskCompletionSource<TResponseMessage>(TaskCreationOptions.RunContinuationsAsynchronously);
        callbackMapper.TryAdd(correlationId, tcs);

        var requestBytes = rpcMessageSerializer.Serialize(requestMessage);
        await channel
            .BasicPublishAsync(rpcClientSettings.ExchangeName, routingKey!, true, props, requestBytes)
            .ConfigureAwait(false);

        return await tcs.Task;
    }
}

public interface IRpcClientSettings
{
    public string ExchangeName { get; set; }
    public string ReplyQueueName { get; set; }
}