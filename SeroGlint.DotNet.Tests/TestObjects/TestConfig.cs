using System.Diagnostics.CodeAnalysis;

namespace SeroGlint.DotNet.Tests.TestObjects;

[ExcludeFromCodeCoverage]
public class TestConfig
{
    public string AppName { get; init; } = "DefaultApp";
    public int RetryCount { get; init; } = 3;
}
