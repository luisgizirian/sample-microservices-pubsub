namespace CommonApi.Infrastructure;

public abstract class BaseMessage
{
    public Guid CorrelationId { get; set; }
}

public class AddContact : BaseMessage
{
    public ContactModel Contact { get; set; }
}