namespace Processors.Infrastructure;

public abstract class BaseMessage
{
    public Guid CorrelationId { get; set; }
}

public class AddContact : BaseMessage
{
    public ContactModel Contact { get; set; }
    public List<CustomerGroupsModel> Groups { get; set; }
    public bool Subscribe { get; set; }
}