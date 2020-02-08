using CoreApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoreApp.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(string baseUrl, string tokenEndpoint, string clientId, string clientSecret, CancellationToken cancellationToken = default(CancellationToken));
    }
}
