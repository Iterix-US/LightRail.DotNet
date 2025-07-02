namespace SeroGlint.DotNet.Notifications.Events
{
    public class ToastInteractionTriggeredEventArgs
    {
        public string ToastId { get; set; }
        public ToastInteractionType InteractionType { get; set; }
        public string InteractionData { get; set; }
    }
}
