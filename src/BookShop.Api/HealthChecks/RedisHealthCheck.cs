using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BookShop.Api.HealthChecks;

public class RedisHealthCheck : IHealthCheck
{
    private readonly IDistributedCache _cache;

    public RedisHealthCheck(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var sampleKey = "Check_key";
            await _cache.SetStringAsync(sampleKey, "Check_value");
            var retrievedValue = await _cache.GetStringAsync(sampleKey);

            if (retrievedValue != "Check_value")
            {
                throw new Exception("Failed to retrieve value from Redis cache.");
            }

            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
