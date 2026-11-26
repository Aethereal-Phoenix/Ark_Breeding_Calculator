using ArkCalculatorTests.Helpers;
using Calculator_API.Controllers;
using Calculator_API.Data;
using Calculator_API.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

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

        #region GetAllBreedingLines

        #region DAL success
        [TestMethod]
        public void GetAllBreedingLines_WithMultipleBreedingLines_ReturnsAllBreedingLines()
        {
            // Arrange
            using var context = CreateInMemoryContext("GetAllBreedingLines_ReturnsAll");

            TestDataSeeder.BasicData(context);

            var dal = new DinoDAL(context, CreateTestConfig());

            // Act
            var result = dal.GetAllBreedingLines();

            // Assert
            Assert.IsNotNull(result);

            Assert.AreEqual(2, result.Count);

            Assert.AreEqual("Test Shadowmane Line Test", result[0].BreedingLineName);
            Assert.AreEqual(SpeciesEnum.Shadowmane, result[0].Species);
        }
        #endregion

        #region Controller Success
        [TestMethod]
        public async Task GetAllBreedingLines_ReturnsSuccessResponse()
        {
            // Arrange
            using var context = CreateInMemoryContext("GetAllBreedingLines_ControllerSuccess");

            TestDataSeeder.BasicData(context);

            var repo = new DinoDAL(context, CreateTestConfig());

            var controller = new DinoController(repo, CreateTestConfig(), context);

            // Act
            var result = await controller.GetAllBreedingLines();

            // Assert
            Assert.IsTrue(result.Status);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("Get Successful", result.Message);

            Assert.IsNotNull(result.BreedingLinesList);
            Assert.AreEqual(2, result.BreedingLinesList.Count);
        }
        #endregion

        #region Controller Failure
        [TestMethod]
        public async Task GetAllBreedingLines_ReturnsFailureResponse()
        {
            // Arrange
            using var context = CreateInMemoryContext("GetAllBreedingLines_ControllerFailure");

            // No Seed data to mimic an empty database

            var repo = new DinoDAL(context, CreateTestConfig());

            var controller = new DinoController(repo, CreateTestConfig(), context);

            // Act
            var result = await controller.GetAllBreedingLines();

            // Assert
            Assert.IsFalse(result.Status);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual("No breeding lines found", result.Message);

            Assert.IsNotNull(result.BreedingLinesList);
            Assert.AreEqual(0, result.BreedingLinesList.Count);
        }
        #endregion

        #region DAL Empty List
        [TestMethod]
        public void GetAllBreedingLines_NoBreedingLines_ReturnsEmptyList()
        {
            // Arrange
            using var context = CreateInMemoryContext("GetAllBreedingLines_ReturnsNone");

            var dal = new DinoDAL(context, CreateTestConfig());

            // Act
            var result = dal.GetAllBreedingLines();

            // Assert
            Assert.IsNotNull(result);

            Assert.AreEqual(0, result.Count);
        }
        #endregion

        #endregion

        #region GetDinosaurByBreedingline

        #region GetDinosaursByBreedingLine Success
        [TestMethod]
        public void GetDinosaurByBreedingLine_WithMultipleDinosaurs_ReturnsList()
        {
            // Arrange
            using var context = CreateInMemoryContext("GetDinosaurByBreedingLine_Success");

            TestDataSeeder.BasicData(context);

            var dal = new DinoDAL(context, CreateTestConfig());

            // Act
            var result = dal.GetDinosaursByBreedingLine(1);

            // Assert
            Assert.IsNotNull(result);

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].DinosaurId);
            Assert.AreEqual(1, result[0].BreedingLineId);
            Assert.AreEqual("Shadowmane test 1", result[0].DinosaurName);
            Assert.AreEqual(SpeciesEnum.Shadowmane, result[0].Species);
            Assert.AreEqual(GenderEnum.Male, result[0].Gender);
            Assert.AreEqual(GenerationsEnum.Base, result[0].Generation);
            Assert.AreEqual(1, result[0].StatsId);
            Assert.IsNotNull(result[0].Stats);
            Assert.AreEqual(1, result[0].MutatedStatsId);
            Assert.IsNotNull(result[0].MutatedStats);
            Assert.IsFalse(result[0].Mutated);
            Assert.IsTrue(result[0].AquaticDino);
            Assert.IsNull(result[0].FatherId);
            Assert.IsNull(result[0].Father);
            Assert.IsNull(result[0].MotherId);
            Assert.IsNull(result[0].Mother);
        }
        #endregion

        #region GetDinosaursByBreedingLine Null
        [TestMethod]
        public void GetDinosaurByBreedingLine_InvalidBreedingLineId_ReturnsNull()
        {
            // Arrange
            using var context = CreateInMemoryContext("GetDinosaurByBreedingLine_InvalidId");

            TestDataSeeder.BasicData(context);

            var dal = new DinoDAL(context, CreateTestConfig());

            // Act
            var result = dal.GetDinosaursByBreedingLine(3);

            // Assert
            Assert.IsNull(result);
        }
        #endregion

        #endregion

        #region GetStatsById

        #region ValidId Success
        [TestMethod]
        public void GetStatsById_ValidId_ReturnsStats()
        {
            // Arrange
            using var context = CreateInMemoryContext("GetStatsById_ValidId");

            TestDataSeeder.BasicData(context);

            var dal = new DinoDAL(context, CreateTestConfig());

            // Act
            var result = dal.GetStatsById(1);

            // Assert
            Assert.IsNotNull(result);

            Assert.AreEqual(1, result.StatsId);
        }
        #endregion

        #region InvalidId Returns Exception
        [TestMethod]
        public void GetStatsById_InvalidId_ReturnsException()
        {
            // Arrange
            using var context = CreateInMemoryContext("GetStatsById_InvalidId");

            TestDataSeeder.BasicData(context);

            var dal = new DinoDAL(context, CreateTestConfig());

            try
            {
                // Act
                dal.GetStatsById(999);

                Assert.Fail("Expected InvalidOperationException to be thrown.");
            }
            catch (InvalidOperationException ex)
            {
                // Assert
                Assert.AreEqual(
                    "Stats record with 999 was not found",
                    ex.Message);
            }
        }
        #endregion

        #endregion

        #region GetMutatedaStatsById

        #region InvalidId Throws Exception
        [TestMethod]
        public void GetMutatedStatsById_InvalidId_ThrowsException()
        {
            // Arrange
            using var context = CreateInMemoryContext("GetMutatedStatsById_InvalidId");

            TestDataSeeder.BasicData(context);

            var dal = new DinoDAL(context, CreateTestConfig());

            try
            {
                // Act
                dal.GetMutatedStatsById(999);

                Assert.Fail("Expected InvalidOperationException to be thrown.");
            }
            catch (InvalidOperationException ex)
            {
                // Assert
                Assert.AreEqual(
                    "MutatedStats record with 999 was not found",
                    ex.Message);
            }
        }
        #endregion

        #endregion
    }
}