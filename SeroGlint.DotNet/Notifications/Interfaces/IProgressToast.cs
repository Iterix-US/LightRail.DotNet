namespace SeroGlint.DotNet.Notifications.Interfaces
{
    public interface IProgressToast : IToastPayload
    {
        decimal MinValue { get; }
        decimal MaxValue { get; }
        decimal CurrentValue { get; set; }

        void Update();
    }
}
