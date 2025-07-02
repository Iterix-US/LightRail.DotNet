using System;
using SeroGlint.DotNet.Extensions;
using SeroGlint.DotNet.Notifications.Events;
using SeroGlint.DotNet.Notifications.Interfaces;

namespace SeroGlint.DotNet.Notifications.ToastControls
{
    public class ToastButton : IToastButton
    {
        private string _toastId;
        public string Text { get; }
        public Uri Uri { get; }
        public event ToastInteractionTriggered ToastButtonClicked;

        public ToastButton(string buttonText, Uri uri = null)
        {
            Text = buttonText;
            Uri = uri;
        }

        public void Contribute(IButtonToast toastContainer)
        {
            _toastId = toastContainer.Id;
        }

        public void Click()
        {
            ToastButtonClicked?.Invoke(this, new ToastInteractionTriggeredEventArgs
            {
                ToastId = _toastId,
                InteractionType = ToastInteractionType.ButtonClick,
                InteractionData = this.ToJson()
            });
        }
    }
}
