using System.ComponentModel;

namespace SeroGlint.DotNet.Tests.TestObjects;

public enum SampleEnum
{
    [Description("First Option")] 
    ValueOne
}

public enum SampleLongEnum : long
{
    [Description("For conversion failure")]
    ValueOne = (long)int.MaxValue + 1
}
