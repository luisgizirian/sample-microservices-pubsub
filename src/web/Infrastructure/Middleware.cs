namespace Web.Infrastructure;

public class CorrelationIdHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CorrelationIdHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var correlationId = _httpContextAccessor.HttpContext?.Request?.Headers["X-Correlation-ID"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
            request.Headers.Add("X-Correlation-ID", correlationId);
        }

        var response = await base.SendAsync(request, cancellationToken);

        response.Headers.TryGetValues("X-Correlation-ID", out var correlationIdValues);

        if (correlationIdValues == null || !correlationIdValues.Any())
        {
            response.Headers.Add("X-Correlation-ID", correlationId);
        }

        return response;
    }
}
