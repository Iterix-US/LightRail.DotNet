using System;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.Notifications.Interfaces;

namespace SeroGlint.DotNet.Notifications.Payloads
{
    public class BasicToast : IBasicToast
    {
        public ILogger Logger { get; }
        public string Id { get; }
        public string GroupName { get; }
        public string Title { get; }
        public string Body { get; }
        public string IconPath { get; }
        public TimeSpan? Timeout { get; }

        public BasicToast(ILogger logger, string title, string body, string id = null, string groupName = null, TimeSpan? timeout = null, string iconPath = null)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Title = title;
            Body = body;
            Id = id ?? Guid.NewGuid().ToString();
            GroupName = groupName ?? "BasicToast";
            Timeout = timeout ?? TimeSpan.FromSeconds(5);
            IconPath = iconPath;
        }
    }
}
