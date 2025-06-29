using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JJ_ICTPRG433_440_AT2.Models;
using JJ_ICTPRG433_440_AT2.Services;

namespace RecruitmentSystemUnitTests
{
    [TestClass]
    public sealed class ContractorSystemTests
    {
        private RecruitmentSystem recruitmentSystem;
        [TestInitialize]
        public void Setup()
        {
            //Arrange
            recruitmentSystem = new RecruitmentSystem(null, 
                                                      new([new Contractor(Guid.NewGuid().ToString(), "Charlie", "Lee", null, 45.0),
                                                           new Contractor(Guid.NewGuid().ToString(), "Bob", "Johnson", null, 38.5)]));
        }
        [TestMethod]
        public void GetContractors_ReturnsCollection_NotNull()
        {
            //Arrange
            //Setup()

            //Act
            List<Contractor> resultContractors = recruitmentSystem.GetContractors();

            //Assert
            Assert.IsNotNull(resultContractors);
        }
        [TestMethod]
        public void GetContractors_ReturnsCollection()
        {
            //Arrange
            //Setup()

            //Act
            List<Contractor> resultContractors = recruitmentSystem.GetContractors();

            //Assert
            Assert.IsInstanceOfType<List<Contractor>>(resultContractors);
            CollectionAssert.AllItemsAreInstancesOfType(resultContractors, typeof(Contractor));
            Assert.AreEqual(2, resultContractors.Count);
        }
        [TestMethod]
        public void AddContractors_UpdateContractorsCollection_ContainsReference()
        {
            //Arrange
            //Setup()
            Contractor newContractor = new(Guid.NewGuid().ToString(), "Test03_Charlie", "Lee", null, 45.0);

            //Act
            recruitmentSystem.AddContractor(newContractor);
            List<Contractor> resultContractors = recruitmentSystem.GetContractors();

            string value1 = resultContractors[2].ToString();
            string value2 = newContractor.ToString();

            //Assert
            Assert.AreEqual(resultContractors[2].ToString(), newContractor.ToString());
            CollectionAssert.Contains(resultContractors, newContractor);
        }
        [TestMethod]
        public void AddContractors_UpdateContractorsCollection_ContainsThreeObjects()
        {
            //Arrange
            //Setup()
            Contractor newContractor = new(Guid.NewGuid().ToString(), "Test03_Charlie", "Lee", null, 45.0);

            //Act
            recruitmentSystem.AddContractor(newContractor);
            List<Contractor> resultContractors = recruitmentSystem.GetContractors();

            //Assert
            Assert.AreEqual(3, resultContractors.Count);
        }
        [TestMethod]
        public void RemoveContractors_UpdateContractorsCollection_ContainsTwoObjects()
        {
            //Arrange
            //Setup()
            Contractor newContractor = new(Guid.NewGuid().ToString(), "Test03_Charlie", "Lee", null, 45.0);

            //Act
            recruitmentSystem.AddContractor(newContractor);
            var(success, firstName, lastName, jobTitle) = recruitmentSystem.RemoveContractor(newContractor);
            List<Contractor> resultContractors = recruitmentSystem.GetContractors();

            //Assert
            Assert.AreEqual(2, resultContractors.Count);
        }
        [TestMethod]
        public void SortContractors_UpdateContractorsCollection_ByFirstName()
        {
            //Arrange
            Contractor newContractor = new(Guid.NewGuid().ToString(), "Alice", "Smith", null, 42.0);
            recruitmentSystem.AddContractor(newContractor);

            //Act - By First Name
            recruitmentSystem.SortContractorsByFirstName();
            List<Contractor> resultContractors = recruitmentSystem.GetContractors();

            //Assert - By First Name
            Assert.AreEqual("Alice", resultContractors[0].FirstName.ToString());
        }
        [TestMethod]
        public void SortContractors_UpdateContractorsCollection_ByLastName()
        {
            //Arrange
            Contractor newContractor = new(Guid.NewGuid().ToString(), "Alice", "Smith", null, 42.0);
            recruitmentSystem.AddContractor(newContractor);

            //Act - Last Name
            recruitmentSystem.SortContractorsByLastName();
            List<Contractor>resultContractors = recruitmentSystem.GetContractors();

            //Assert - Last Name
            Assert.AreEqual("Johnson", resultContractors[0].LastName.ToString());
        }
        public void SortContractors_UpdateContractorsCollection_ByHourlyWage()
        {
            //Arrange
            Contractor newContractor = new(Guid.NewGuid().ToString(), "Alice", "Smith", null, 42.0);
            recruitmentSystem.AddContractor(newContractor);

            //Act - Hourly Wage
            recruitmentSystem.SortContractorsByHourlyWage();
            List<Contractor>resultContractors = recruitmentSystem.GetContractors();

            //Assert - Hourly Wage
            Assert.AreEqual(38.5, resultContractors[0].HourlyWage);
        }
        [TestMethod]
        public void AssignContractorToJob_ContractorStartDate_NotNull()
        {
            //Arrange
            Contractor newContractor1 = new(Guid.NewGuid().ToString(), "Alice", "Smith", null, 42.0);
            Contractor newContractor2 = new(Guid.NewGuid().ToString(), "Edward", "Wilson", null, 39.0);
            recruitmentSystem.AddContractor(newContractor1);
            recruitmentSystem.AddContractor(newContractor2);
            Job newJob = new Job(Guid.NewGuid().ToString(), "Elec' Maintenance", DateTime.Now, 250.0, null, "Not Assigned", "-", null, null);
            
            //Act
            recruitmentSystem.AssignContractorToAJob(newContractor1, newJob);

            //Assert
            Assert.IsNotNull(newContractor1.StartDate);
        }
        [TestMethod]
        public void AssignContractorToJob_ReturnsCollection()
        {
            //Arrange
            Contractor newContractor = new(Guid.NewGuid().ToString(), "Alice", "Smith", null, 42.0);
            Contractor newContractor2 = new(Guid.NewGuid().ToString(), "Edward", "Wilson", null, 39.0);
            recruitmentSystem.AddContractor(newContractor);
            recruitmentSystem.AddContractor(newContractor2);
            Job newJob = new Job(Guid.NewGuid().ToString(), "Elec' Maintenance", DateTime.Now, 250.0, null, "Not Assigned", "-", null, null);

            //Act
            recruitmentSystem.AssignContractorToAJob(newContractor, newJob);
            List<Contractor> resultContractors = recruitmentSystem.GetContractors();

            //Assert
            Assert.AreEqual(4, resultContractors.Count);
        }
        [TestMethod]
        public void FilterContractors_UpdateContractorsCollection_ShowAvailableOnly()
        {
            //Arrange
            Contractor newContractor = new(Guid.NewGuid().ToString(), "Alice", "Smith", null, 42.0);
            Contractor newContractor2 = new(Guid.NewGuid().ToString(), "Edward", "Wilson", null, 39.0);
            recruitmentSystem.AddContractor(newContractor);
            recruitmentSystem.AddContractor(newContractor2);
            Job newJob = new Job(Guid.NewGuid().ToString(), "Elec' Maintenance", DateTime.Now, 250.0, null, "Not Assigned", "-", null, null);

            //Act - ShowAvailableOnly
            recruitmentSystem.AssignContractorToAJob(newContractor, newJob);
            recruitmentSystem.ShowAvailableContractorsOnly();
            List<Contractor>resultContractors = recruitmentSystem.GetContractors();

            //Assert - ShowAvailableOnly
            Assert.AreEqual(3, resultContractors.Count);
        }
        [TestMethod]
        public void FilterContractors_UpdateContractorsCollection_ShowAllContractors()
        {
            //Arrange
            Contractor newContractor = new(Guid.NewGuid().ToString(), "Alice", "Smith", null, 42.0);
            Contractor newContractor2 = new(Guid.NewGuid().ToString(), "Edward", "Wilson", null, 39.0);
            recruitmentSystem.AddContractor(newContractor);
            recruitmentSystem.AddContractor(newContractor2);
            Job newJob = new Job(Guid.NewGuid().ToString(), "Elec' Maintenance", DateTime.Now, 250.0, null, "Not Assigned", "-", null, null);
            recruitmentSystem.AssignContractorToAJob(newContractor, newJob);
            recruitmentSystem.ShowAvailableContractorsOnly();
            List<Contractor> availableContractors = recruitmentSystem.GetContractors();
            Assert.AreEqual(3, availableContractors.Count);


            //Act - ShowAllContractors
            recruitmentSystem.ShowAllContractors();
            List<Contractor> resultContractors = recruitmentSystem.GetContractors();

            //Assert - ShowAllContractors
            Assert.AreEqual(4, resultContractors.Count);
        }
    }
}
