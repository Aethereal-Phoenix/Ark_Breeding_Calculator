using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Calculator_API.Data;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Calculator_API.Repository;
using ArkCalculatorTests.Helpers;

namespace ArkCalculatorTests
{
    [TestClass]
    public class DinoDalTests
    {
        #region Setup

        private CalculatorContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<CalculatorContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new CalculatorContext(options);
        }

        private IConfiguration CreateTestConfig()
        {
            // UserDAL's constructor requires IConfiguration.
            // This mock provides fake values so the DAL can be constructed
            // in a test without a real appsettings.json.
            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c["TokenAuthentication:SecretKey"])
                .Returns("this is my custom Secret key for authentication");
            mockConfig.Setup(c => c["TokenAuthentication:Issuer"])
                .Returns("https://localhost:44325");
            return mockConfig.Object;
        }
        #endregion    

        #region GetAllBreedingLines Success
        [TestMethod]
        public void GetAllBreedingLines_WithMultipleBreedingLines_ReturnsAllBreedingLines()
        {
            using var context = CreateInMemoryContext("GetAllBreedingLines_ReturnsAll");

            TestDataSeeder.BasicData(context);

            var dal = new DinoDAL(context, CreateTestConfig());

            var result = dal.GetAllBreedingLines();

            Assert.IsNotNull(result);
            Console.WriteLine(result);

            Assert.AreEqual(2, result.Count);
        }
        #endregion

        #region GetAllBreedingLines Empty List
        [TestMethod]
        public async Task GetAllBreedingLines_NoBreedingLines_ReturnsEmptyList()
        {
            using var context = CreateInMemoryContext("GetAllBreedingLines_ReturnsNone");

            var dal = new DinoDAL(context, CreateTestConfig());

            var result = dal.GetAllBreedingLines();

            Assert.IsNotNull(result);

            Assert.AreEqual(0, result.Count);
        }
        #endregion

        #region GetDinosaursByBreedingLine Success
        [TestMethod]
        public void GetDinosaurByBreedingLine_WithMultipleDinosaurs_ReturnsList()
        {
            using var context = CreateInMemoryContext("GetDinosaurByBreedingLine_Success");

            TestDataSeeder.BasicData(context);

            var dal = new DinoDAL(context, CreateTestConfig());

            var result = dal.GetDinosaursByBreedingLine(1);

            Assert.IsNotNull(result);

            Assert.AreEqual(2, result.Count);

            Assert.AreEqual(1, result[0].DinosaurId);
        }
        #endregion

        #region GetDinosaursByBreedingLine Null
        [TestMethod]
        public void GetDinosaurByBreedingLine_InvalidBreedingLineId_ReturnsNull()
        {
            using var context = CreateInMemoryContext("GetDinosaurByBreedingLine_InvalidId");

            TestDataSeeder.BasicData(context);

            var dal = new DinoDAL(context, CreateTestConfig());

            var result = dal.GetDinosaursByBreedingLine(3);

            Assert.IsNull(result);
        }
        #endregion

        #region GetStatsById Success
        [TestMethod]
        public void GetStatsById_ValidId_ReturnsStats()
        {
            using var context = CreateInMemoryContext("GetStatsById_ValidId");

            TestDataSeeder.BasicData(context);

            var dal = new DinoDAL(context, CreateTestConfig());

            var result = dal.GetStatsById(1);

            Assert.IsNotNull(result);

            Assert.AreEqual(1, result.StatsId);
        }
        #endregion

        #region GetStatsById Exception
        [TestMethod]
        public void GetStatsById_InvalidId_ReturnsException()
        {
            using var context = CreateInMemoryContext("GetStatsById_InvalidId");

            TestDataSeeder.BasicData(context);

            var dal = new DinoDAL(context, CreateTestConfig());

            try
            {
                dal.GetStatsById(999);

                Assert.Fail("Expected InvalidOperationException to be thrown.");
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual(
                    "Stats record with 999 was not found",
                    ex.Message);
            }
        }
        #endregion
    }
}
