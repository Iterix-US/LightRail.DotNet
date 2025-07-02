using System;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.Notifications.Interfaces;

namespace SeroGlint.DotNet.Notifications.Payloads
{
    public class ProgressToast : IProgressToast
    {
        public ILogger Logger { get; }
        public string Id { get; }
        public string GroupName { get; }
        public string Title { get; }
        public TimeSpan? Timeout { get; }
        public decimal MinValue { get; }
        public decimal MaxValue { get; }
        public decimal CurrentValue { get; set; }

        public ProgressToast(ILogger logger, string id, string groupName, string title, decimal minValue, decimal maxValue, decimal currentValue, TimeSpan? timeout = null)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Id = id ?? Guid.NewGuid().ToString();
            GroupName = groupName ?? "SeroGlint";
            Title = title ?? string.Empty;
            MinValue = minValue;
            MaxValue = maxValue;
            CurrentValue = currentValue;
            Timeout = timeout;
        }

        public void Update()
        {

        }
    }
}
