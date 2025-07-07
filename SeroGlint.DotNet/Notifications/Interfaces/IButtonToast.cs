using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace SeroGlint.DotNet.Notifications.Interfaces
{
    public interface IButtonToast
    {
        /// <summary>
        /// Gets the logger instance for logging events related to this button toast.
        /// </summary>
        ILogger Logger { get; }
        /// <summary>
        /// Gets the collection of buttons associated with this toast notification.
        /// </summary>
        IList<IToastButton> Buttons { get; }
    }
}
