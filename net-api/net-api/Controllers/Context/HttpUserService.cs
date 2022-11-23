using MongoDB.Bson;
using System.Security.Claims;

namespace net_api.Controllers.Context
{
    public class HttpUserService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetName()
        {
            return GetClaim(ClaimTypes.Name);
        }

        public string? GetId()
        {
            return GetClaim(ClaimTypes.NameIdentifier);
        }

        public ObjectId? GetObjectId()
        {
            string? id = GetId();
            if (id == null) return null;

            return ObjectId.Parse(id);
        }

        private string? GetClaim(string claimType)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                return _httpContextAccessor.HttpContext.User.FindFirstValue(claimType);
            }

            return null;
        }
    }
}
