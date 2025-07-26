using ASPNET.Common.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET.Common.RequestRateLimiter
{

    public class InMemoryRateLimiter : IInMemoryRateLimiter, IDisposable
    {
        private readonly Dictionary<MemoryRateRequestType, RateLimitRule> _rules;
        private readonly ConcurrentDictionary<string, List<DateTime>> _requestLogs = new();
        private readonly ConcurrentDictionary<string, DateTime> _blockedUntil = new();

        private readonly Timer _cleanupTimer;
        private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(10); // هر 1 دقیقه پاک‌سازی

        public InMemoryRateLimiter(Dictionary<MemoryRateRequestType, RateLimitRule> rules)
        {
            _rules = rules;

            _cleanupTimer = new Timer(CleanupOldEntries, null, _cleanupInterval, _cleanupInterval);
        }

        public bool IsRequestAllowed(MemoryRateRequestType type, string clientId)
        {
            if (!_rules.TryGetValue(type, out var rule))
                return true;

            string key = $"{type}_{clientId}";
            var now = DateTime.UtcNow;

            // بررسی بلاک بودن
            if (_blockedUntil.TryGetValue(key, out var blockedUntil))
            {
                if (now < blockedUntil)
                    return false;
                else
                    _blockedUntil.TryRemove(key, out _);
            }

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
                {
                    _blockedUntil[key] = now.AddSeconds(rule.BlockDurationSeconds);
                    return false;
                }

                log.Add(now);
                return true;
            }
        }

        private void CleanupOldEntries(object? state)
        {
            var now = DateTime.UtcNow;

            // پاک‌سازی بلاک‌هایی که منقضی شدن
            foreach (var kvp in _blockedUntil)
            {
                if (kvp.Value < now)
                    _blockedUntil.TryRemove(kvp.Key, out _);
            }

            // پاک‌سازی کلیدهایی که همه‌ی درخواست‌هاشون قدیمی شده
            foreach (var kvp in _requestLogs)
            {
                var typeStr = kvp.Key.Split('_')[0];
                if (!Enum.TryParse<MemoryRateRequestType>(typeStr, out var type))
                    continue;

                if (!_rules.TryGetValue(type, out var rule))
                    continue;

                var windowStart = rule.TimeUnit switch
                {
                    RateLimitTimeUnit.Second => now.AddSeconds(-rule.Duration),
                    RateLimitTimeUnit.Minute => now.AddMinutes(-rule.Duration),
                    RateLimitTimeUnit.Hour => now.AddHours(-rule.Duration),
                    _ => now
                };

                var list = kvp.Value;
                lock (list)
                {
                    list.RemoveAll(t => t < windowStart);
                    if (list.Count == 0)
                        _requestLogs.TryRemove(kvp.Key, out _);
                }
            }
        }

        public void Dispose()
        {
            _cleanupTimer?.Dispose();
        }
    }


}
