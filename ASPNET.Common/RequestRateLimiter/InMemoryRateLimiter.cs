using ASPNET.Common.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET.Common.RequestRateLimiter
{

    public class InMemoryRateLimiter : IInMemoryRateLimiter
    {
        private readonly Dictionary<MemoryRateRequestType, RateLimitRule> _rules;
        private readonly ConcurrentDictionary<string, List<DateTime>> _requestLogs = new();

        public InMemoryRateLimiter(Dictionary<MemoryRateRequestType, RateLimitRule> rules)
        {
            _rules = rules;
        }

        public bool IsRequestAllowed(MemoryRateRequestType type, string clientId)
        {
            if (!_rules.TryGetValue(type, out var rule))
                return true; 

            string key = $"{type}_{clientId}";
            var now = DateTime.UtcNow;
            var windowStart = rule.TimeUnit switch
            {
                RateLimitTimeUnit.Second => now.AddSeconds(-rule.Duration),
                RateLimitTimeUnit.Minute => now.AddMinutes(-rule.Duration),
                RateLimitTimeUnit.Hour => now.AddHours(-rule.Duration),
                _ => now
            };

            var log = _requestLogs.GetOrAdd(key, _ => new List<DateTime>());

            lock (log)
            {
                log.RemoveAll(t => t < windowStart);

                if (log.Count >= rule.MaxRequests)
                    return false;

                log.Add(now);
                return true;
            }
        }
    }

}
