using System.Diagnostics.CodeAnalysis;

namespace SeroGlint.DotNet.Tests.TestObjects;

[ExcludeFromCodeCoverage]
public class TestConfig
{
    public string AppName { get; set; } = "DefaultApp";
    public int RetryCount { get; set; } = 3;
}
