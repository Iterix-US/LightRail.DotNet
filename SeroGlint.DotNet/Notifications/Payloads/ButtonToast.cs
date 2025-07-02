using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SeroGlint.DotNet.Notifications.Interfaces;

namespace SeroGlint.DotNet.Notifications.Payloads
{
    public class ButtonToast : IButtonToast
    {
        public ILogger Logger { get; }
        public string Id { get; }
        public string GroupName { get; }
        public string Title { get; }
        public TimeSpan? Timeout { get; }
        public IList<IToastButton> Buttons { get; }

        public ButtonToast(ILogger logger, string title, string id = null, string groupName = null, TimeSpan? timeout = null, IList<IToastButton> buttons = null)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Title = title;
            Id = id ?? Guid.NewGuid().ToString();
            GroupName = groupName ?? "ButtonToast";
            Timeout = timeout ?? TimeSpan.FromSeconds(5);
            Buttons = buttons ?? new List<IToastButton>();
        }
    }
}
