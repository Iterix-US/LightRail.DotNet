using System.Diagnostics.CodeAnalysis;

namespace LightRail.DotNet.Tests.TestObjects;

[ExcludeFromCodeCoverage]
public class TestConfig
{
    public string AppName { get; set; } = "DefaultApp";
    public int RetryCount { get; set; } = 3;
}
