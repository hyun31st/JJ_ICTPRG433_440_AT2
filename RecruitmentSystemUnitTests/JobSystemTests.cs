using JJ_ICTPRG433_440_AT2.Models;
using JJ_ICTPRG433_440_AT2.Services;

namespace RecruitmentSystemUnitTests

{
    [TestClass]
    public sealed class JobSystemTests
    {
        private RecruitmentSystem recruitmentSystem;
        [TestInitialize]
        public void Setup()
        {
            //Arrange
            recruitmentSystem = new([new Job(Guid.NewGuid().ToString(), "HVAC Installation", DateTime.Now, 240.0, null, "Not Assigned", "-", null, null),
                                     new Job(Guid.NewGuid().ToString(), "Elec' Maintenance", DateTime.Now, 180.0, null, "Not Assigned", "-", null, null)]
                                     , null);

        }
        [TestMethod]
        public void GetJobs_ReturnsCollection_NotNull()
        {
            //Arrange
            //Setup()

            //Act
            List<Job> resultJobs = recruitmentSystem.GetJobs();

            //Assert
            Assert.IsNotNull(resultJobs);
        }
        [TestMethod]
        public void GetJobs_ReturnsCollection()
        {
            //Arrange
            //Setup()

            //Act
            List<Job> resultJobs = recruitmentSystem.GetJobs();

            //Assert
            Assert.IsInstanceOfType<List<Job>>(resultJobs);
            CollectionAssert.AllItemsAreInstancesOfType(resultJobs, typeof(Job));
            Assert.AreEqual(2, resultJobs.Count);
        }
        [TestMethod]
        public void AddJobs_UpdateJobsCollection_ContainsReference()
        {
            //Arrange
            //Setup()

            Job newJob = new(Guid.NewGuid().ToString(), "A_HVAC Installation", DateTime.Now, 300.0, null, "Not Assigned", "-", null, null);

            //Act
            recruitmentSystem.AddJob(newJob);
            List<Job> resultJobs = recruitmentSystem.GetJobs();

            //Assert
            CollectionAssert.Contains(resultJobs, newJob);
        }
        [TestMethod]
        public void AddJobs_UpdateJobsCollection_ContainsThreeObjects()
        {
            //Arrange
            //Setup()

            Job newJob = new(Guid.NewGuid().ToString(), "A_HVAC Installation", DateTime.Now, 300.0, null, "Not Assigned", "-", null, null);

            //Act
            recruitmentSystem.AddJob(newJob);
            List<Job> resultJobs = recruitmentSystem.GetJobs();

            //Assert
            Assert.AreEqual(3, resultJobs.Count);
        }
        [TestMethod]
        public void RemoveJobs_UpdateJobsCollection_ContainsTwoObjects()
        {
            //Arrange
            //Setup()
            Job newJob = new(Guid.NewGuid().ToString(), "TEST03_HVAC Installation", DateTime.Now, 300.0, null, "Not Assigned", "-", null, null);

            //Act
            recruitmentSystem.AddJob(newJob);
            recruitmentSystem.RemoveJob(newJob);
            List<Job> resultJobs = recruitmentSystem.GetJobs();

            //Assert
            Assert.AreEqual(2, resultJobs.Count);
        }
        [TestMethod]
        public void SortJobs_UpdateJobsCollection_ByTitleAndCostAndCostRange()
        {
            //Arrange
            //Setup()

            Job newJob = new(Guid.NewGuid().ToString(), "Carpentry Work", DateTime.Now, 210.0, null, "Not Assigned", "-", null, null);
            recruitmentSystem.AddJob(newJob);

            //Act - By Title
            recruitmentSystem.SortJobsByTitle();
            List<Job> resultJobs = recruitmentSystem.GetJobs();

            //Assert - By Title
            Assert.AreEqual("Carpentry Work", resultJobs[0].Title.ToString());


            //Act - By Cost
            recruitmentSystem.SortJobsByCost();
            resultJobs = recruitmentSystem.GetJobs();

            //Assert - By Title
            Assert.AreEqual("180", resultJobs[0].Cost.ToString());


            //Act - By Cost Range
            recruitmentSystem.JobsFilterbyCostRange(180,210);
            resultJobs = recruitmentSystem.GetJobs();

            //Assert - By Cost Range
            Assert.AreEqual(2, resultJobs.Count);
        }
        [TestMethod]
        public void AssignContractorToJob_UpdateJobStatus()
        {
            //Arrange
            Contractor newContractor1 = new(Guid.NewGuid().ToString(), "Alice", "Smith", null, 42.0);
            recruitmentSystem.AddContractor(newContractor1);
            Job newJob1 = new(Guid.NewGuid().ToString(), "Carpentry Work", DateTime.Now, 210.0, null, "Not Assigned", "-", null, null);
            recruitmentSystem.AddJob(newJob1);

            //Act
            recruitmentSystem.AssignContractorToAJob(newContractor1, newJob1);

            //Assert
            Assert.AreEqual("Yes", newJob1.ContractorAssigned.ToString());
            StringAssert.StartsWith(newJob1.Remark.ToString(), "In progress-");
        }
        [TestMethod]
        public void GetContractorById_ReturnsMatchingContractor_AssignedToJob()
        {
            //Arrange
            Contractor newContractor1 = new(Guid.NewGuid().ToString(), "Alice", "Smith", null, 42.0);
            Contractor newContractor2 = new(Guid.NewGuid().ToString(), "Edward", "Wilson", null, 39.0);
            recruitmentSystem.AddContractor(newContractor1);
            recruitmentSystem.AddContractor(newContractor2);

            Job newJob1 = new(Guid.NewGuid().ToString(), "Carpentry Work", DateTime.Now, 210.0, null, "Not Assigned", "-", null, null);
            Job newJob2 = new(Guid.NewGuid().ToString(), "Flooring Upgrade", DateTime.Now, 275.0, null, "Not Assigned", "-", null, null);
            recruitmentSystem.AddJob(newJob1);
            recruitmentSystem.AddJob(newJob2);

            //Act
            recruitmentSystem.AssignContractorToAJob(newContractor1, newJob1);
            recruitmentSystem.AssignContractorToAJob(newContractor2, newJob2);

            Contractor selectedContractor1 = (Contractor)recruitmentSystem.GetContractorByID(newJob1.ContractorId);
            Contractor selectedContractor2 = (Contractor)recruitmentSystem.GetContractorByID(newJob2.ContractorId);

            //Assert
            Assert.AreEqual(newContractor1, selectedContractor1);
            Assert.AreNotEqual(newContractor1, selectedContractor2);
        }
        [TestMethod]
        public void IsContractorAssignedToJob_ReturnsTrueIfAssigned_FalseIfNotAssigned()
        {
            //Arrange
            Contractor newContractor1 = new(Guid.NewGuid().ToString(), "Alice", "Smith", null, 42.0);
            Contractor newContractor2 = new(Guid.NewGuid().ToString(), "Edward", "Wilson", null, 39.0);
            recruitmentSystem.AddContractor(newContractor1);
            recruitmentSystem.AddContractor(newContractor2);

            Job newJob1 = new(Guid.NewGuid().ToString(), "Carpentry Work", DateTime.Now, 210.0, null, "Not Assigned", "-", null, null);
            Job newJob2 = new(Guid.NewGuid().ToString(), "Flooring Upgrade", DateTime.Now, 275.0, null, "Not Assigned", "-", null, null);
            recruitmentSystem.AddJob(newJob1);
            recruitmentSystem.AddJob(newJob2);

            //Act
            recruitmentSystem.AssignContractorToAJob(newContractor1, newJob1);

            bool isAssigned = recruitmentSystem.IsContractorAssignedToJob(newContractor1);
            bool isNotAssigned = recruitmentSystem.IsContractorAssignedToJob(newContractor2);

            //Assert
            Assert.AreEqual(true, isAssigned);
            Assert.AreEqual(false, isNotAssigned);
        }
        [TestMethod]
        public void CompleteJob_UpdatesJobAndContractorStatus_SetsCompletionDetails()
        {
            //Arrange
            Contractor newContractor1 = new(Guid.NewGuid().ToString(), "Alice", "Smith", null, 42.0);
            Contractor newContractor2 = new(Guid.NewGuid().ToString(), "Edward", "Wilson", null, 39.0);
            recruitmentSystem.AddContractor(newContractor1);
            recruitmentSystem.AddContractor(newContractor2);

            Job newJob1 = new(Guid.NewGuid().ToString(), "Carpentry Work", DateTime.Now, 210.0, null, "Not Assigned", "-", null, null);
            Job newJob2 = new(Guid.NewGuid().ToString(), "Flooring Upgrade", DateTime.Now, 275.0, null, "Not Assigned", "-", null, null);
            recruitmentSystem.AddJob(newJob1);
            recruitmentSystem.AddJob(newJob2);

            //Act
            recruitmentSystem.AssignContractorToAJob(newContractor1, newJob1);
            recruitmentSystem.AssignContractorToAJob(newContractor2, newJob2);

            Contractor selectedContractor1 = (Contractor)recruitmentSystem.GetContractorByID(newJob1.ContractorId);
            Contractor selectedContractor2 = (Contractor)recruitmentSystem.GetContractorByID(newJob2.ContractorId);

            double hoursWorkedbyContractor = 5;
            recruitmentSystem.CompleteJob(newJob1, selectedContractor1, hoursWorkedbyContractor);

            //Assert
            Assert.IsNull(selectedContractor1.StartDate);
            Assert.IsNotNull(newJob1.CompleteDate);
            Assert.AreEqual("Completed", newJob1.ContractorAssigned);
            Assert.AreEqual(hoursWorkedbyContractor, newJob1.ContractorHours);
            StringAssert.StartsWith(newJob1.Remark.ToString(), selectedContractor1.FirstName);
        }
        [TestMethod]
        public void FilterJobs_UpdateJobsCollection_ShowAvailableAndInProgressAndCompleted()
        {
            //Arrange
            Contractor newContractor1 = new(Guid.NewGuid().ToString(), "Alice", "Smith", null, 42.0);
            Contractor newContractor2 = new(Guid.NewGuid().ToString(), "Edward", "Wilson", null, 39.0);
            recruitmentSystem.AddContractor(newContractor1);
            recruitmentSystem.AddContractor(newContractor2);

            Job newJob1 = new(Guid.NewGuid().ToString(), "Carpentry Work", DateTime.Now, 210.0, null, "Not Assigned", "-", null, null);
            Job newJob2 = new(Guid.NewGuid().ToString(), "Flooring Upgrade", DateTime.Now, 275.0, null, "Not Assigned", "-", null, null);
            recruitmentSystem.AddJob(newJob1);
            recruitmentSystem.AddJob(newJob2);

            recruitmentSystem.AssignContractorToAJob(newContractor1, newJob1);
            recruitmentSystem.AssignContractorToAJob(newContractor2, newJob2);

            Contractor selectedContractor1 = (Contractor)recruitmentSystem.GetContractorByID(newJob1.ContractorId);
            Contractor selectedContractor2 = (Contractor)recruitmentSystem.GetContractorByID(newJob2.ContractorId);

            double hoursWorkedbyContractor = 5;
            recruitmentSystem.CompleteJob(newJob1, selectedContractor1, hoursWorkedbyContractor);

            //Act - Show Available Jobs Only
            recruitmentSystem.ShowAvailableJobsOnly();
            List<Job> resultJobs = recruitmentSystem.GetJobs();

            //Assert - Show Available Jobs Only
            Assert.AreEqual(2, resultJobs.Count);
            Assert.AreNotEqual(1, resultJobs.Count);


            //Act - Show In Progress Jobs Only
            recruitmentSystem.ShowInProgressJobsOnly();
            resultJobs = recruitmentSystem.GetJobs();

            //Assert - Show In Progress Jobs Only
            Assert.AreEqual(1, resultJobs.Count);
            Assert.AreNotEqual(2, resultJobs.Count);


            //Act - Show Completed Jobs Only
            recruitmentSystem.ShowCompletedJobsOnly();
            resultJobs = recruitmentSystem.GetJobs();

            //Assert - Show Completed Jobs Only
            Assert.AreEqual(1, resultJobs.Count);
            Assert.AreNotEqual(2, resultJobs.Count);


            //Act - Show All Jobs
            recruitmentSystem.ShowAllJobs();
            resultJobs = recruitmentSystem.GetJobs();

            //Assert - Show All Jobs
            Assert.AreEqual(4, resultJobs.Count);
            Assert.AreNotEqual(1, resultJobs.Count);
        }
        [TestMethod]
        public void JobCreateReport_CreatesCsvFile_WithCorrectContent()
        {
            //Arrange
            //Setup()

            //Act
            string filePath;
            filePath = recruitmentSystem.JobCreateReport();

            //Assert
            Assert.IsTrue(File.Exists(filePath));
            string[] lines = File.ReadAllLines(filePath);
            Assert.IsTrue(lines[1].StartsWith("HVAC Installation"));
        }
    }
}
