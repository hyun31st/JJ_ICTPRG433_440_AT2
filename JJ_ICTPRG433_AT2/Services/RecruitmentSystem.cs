using JJ_ICTPRG433_440_AT2.Models;
using JJ_ICTPRG433_440_AT2.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JJ_ICTPRG433_440_AT2.Services
{
    /// <summary>
    /// Handles core logic for managing contractors and jobs, including adding, removing and assigning contractors to jobs.
    /// </summary>
    public class RecruitmentSystem
    {
        private List<Job> _jobs = [];
        private List<Job> filteredJobs = [];
        private List<Job> outOfRangeFilteredJobs = [];
        private List<Contractor> _contractors = [];
        private List<Contractor> filteredContractors = [];

        public RecruitmentSystem()
        {

        }

        public RecruitmentSystem(List<Job>? _jobs = null, List<Contractor>? _contractors = null)
        {
            this._jobs = _jobs ?? [];
            this._contractors = _contractors ?? [];
        }
        /// <summary>
        /// Adds a new contractor from the user input or predefined examples.
        /// </summary>
        /// <param name="newContractor">The contractor instance to add, either created from user input.</param>
        public void AddContractor(Contractor newContractor)
        {
            _contractors.Add(newContractor);
        }

        /// <summary>
        /// Removes the selected contractor from the list if they are not currently assigned to any job.
        /// </summary>
        /// <param name="newContractor">The contractor to be removed.</param>
        public (bool sucess, string contractorFirstName, string contractorLastName, string jobTitle) RemoveContractor(Contractor newContractor)
        {
            foreach (var job in _jobs)
            {
                if (newContractor.Id == job.ContractorId)
                {
                    if (job.ContractorAssigned.StartsWith("Yes"))
                    {
                        return (false, newContractor.FirstName, newContractor.LastName, job.Title);
                    }
                }
            }
            _contractors.Remove(newContractor);
            return (true, string.Empty, string.Empty, string.Empty);
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
        /// <param name="newJob">The job instance to add, either created from user input.</param>
        public void AddJob(Job newJob)
        {
            _jobs.Add(newJob);
        }

        /// <summary>
        /// Removes the selected job from the list. 
        /// If a contractor is assigned and the job is in progress, the contractor will be marked as available by setting start date to null.
        /// </summary>
        /// <param name="newJob">The job to be removed.</param>
        public void RemoveJob(Job newJob)
        {
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
        /// <param name="minCostRange">The minimum job cost to include.</param>
        /// <param name="maxCostRange">The maximum job cost to include.</param>
        public void JobsFilterbyCostRange(double minCostRange, double maxCostRange)
        {

            foreach (var job in _jobs.Where(j => j.Cost < minCostRange || j.Cost > maxCostRange).ToList())
            {
                outOfRangeFilteredJobs.Add(job);
            }
            _jobs = _jobs.Where(j => j.Cost >= minCostRange && j.Cost <= maxCostRange).ToList();
            
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
            // Assign the contractor to the job and set their start date
            selectedContractor.StartDate = DateTime.Now;
            selectedJob.ContractorAssigned = "Yes";
            selectedJob.ContractorId = selectedContractor.Id;
            selectedJob.Remark = "In progress-" + selectedContractor.FirstName + " " + selectedContractor.LastName;
        }

        /// <summary>
        /// Marks the selected job as completed and records contractor work hours.
        /// The assigned contractor is returning to the available pool.
        /// </summary>
        /// <param name="selectedJob">The job to be completed.</param>
        /// <param name="assignedContractor">The contractor assigned to the job; their start date will be cleared.(Rreturning the contractor to the available pool)</param>
        /// <param name="HoursWorkedByContractor">The number of hours the contractor worked on this job.</param>
        public void CompleteJob(Job selectedJob, Contractor assignedContractor, double HoursWorkedByContractor)
        {
            // Rreturning the contractor to the available pool
            assignedContractor.StartDate = null;

            selectedJob.CompleteDate = DateTime.Now;
            selectedJob.ContractorAssigned = "Completed";

            string remark = selectedJob.Remark;
            remark = remark.Replace("In progress-", "");
            selectedJob.ContractorHours = HoursWorkedByContractor;
            selectedJob.Remark = remark + " ";
        }

        /// <summary>
        /// Creates a CSV report containing the currently visible jobs in the list.
        /// </summary>
        public string JobCreateReport()
        {
            string writeText = "Title,Created Date,Cost,Completed Date,Contractor Assigned,Remark,Worked Hours by Contractor\n";
            string dateTimeNow = DateTime.Now.ToString("yyyyMMdd HHmmss");
            string reportName = $"Job Report - {dateTimeNow}.csv";
            foreach (var job in _jobs)
            {
                writeText += job.Title + "," + job.CreateDate + "," + job.Cost + "," + job.CompleteDate + "," + job.ContractorAssigned + "," + job.Remark + "," + job.ContractorHours + "\n";

            }
            File.WriteAllText(reportName, writeText);
            return reportName;
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
                        return true;
                    }
                }
            }
            return false;
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
            List<Contractor> contractors = new List<Contractor> {
                new Contractor(Guid.NewGuid().ToString(), "Alice", "Smith", null, 42.0),
                new Contractor(Guid.NewGuid().ToString(), "Bob", "Johnson", null, 38.5),
                new Contractor(Guid.NewGuid().ToString(), "Charlie", "Lee", null, 45.0),
                new Contractor(Guid.NewGuid().ToString(), "Diana", "Brown", null, 41.0),
                new Contractor(Guid.NewGuid().ToString(), "Edward", "Wilson", null, 39.0),
                new Contractor(Guid.NewGuid().ToString(), "Fiona", "Davis", null, 43.5),
                new Contractor(Guid.NewGuid().ToString(), "George", "Taylor", null, 40.0),
                new Contractor(Guid.NewGuid().ToString(), "Helen", "Martin", null, 44.0),
                new Contractor(Guid.NewGuid().ToString(), "Ian", "Walker", null, 37.5),
                new Contractor(Guid.NewGuid().ToString(), "Julia", "White", null, 46.0)
            };
            foreach (var item in contractors)
            {
                AddContractor(item);
            }
        }

        /// <summary>
        /// Adds 10 example jobs.
        /// </summary>
        public void AddJobxamples()
        {
            // Add 10 sample jobs
            List<Job> jobs = new List<Job> {
                new Job(Guid.NewGuid().ToString(), "Carpentry Work", DateTime.Now, 240.0, null, "Not Assigned", "-", null, null),
                new Job(Guid.NewGuid().ToString(), "Carpentry Work", DateTime.Now, 240.0, null, "Not Assigned", "-", null, null),
                new Job(Guid.NewGuid().ToString(), "Elec' Maintenance", DateTime.Now, 250.0, null, "Not Assigned", "-", null, null),
                new Job(Guid.NewGuid().ToString(), "HVAC Installation", DateTime.Now, 300.0, null, "Not Assigned", "-", null, null),
                new Job(Guid.NewGuid().ToString(), "Painting Project", DateTime.Now, 220.0, null, "Not Assigned", "-", null, null),
                new Job(Guid.NewGuid().ToString(), "Plumbing Repair", DateTime.Now, 180.0, null, "Not Assigned", "-", null, null),
                new Job(Guid.NewGuid().ToString(), "Roof Inspection", DateTime.Now, 195.0, null, "Not Assigned", "-", null, null),
                new Job(Guid.NewGuid().ToString(), "Win' Replacement", DateTime.Now, 275.0, null, "Not Assigned", "-", null, null),
                new Job(Guid.NewGuid().ToString(), "Flooring Upgrade", DateTime.Now, 210.0, null, "Not Assigned", "-", null, null),
                new Job(Guid.NewGuid().ToString(), "Gate Installation", DateTime.Now, 260.0, null, "Not Assigned", "-", null, null),
                new Job(Guid.NewGuid().ToString(), "Lighting Setup", DateTime.Now, 230.0, null, "Not Assigned", "-", null, null)};

            foreach (var item in jobs)
            {
                AddJob(item);
            }
        }
    }

}
