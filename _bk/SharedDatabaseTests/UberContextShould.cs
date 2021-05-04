using NUnit.Framework;
using System.Data.Entity;
using WebApplication.SharedDatabase.DataModel;

namespace SharedDatabaseTests
{
    [TestFixture]
    public class UberContextShould
    {
        public UberContextShould()
        {
            Database.SetInitializer(new TestInitializerForUberContext());
        }

        [Test]
        public void BuildModel()
        {
            var db = new UberContext();
            db.Database.Initialize(true);
        }
    }
}
