﻿namespace CoreApp.Domain.Interfaces.Services;

public interface ITokenService
{
    Task<string> GetTokenAsync(string baseUrl, string tokenEndpoint, string clientId, string clientSecret, CancellationToken cancellationToken = default(CancellationToken));
}

