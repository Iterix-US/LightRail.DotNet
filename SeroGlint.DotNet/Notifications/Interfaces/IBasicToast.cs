namespace SeroGlint.DotNet.Notifications.Interfaces
{
    public interface IBasicToast : IToastPayload
    {
        string Body { get; }
        string IconPath { get; }
    }
}
