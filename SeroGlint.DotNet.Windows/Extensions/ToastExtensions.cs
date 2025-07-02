using Windows.UI.Notifications;
using SeroGlint.DotNet.Notifications.Interfaces;

namespace SeroGlint.DotNet.Windows.Extensions
{
    public static class ToastExtensions
    {
        public static ToastNotification ToToast(this IToastPayload payload)
        {
            var payloadType = payload.GetType();
            if (payloadType.IsAssignableFrom(typeof(IProgressToast)))
            {
                return ParseProgressToast(payload);
            }

            if (payloadType.IsAssignableFrom(typeof(IButtonToast)))
            {
                return ParseButtonToast(payload);
            }

            if (payloadType.IsAssignableFrom(typeof(IBasicToast)))
            {
                return ParseBasicToast(payload);
            }

            return default;
        }

        private static ToastNotification ParseProgressToast(IToastPayload payload)
        {
            throw new System.NotImplementedException();
        }

        private static ToastNotification ParseButtonToast(IToastPayload payload)
        {
            throw new System.NotImplementedException();
        }

        private static ToastNotification ParseBasicToast(IToastPayload payload)
        {
            throw new System.NotImplementedException();
        }
    }
}
