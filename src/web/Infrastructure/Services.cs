using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Web.Infrastructure;

public interface IContactService
{
    Task<ContactBatch> SearchAsync(string expression, int? elements = 10, int page = 1);
    Task<bool> Add(ContactViewModel model);
}

public class ContactService : IContactService
{
    #region Private implementation
    private readonly IHttpClientFactory _factory;
    private readonly IHttpContextAccessor _contextAccessor;

    private HttpClient Client
    {
        get => _factory.CreateClient("smps.common.api");
    }

    private HttpContext Context
    {
        get => _contextAccessor.HttpContext;
    }

    const string _basePath = "contacts";
    #endregion

    public ContactService(
        IHttpClientFactory factory,
        IHttpContextAccessor contextAccessor)
    {
        _factory = factory;
        _contextAccessor = contextAccessor;
    }

    public async Task<ContactBatch> SearchAsync(
        string expression,
        int? elements = 10,
        int page = 1)
    {
        //
        // TODO / Security: reject Empty or Null "expresion" argument
        //

        var resp = await Client.GetAsync($"{_basePath}?expression={expression}&elements={elements}&page={page}");
        
        var result = new ContactBatch();

        if (resp.IsSuccessStatusCode)
        {
            result = JsonSerializer.Deserialize<ContactBatch>(
                await resp.Content.ReadAsStringAsync(),
                new JsonSerializerOptions{
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
        }

        return result;
    }
    
    public async Task<bool> Add(ContactViewModel model)
    {
        var result = true;

        var dto = new ContactModel
        {
            ContactId = Guid.NewGuid(),
            Name = model.Name,
            Email = model.Email,
            Phone = model.Phone,
            Message = model.Message,

            Created = DateTime.UtcNow
        };

        await Client
            .PostAsync(_basePath, new StringContent(
                JsonSerializer.Serialize(new
                {
                    Contact = dto
                }),
                Encoding.UTF8, "application/json"))
            .ContinueWith(resp =>
            {
                result &= resp.Result.IsSuccessStatusCode;
            });
        
        return result;
    }
}

public interface IManagementService
{
    Task<ContactBatch> GetLatestMessagesAsync(string expression = null, int page = 1, int size = 20);
}

public class ManagementService : IManagementService
{
    #region Private implementation
    private readonly IContactService _contact;
    #endregion

    public ManagementService(
        IContactService contactService
        )
    {
        _contact = contactService;
    }

    public Task<ContactBatch> GetLatestMessagesAsync(
        string expression,
        int page,
        int size)
    {
        return _contact.SearchAsync(expression, size, page);
    }
}