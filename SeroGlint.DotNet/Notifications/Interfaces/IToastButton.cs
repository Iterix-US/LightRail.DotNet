using System;
using SeroGlint.DotNet.Notifications.Events;

namespace SeroGlint.DotNet.Notifications.Interfaces
{
    public interface IToastButton
    {
        string Text { get; }
        Uri Uri { get; }
        event ToastInteractionTriggered ToastButtonClicked;
        void Contribute(IButtonToast toastContainer);
        void Click();
    }
}
