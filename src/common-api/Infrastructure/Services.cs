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
        var _REPLY_CHANNEL = correlation.ToString();

        // 1 - Prepare event-message
        var @event = new AddContact
        {
            CorrelationId = correlation,
            Contact = model
        };
        string message = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(message);

        var redispub = _connectionMultiplexer.GetSubscriber();
        
        var respSubscription = await redispub.SubscribeAsync(_REPLY_CHANNEL);
        
        _ = await redispub.PublishAsync(_QUEUE_NAME, body);

        var response = await this.ProcessResponse(
            redispub,
            await respSubscription.ReadAsync(),
            _REPLY_CHANNEL);
        
        _logger.LogInformation($"Ready to continue... Final status: {response}");
    }

    private async Task<string> ProcessResponse(ISubscriber redispub, ChannelMessage val, string challenge)
    {
        var resp = JsonSerializer.Deserialize<CommentAdded>(val.Message, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
        
        if (val.Channel != challenge) {
            _logger.LogInformation($"Discarding {resp?.CorrelationId} as is not for us to process.");
            return "disposed";
        }
        
        await redispub.UnsubscribeAsync(challenge);
        return resp?.Status ?? "unknown";
    }
    #endregion
}