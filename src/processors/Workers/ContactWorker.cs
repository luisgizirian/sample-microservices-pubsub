using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Processors.Infrastructure;
using StackExchange.Redis;

namespace Processors.Workers;

public class ContactWorker : BackgroundService
{
    private readonly ILogger<ContactWorker> _logger;
    private readonly IDistributedCache _cachedb;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public ContactWorker(
        ILogger<ContactWorker> logger,
        IDistributedCache cache,
        IConnectionMultiplexer multiplexer)
    {
        _logger = logger;
        _cachedb = cache;
        _connectionMultiplexer = multiplexer;
    }
        
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        const string _QUEUE_NAME = "contacts";

        var pubsub = _connectionMultiplexer.GetSubscriber();
        _logger.LogInformation("Connected to Message Bus :)");
        _logger.LogDebug($"ContactWorker task doing background work.");
        await pubsub.SubscribeAsync(_QUEUE_NAME, async (channel, message) => await MessageAction(message));
        _logger.LogDebug("Quitting doing background work?");
    }

    private async Task MessageAction(RedisValue raw)
    {
        var message = System.Text.Json.JsonSerializer
                .Deserialize<AddContact>(
                    Encoding.UTF8.GetString(raw));
        
        _logger.LogDebug($" [x] Received {message?.CorrelationId}");

        await AddContact(message);
    }

    private async Task AddContact(AddContact e)
    {
        const string _REPLY_QUEUE = "contacts_events";

        // Let's simulate some delay. Optional (please comment it after testing)
        await Task.Delay(3000);
        //

        // TODO: this overwrites each time. Save multiple instances as expected for a table.
        await _cachedb.SetStringAsync("db_contacts", JsonSerializer.Serialize(e.Contact));
        //

        // TODO: act on MailChimp, Zendesk, Email and other actions.
        
        // NOTE: signal operation successful completion.
        var signal = JsonSerializer.Serialize(
                new { e.CorrelationId, Status = "done"});
        var pubsub = _connectionMultiplexer.GetSubscriber();
        _ = await pubsub.PublishAsync(
            _REPLY_QUEUE,
            signal);
        _logger.LogInformation($" [x] Signaled {signal}");
    }
}