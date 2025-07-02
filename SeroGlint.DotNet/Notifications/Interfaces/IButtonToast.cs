using System.Collections.Generic;

namespace SeroGlint.DotNet.Notifications.Interfaces
{
    public interface IButtonToast : IToastPayload
    {
        IList<IToastButton> Buttons { get; }
    }
}
