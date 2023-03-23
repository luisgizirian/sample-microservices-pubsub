
using System.Text.Json;

namespace Processors.Infrastructure;

public class ContactModel
{
    public Guid ContactId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
    public DateTime Created { get; set; }
    public bool IsEnabled { get; set; }
    public string Phone { get; set; }
    public string Company { get; set; }
}

public class CustomerGroupsModel
{
    public string Id { get; set; }
    public List<string> Elements { get; set; }
}