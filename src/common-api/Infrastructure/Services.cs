using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace CommonApi.Infrastructure;

public interface IContactService
{
    Task AddAsync(Guid correlation, ContactModel contact);
    Task<ContactBatch> Search(string q = "", int elements = 10, int page = 1);
}

public class ContactService : IContactService
{
    private readonly ILogger<ContactService> _logger;
    private readonly IDistributedCache _cachedb;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public ContactService(
        ILogger<ContactService> logger,
        IDistributedCache cache,
        IConnectionMultiplexer multiplexer)
    {
        _logger = logger;
        _cachedb = cache;
        _connectionMultiplexer = multiplexer;
    }

    public async Task AddAsync(
        Guid correlation,
        ContactModel contact)
    {
        // TODO: review implemented async pattern.
        await NotifyAsync(correlation, contact);
    }

    public async Task<ContactBatch> Search(string q = "", int elements = 10, int page = 1)
    {
        var table = (await _cachedb.GetStringAsync("db_contacts")).Split("|:|");
        // TODO LIKE filtering
        
        var total =
            table.AsEnumerable().Count();

        var result =
            table.ToList()
                .Skip((page - 1) * elements)
                .Take(elements)
                .ToList();

        return new ContactBatch { Total = total, Elements = result.AsModel(), Search = q, Number = page };
    }

    #region Private implementation
    private async Task NotifyAsync(
        Guid correlation,
        ContactModel model)
    {
        const string _QUEUE_NAME = "contacts";
        const string _REPLY_QUEUE = "contacts_events";

        // 1 - Prepare event-message
        var @event = new AddContact
        {
            CorrelationId = correlation,
            Contact = model
        };
        string message = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(message);

        var redispub = _connectionMultiplexer.GetSubscriber();
        _ = await redispub.PublishAsync(_QUEUE_NAME, body);
        var result = "unknown";
        await redispub.SubscribeAsync(_REPLY_QUEUE, async (channel, val) => {
            var resp = JsonSerializer.Deserialize<CommentAdded>(val, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
            if (resp?.CorrelationId == correlation) {
                result = resp.Status;
                await redispub.UnsubscribeAsync(channel);
            }else{
                _logger.LogInformation($"Discarding {resp?.CorrelationId} as not for us to process.");
            }
        });
        while (result == "unknown") {
            await Task.Delay(100);
            _logger.LogInformation("Waiting on forced Synchronicity");
        }
        _logger.LogInformation("Ready to continue...");
    }
    #endregion
}