using CommonApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CommonApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactsController : ControllerBase
{
    private readonly ILogger<ContactsController> _logger;
    private readonly IContactService _contact;

    public ContactsController(
        ILogger<ContactsController> logger,
        IContactService contactService)
    {
        _logger = logger;
        _contact = contactService;
    }

    [HttpGet(Name = "Search")]
    public async Task<ContactBatch> Get(
        string expresion = "",
        int elements = 10,
        int page = 1)
    {
        return
        (await _contact.Search(expresion, elements, page));
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody]ContactFormModel model)
    {
        if (ModelState.IsValid)
        {
            await _contact.AddAsync(model.CorrelationId, model.Contact);
            return Ok();
        }

        return BadRequest();
    }
}
