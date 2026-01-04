using RabbitMQ.Client;

namespace RabbitMqModule.Common;

public class ChannelBuilder
{
    private readonly IConnection connection;

    private string? exchangeName;
    private string? exchangeType;
    private bool isExchangeAdd;

    private string? queueName;
    private bool isQueueAdd;

    private string[]? routingKeys;
    private bool isUseRouting;

    private bool useQos;
    private ushort prefetchCount;

    public static ChannelBuilder New(IConnection connection) => new(connection);

    private ChannelBuilder(IConnection connection)
    {
        this.connection = connection;
    }

    public ChannelBuilder AddExchange(string name, string type)
    {
        exchangeName = name;
        exchangeType = type;
        isExchangeAdd = true;

        return this;
    }

    public ChannelBuilder DeleteExchange()
    {
        isExchangeAdd = false;

        return this;
    }

    public ChannelBuilder DeclareQueue(string name = null)
    {
        queueName = name;
        isQueueAdd = true;

        return this;
    }

    public ChannelBuilder BindQueue(params string[] routingKeys)
    {
        this.routingKeys = routingKeys;
        isUseRouting = true;

        return this;
    }

    public ChannelBuilder UnbindQueue()
    {
        isUseRouting = false;

        return this;
    }

    public ChannelBuilder AddQos(ushort prefetchCount = 1)
    {
        useQos = true;
        this.prefetchCount = prefetchCount;

        return this;
    }

    public ChannelBuilder RemoveQos()
    {
        useQos = false;

        return this;
    }

    public async Task<IChannel> Build()
    {
        var channel = await connection.CreateChannelAsync().ConfigureAwait(false);

        if (isExchangeAdd)
            await channel.ExchangeDeclareAsync(exchangeName!, exchangeType!).ConfigureAwait(false);

        if (isQueueAdd)
            await AddQueue(channel).ConfigureAwait(false);

        if (useQos)
            await channel.BasicQosAsync(0, prefetchCount, false).ConfigureAwait(false);

        if (isUseRouting && isExchangeAdd)
        {
            var currentQueueName = channel.CurrentQueue;
            foreach (var routingKey in routingKeys!)
            {
                await channel.QueueBindAsync(currentQueueName!, exchangeName!, routingKey)
                    .ConfigureAwait(false);
            }
        }

        return channel;
    }

    private async Task AddQueue(IChannel channel)
    {
        if (queueName != null)
            await channel.QueueDeclareAsync(queueName).ConfigureAwait(false);
        else
            await channel.QueueDeclareAsync().ConfigureAwait(false);
    }
}