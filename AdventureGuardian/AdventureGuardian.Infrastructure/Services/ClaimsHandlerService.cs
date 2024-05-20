using Microsoft.AspNetCore.Http;

namespace AdventureGuardian.Infrastructure.Services;

public interface IClaimsHandlerService
{
    string GetClaim(string claim);
}

public class ClaimsHandlerService : IClaimsHandlerService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClaimsHandlerService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetClaim(string claim)
    {
        if (_httpContextAccessor.HttpContext == null) throw new ApplicationException("HttpContext was null");

        return _httpContextAccessor.HttpContext.User.FindFirst(claim)?.Value ??
            throw new ArgumentNullException($"{claim} was not present in the claims list");
    }
}