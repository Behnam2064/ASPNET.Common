using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPNET.Common.Interfaces
{
    public enum MemoryRateRequestType
    {
        Auth, //This amount is considered as a general amount.
        AuthWebSocket,
        Download, //This amount is considered as a general amount.
        Activation, //This amount is considered as a general amount.
        ActivationWebSocket,
        ConnectWebSocket,
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
        public int Duration { get; set; } // مثلا 1 دقیقه
        public int BlockDurationSeconds { get; set; } = 60; // مدت بلاک بعد از عبور از محدودیت
    }
    public interface IInMemoryRateLimiter
    {
        public bool IsRequestAllowed(MemoryRateRequestType type, string clientId);
        public void Reset();
    }
}
