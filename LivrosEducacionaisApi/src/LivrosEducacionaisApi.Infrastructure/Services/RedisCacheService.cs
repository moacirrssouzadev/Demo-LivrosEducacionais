using LivrosEducacionaisApi.Application.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;

namespace LivrosEducacionaisApi.Infrastructure.Services;

public class RedisCacheService : ICacheService
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = false,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };

    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        var data = await _cache.GetAsync(key, cancellationToken);
        if (data == null) return default;

        return JsonSerializer.Deserialize<T>(data, _jsonOptions);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken)
    {
        var data = JsonSerializer.SerializeToUtf8Bytes(value, _jsonOptions);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        await _cache.SetAsync(key, data, options, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync(key, cancellationToken);
    }
}
