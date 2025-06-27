namespace RegressionTestHarness.Objects
{
    internal class TestObject
    {
        public int Id { get; set; } = new Random().Next();
        public string Name { get; set; } = "TestObjectName";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Source { get; } = "RegressionTestHarness";
    }
}
