using Library.Api;

namespace LibraryTests
{
    class TestRecord
    {
        public string? field { get; set; }
    }

    [TestClass]
    public class CatAPITest
    {
        [TestMethod]
        public void TestApiFetch()
        {
            var api = new CatApiClient<TestRecord>();
            var response = @"
                            [{'field': 'value1'}, {'field': 'value2'}]
                            ";
            List<TestRecord>? records = api.CreateCatsFromJson(response);
            Assert.IsNotNull(records);
            Assert.AreEqual(2, records.Count());
            Assert.AreEqual("value1", records[0].field);
            Assert.AreEqual("value2", records[1].field);
        }
    }
}