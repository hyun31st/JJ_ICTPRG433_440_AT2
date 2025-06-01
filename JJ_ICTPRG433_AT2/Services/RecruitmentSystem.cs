using JJ_ICTPRG433_440_AT2.Models;
using JJ_ICTPRG433_440_AT2.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace JJ_ICTPRG433_440_AT2.Services
{
    /// <summary>
    /// Handles core logic for managing contractors and jobs, including adding, removing and assigning contractors to jobs.
    /// </summary>
    class RecruitmentSystem
    {
        private List<Job> _jobs = [];
        private List<Job> filteredJobs = [];
        private List<Job> outOfRangeFilteredJobs = [];
        private List<Contractor> _contractors = [];
        private List<Contractor> filteredContractors = [];

        /// <summary>
        /// Adds a new contractor from the user input or predefined examples.
        /// </summary>
        /// <param name="contractorID">The unique identifier for the contractor.</param>
        /// <param name="contractorFirstName">The first name of the contractor.</param>
        /// <param name="contractorLastName">The last name of the contractor.</param>
        /// <param name="startDate">The start date for the contractor's assignment, or null if not assigned.</param>
        /// <param name="contractorHourlyWage">The hourly wage rate for the contractor.</param>
        public void AddContractor(string contractorID, string contractorFirstName, string contractorLastName, DateTime? startDate, double contractorHourlyWage)
        {
            Contractor newContractor = new Contractor(contractorID, contractorFirstName, contractorLastName, null, contractorHourlyWage);
            _contractors.Add(newContractor);
        }

        /// <summary>
        /// Removes the selected contractor from the list if they are not currently assigned to any job.
        /// </summary>
        /// <param name="newContractor">The contractor to be removed.</param>
        public void RemoveContractor(Contractor newContractor)
        {
            foreach (var job in _jobs)
            {
                if (newContractor.Id == job.ContractorId)
                {
                    if (job.ContractorAssigned.StartsWith("Yes"))
                    {
                        MessageBox.Show($"{newContractor.FirstName} {newContractor.LastName} is currently assigned to \"{job.Title}\"." +
                            $"\nPlease complete the job before removing the contractor.", "Cannot Remove Contractor", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
            }
            _contractors.Remove(newContractor);
        }

        /// <summary>
        /// Returns the complete list of contractors.
        /// </summary>
        /// <returns>A list of contractors.</returns>
        public List<Contractor> GetContractors()
        {
            return _contractors;
        }

        /// <summary>
        /// Adds a new job from the user input or predefined examples.
        /// </summary>
        /// <param name="id">The unique identifier for the job.</param>
        /// <param name="title">The title of the job.</param>
        /// <param name="createDate">The created date for the job.</param>
        /// <param name="cost">The cost of the job.</param>
        /// <param name="completeDate">The completed date for the job.</param>
        /// <param name="contractorAssigned">The current assignment status of the job (e.g., 'Not Assigned', 'Yes', or 'Completed').</param>
        /// <param name="remark">Additional remarks or notes about the job.</param>
        /// <param name="contractorID">The assigned contractor's ID.</param>
        /// <param name="contractorHours">The number of hours worked by the contractor.</param>
        public void AddJob(string id, string title, DateTime? createDate, double cost, DateTime? completeDate, string contractorAssigned, string remark, string? contractorID, double? contractorHours)
        {
            Job newJob = new Job(id, title, createDate, cost, completeDate, contractorAssigned, remark, contractorID, contractorHours);
            _jobs.Add(newJob);
        }

        /// <summary>
        /// Removes the selected job from the list. 
        /// If a contractor is assigned and the job is in progress, the contractor will be marked as available by setting start date to null.
        /// </summary>
        /// <param name="newJob">The job to be removed.</param>
        public void RemoveJob(Job newJob)
        {
            if (newJob.ContractorAssigned.StartsWith("Yes"))
            {
                Contractor contractor = (Contractor)GetContractorByID(newJob.ContractorId);
                MessageBox.Show($"{contractor.FirstName} {contractor.LastName} is currently assigned to \"{newJob.Title}\"." +
                            $"\nThe contractor will be marked as available.", "Job Removal Notice", MessageBoxButton.OK, MessageBoxImage.Warning);
                contractor.StartDate = null;
            }   
            _jobs.Remove(newJob);
        }

        /// <summary>
        /// Returns the complete list of jobs.
        /// </summary>
        /// <returns>A list of jobs.</returns>
        public List<Job> GetJobs()
        {
            return _jobs;
        }

        /// <summary>
        /// Sorts the contractors by the first name.
        /// </summary>
        public void SortContractorsByFirstName()
        {
            _contractors = _contractors.OrderBy(c => c.FirstName).ToList();
        }

        /// <summary>
        /// Sorts the contractors by the last name.
        /// </summary>
        public void SortContractorsByLastName()
        {
            _contractors = _contractors.OrderBy(c => c.LastName).ToList();
        }

        /// <summary>
        /// Sorts the contractors by the hourly wage.
        /// </summary>
        public void SortContractorsByHourlyWage()
        {
            _contractors = _contractors.OrderBy(c => c.HourlyWage).ToList();
        }

        /// <summary>
        /// Filters the contractor list to show only available contractors (those without a start date).
        /// Unavailable contractors are temporarily stored in <c>filteredContractors</c>
        /// </summary>
        public void ShowAvailableContractorsOnly()
        {
            filteredContractors = _contractors.Where(x => x.StartDate != null).ToList();
            _contractors = _contractors.Where(c => c.StartDate == null).ToList();
        }

        /// <summary>
        /// Restores <c>filteredContractors</c> to the main contractor list.
        /// Then sorts the contractors by the first name.
        /// </summary>
        public void ShowAllContractors()
        {
            foreach (var contractor in filteredContractors)
            {
                _contractors.Add(contractor);
            }
            filteredContractors.Clear();
            SortContractorsByFirstName();
        }

        /// <summary>
        /// Sorts the jobs by the title.
        /// </summary>
        public void SortJobsByTitle()
        {
            _jobs = _jobs.OrderBy(j => j.Title).ToList();
        }

        /// <summary>
        /// Sorts the jobs by the cost.
        /// </summary>
        public void SortJobsByCost()
        {
            _jobs = _jobs.OrderBy(j => j.Cost).ToList();
        }

        /// <summary>
        /// Filters the job list to show only available jobs (e.g. jobs where <c>contractorAssigned</c> is "Not Assigned").
        /// Completed or in-progress jobs are temporarily stored in <c>filteredJobs</c>
        /// </summary>
        public void ShowAvailableJobsOnly()
        {
            ShowAllJobs();
            foreach (var job in _jobs.Where(j => j.ContractorAssigned != "Not Assigned").ToList())
            {
                filteredJobs.Add(job);
            }
            _jobs = _jobs.Where(j => j.ContractorAssigned == "Not Assigned").ToList();
        }

        /// <summary>
        /// Filters the job list to show only in-progress jobs (e.g. jobs where <c>contractorAssigned</c> is "Yes").
        /// Completed or available jobs are temporarily stored in <c>filteredJobs</c>
        /// </summary>
        public void ShowInProgressJobsOnly()
        {
            ShowAllJobs();
            foreach (var job in _jobs.Where(j => j.ContractorAssigned != "Yes").ToList())
            {
                filteredJobs.Add(job);
            }
            _jobs = _jobs.Where(j => j.ContractorAssigned == "Yes").ToList();
        }

        /// <summary>
        /// Filters the job list to show only completed jobs (e.g. jobs where <c>contractorAssigned</c> is "Completed").
        /// In-progress or available jobs are temporarily stored in <c>filteredJobs</c>
        /// </summary>
        public void ShowCompletedJobsOnly()
        {
            ShowAllJobs();
            foreach (var job in _jobs.Where(j => j.ContractorAssigned != "Completed").ToList())
            {
                filteredJobs.Add(job);
            }
            _jobs = _jobs.Where(j => j.ContractorAssigned == "Completed").ToList();
        }

        /// <summary>
        /// Filters the job list to show only jobs within user selected cost range.
        /// Jobs outside the selected range are temporarily stored in <c>outOfRangeFilteredJobs</c>
        /// </summary>
        public void JobsFilterbyCostRange()
        {
            AddJobFilterbyCostRange addJobFilterbyCostRangeWindow = new AddJobFilterbyCostRange();
            if (addJobFilterbyCostRangeWindow.ShowDialog() == true)
            {
                foreach (var job in _jobs.Where(j => j.Cost < addJobFilterbyCostRangeWindow.MinCostRange || j.Cost > addJobFilterbyCostRangeWindow.MaxCostRange).ToList())
                {
                    outOfRangeFilteredJobs.Add(job);
                }
                _jobs = _jobs.Where(j => j.Cost >= addJobFilterbyCostRangeWindow.MinCostRange && j.Cost <= addJobFilterbyCostRangeWindow.MaxCostRange).ToList();
            }
        }

        /// <summary>
        /// Restores <c>filteredJobs</c> & <c>outOfRangeFilteredJobs</c> to the main job list.
        /// Then sorts the jobs by the created date.
        /// </summary>
        public void ShowAllJobs()
        {
            foreach (var job in filteredJobs)
            {
                _jobs.Add(job);
            }
            foreach (var job in outOfRangeFilteredJobs)
            {
                _jobs.Add(job);
            }
            filteredJobs.Clear();
            outOfRangeFilteredJobs.Clear();
            _jobs = _jobs.OrderBy(j => j.CreateDate).ToList();
        }

        /// <summary>
        /// Assigns a selected contractor to a selected job if both are valid and unassigned.
        /// </summary>
        /// <param name="selectedContractor">The contractor to be assigned. Must not be null or already assigned to another job.</param>
        /// <param name="selectedJob">The job to assign the contractor to. Must not be null or already in progress or completed.</param>
        public void AssignContractorToAJob(Contractor selectedContractor, Job selectedJob)
        {
            if (selectedContractor == null)
            {
                MessageBox.Show("Please select a contractor from the list before proceeding.", "Contractor Not Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selectedJob == null)
            {
                MessageBox.Show("Please select a job from the list before assigning a contractor.", "Job Not Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Verify that the selected job is not already assigned to a contractor
            if (selectedJob.ContractorAssigned.StartsWith("Not Assigned"))
            {
                // Check if the selected contractor is already assigned to another job
                if (IsContractorAssignedToJob(selectedContractor))
                {
                    // Assign the contractor to the job and set their start date
                    selectedContractor.StartDate = DateTime.Now;
                    selectedJob.ContractorAssigned = "Yes";
                    selectedJob.ContractorId = selectedContractor.Id;
                    selectedJob.Remark = "In progress-" + selectedContractor.FirstName + " " + selectedContractor.LastName;
                }
                else
                {
                    MessageBox.Show($"{selectedContractor.FirstName} {selectedContractor.LastName} is already assigned to another job. Please select a different contractor.",
                        "Contractor Already Assigned", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            else
            {
                MessageBox.Show($"{selectedJob.Title} is already in process or completed. Please select another job.",
                        "Job Already Assigned", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

        }

        /// <summary>
        /// Marks the selected job as completed and records contractor work hours.
        /// The assigned contractor is returning to the available pool.
        /// </summary>
        /// <param name="selectedJob">The job to be completed.</param>
        public void CompleteJob(Job selectedJob)
        {
            // Check if the job is already completed
            if (selectedJob.ContractorAssigned.StartsWith("Completed"))
            {
                MessageBox.Show($"The job \"{selectedJob.Title}\" has already been completed. Please select a different job to proceed.",
                    "Job Already Completed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            // Check if the job has a contractor assigned
            else if (!selectedJob.ContractorAssigned.StartsWith("Not Assigned"))
            {
                Contractor contractor = (Contractor)GetContractorByID(selectedJob.ContractorId);

                // Get user input how many hours contractor spent
                AddContractorHours addContractorHours = new AddContractorHours(selectedJob.Title, contractor.FirstName, contractor.LastName, contractor.HourlyWage);
                if (addContractorHours.ShowDialog() == true)
                {

                    // Rreturning the contractor to the available pool
                    contractor.StartDate = null;

                    selectedJob.CompleteDate = DateTime.Now;
                    selectedJob.ContractorAssigned = "Completed";

                    string remark = selectedJob.Remark;
                    remark = remark.Replace("In progress-", "");
                    selectedJob.ContractorHours = addContractorHours.HoursWorkedByContractor;
                    selectedJob.Remark = remark + " ";

                }
            }
            // Job is not yet assigned
            else
            {
                MessageBox.Show($"The job \"{selectedJob.Title}\" does not have a contractor assigned. Please assign a contractor before marking the job as complete.",
                    "Contractor Not Assigned", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Creates a CSV report containing the currently visible jobs in the list.
        /// </summary>
        public void JobCreateReport()
        {
            MessageBoxResult result = MessageBox.Show("Do you want to create the report in CSV format?\nOnly visible items in the ListView will be exported.", "Export to CSV", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                string writeText = "Title,Created Date,Cost,Completed Date,Contractor Assigned,Remark,Worked Hours by Contractor\n";
                string dateTimeNow = DateTime.Now.ToString("yyyyMMdd HHmmss");
                string reportName = $"Job Report - {dateTimeNow}.csv";
                foreach (var job in _jobs)
                {
                    writeText += job.Title + "," + job.CreateDate + "," + job.Cost + "," + job.CompleteDate + "," + job.ContractorAssigned + "," + job.Remark + "," + job.ContractorHours + "\n";

                }
                File.WriteAllText(reportName, writeText);

                MessageBox.Show($"{reportName} has been successfully created.\nFile location:\n{Path.GetFullPath(reportName)}.", "Report Created");
            }
        }

        /// <summary>
        /// Determines whether the specified contractor is currently available.
        /// </summary>
        /// <param name="selectedContractor">The contractor to check</param>
        /// <returns>
        /// True, if the contractor is not assigned to any in-progress job
        /// False, if the contractor is assigned to a job marked as in-progress.
        /// </returns>
        public bool IsContractorAssignedToJob(Contractor selectedContractor)
        {
            foreach (var job in _jobs)
            {
                if (selectedContractor.Id == job.ContractorId)
                {
                    if (job.ContractorAssigned.StartsWith("Yes"))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Retrieves a contractor from the list by their unique ID.
        /// </summary>
        /// <param name="contractorID">The contractor's ID.</param>
        /// <returns>The object that matches the contractor's ID. 
        /// Otherwise, it returns null. However the contractor's ID is expected to correspond to a contractor already in the current list.</returns>
        public Contractor GetContractorByID(string contractorID)
        {
            foreach (var item in _contractors)
            {
                if (contractorID == item.Id)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// Adds 10 example contractors.
        /// </summary>
        public void AddContractorExamples()
        {
            // Add 10 sample contractors
            AddContractor(Guid.NewGuid().ToString(), "Alice", "Smith", null, 42.0);
            AddContractor(Guid.NewGuid().ToString(), "Bob", "Johnson", null, 38.5);
            AddContractor(Guid.NewGuid().ToString(), "Charlie", "Lee", null, 45.0);
            AddContractor(Guid.NewGuid().ToString(), "Diana", "Brown", null, 41.0);
            AddContractor(Guid.NewGuid().ToString(), "Edward", "Wilson", null, 39.0);
            AddContractor(Guid.NewGuid().ToString(), "Fiona", "Davis", null, 43.5);
            AddContractor(Guid.NewGuid().ToString(), "George", "Taylor", null, 40.0);
            AddContractor(Guid.NewGuid().ToString(), "Helen", "Martin", null, 44.0);
            AddContractor(Guid.NewGuid().ToString(), "Ian", "Walker", null, 37.5);
            AddContractor(Guid.NewGuid().ToString(), "Julia", "White", null, 46.0);
        }

        /// <summary>
        /// Adds 10 example jobs.
        /// </summary>
        public void AddJobxamples()
        {
            // Add 10 sample jobs
            AddJob(Guid.NewGuid().ToString(), "Carpentry Work", DateTime.Now, 240.0, null, "Not Assigned", "-", null, null);
            AddJob(Guid.NewGuid().ToString(), "Elec' Maintenance", DateTime.Now, 250.0, null, "Not Assigned", "-", null, null);
            AddJob(Guid.NewGuid().ToString(), "HVAC Installation", DateTime.Now, 300.0, null, "Not Assigned", "-", null, null);
            AddJob(Guid.NewGuid().ToString(), "Painting Project", DateTime.Now, 220.0, null, "Not Assigned", "-", null, null);
            AddJob(Guid.NewGuid().ToString(), "Plumbing Repair", DateTime.Now, 180.0, null, "Not Assigned", "-", null, null);
            AddJob(Guid.NewGuid().ToString(), "Roof Inspection", DateTime.Now, 195.0, null, "Not Assigned", "-", null, null);
            AddJob(Guid.NewGuid().ToString(), "Win' Replacement", DateTime.Now, 275.0, null, "Not Assigned", "-", null, null);
            AddJob(Guid.NewGuid().ToString(), "Flooring Upgrade", DateTime.Now, 210.0, null, "Not Assigned", "-", null, null);
            AddJob(Guid.NewGuid().ToString(), "Gate Installation", DateTime.Now, 260.0, null, "Not Assigned", "-", null, null);
            AddJob(Guid.NewGuid().ToString(), "Lighting Setup", DateTime.Now, 230.0, null, "Not Assigned", "-", null, null);
        }
    }

}
