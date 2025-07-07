namespace SeroGlint.DotNet.Windows.Registry.Interfaces
{
    internal interface IRegistryWrapper
    {
        IRegistryKey CurrentUser { get; }
        IRegistryKey LocalMachine { get; }
        IRegistryKey ClassesRoot { get; }
        IRegistryKey Users { get; }
        IRegistryKey PerformanceData { get; }
        IRegistryKey CurrentConfig { get; }
    }
}
