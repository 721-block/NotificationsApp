using RabbitMQ.Client;

namespace RabbitMqModule;

public interface IRabbitMqConnectionProvider
{
    Task<IConnection> Get();
}

public class RabbitMqConnectionProvider : IRabbitMqConnectionProvider
{
    private IConnection? connection;
    private readonly SemaphoreSlim semaphoreSlim = new(1, 1);
    private ConnectionFactory factory;

    public RabbitMqConnectionProvider(IRabbitMqSettings rabbitMqSettings)
    {
        factory = new ConnectionFactory
        {
            HostName = rabbitMqSettings.HostName,
            AutomaticRecoveryEnabled = rabbitMqSettings.AutomaticRecoveryEnabled,
            NetworkRecoveryInterval = rabbitMqSettings.NetworkRecoveryInterval
        };
    }

    public async Task<IConnection> Get()
    {
        await semaphoreSlim.WaitAsync();
        try
        {
            if (connection is not { IsOpen: true })
            {
                connection?.Dispose();
                connection = await factory.CreateConnectionAsync().ConfigureAwait(false);
            }
        }
        finally
        {
            semaphoreSlim.Release();
        }

        return connection;
    }
}

public interface IRabbitMqSettings
{
    string HostName { get; set; }
    bool AutomaticRecoveryEnabled { get; set; }
    TimeSpan NetworkRecoveryInterval { get; set; }
}