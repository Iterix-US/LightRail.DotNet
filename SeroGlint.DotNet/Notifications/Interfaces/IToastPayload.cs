using System;
using Microsoft.Extensions.Logging;

namespace SeroGlint.DotNet.Notifications.Interfaces
{
    public interface IToastPayload
    {
        ILogger Logger { get; }
        string Id { get; }
        string GroupName { get; }
        string Title { get; }
        TimeSpan? Timeout { get; }
    }
}
