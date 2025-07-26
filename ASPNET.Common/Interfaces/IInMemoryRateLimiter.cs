using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET.Common.Interfaces
{
    public enum MemoryRateRequestType
    {
        AuthWebSocket,
        Download,
        ActivationWebSocket,
        // موارد دلخواه دیگر...
    }

    public enum RateLimitTimeUnit
    {
        Second,
        Minute,
        Hour
    }

    public class RateLimitRule
    {
        public int MaxRequests { get; set; }
        public RateLimitTimeUnit TimeUnit { get; set; }
        public int Duration { get; set; } // مثلاً 1 دقیقه یا 10 ثانیه
    }
    public interface IInMemoryRateLimiter
    {
    }
}
