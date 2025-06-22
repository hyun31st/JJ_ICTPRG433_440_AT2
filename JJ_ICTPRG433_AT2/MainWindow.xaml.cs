using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using JJ_ICTPRG433_440_AT2.Models;
using JJ_ICTPRG433_440_AT2.Services;
using JJ_ICTPRG433_440_AT2.Views;
using static System.Reflection.Metadata.BlobBuilder;
using System.IO;

namespace JJ_ICTPRG433_440_AT2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RecruitmentSystem _recruitmentSystem = new();

        public MainWindow()
        {
            InitializeComponent();
            lbl_MainWindowDate.Content = "Today: " + DateTime.Now.ToLongDateString();

            ClearComboBoxes();
            AddContractorFilter();
            AddJobFilter();

            _recruitmentSystem.AddContractorExamples();
            _recruitmentSystem.AddJobxamples();
            RefreshJobs();
            RefreshContractors();
        }
        private void ButtonContractorAdd_Click(object sender, RoutedEventArgs e)
        {
            AddConstractor addContractorWindow = new AddConstractor(_recruitmentSystem);

            if (addContractorWindow.ShowDialog() == true)
            {
                RefreshContractors();
            }  
        }
        private void ButtonContractorRemove_Click(object sender, RoutedEventArgs e)
        {
            foreach (var selectedItem in ListViewContractors.SelectedItems)
            {
                Contractor contractor = (Contractor)selectedItem;

                var (success, firstName, lastName, jobTitle) = _recruitmentSystem.RemoveContractor(contractor);

                if (!success)
                {
                    MessageBox.Show($"{firstName} {lastName} is currently assigned to \"{jobTitle}\"." +
                            $"\nPlease complete the job before removing the contractor.", "Cannot Remove Contractor", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            RefreshContractors();
        }
        private void RefreshContractors()
        {
            ListViewContractors.ItemsSource = null;
            ListViewContractors.ItemsSource = _recruitmentSystem.GetContractors();
        }

        private void ButtonJobAdd_Click(object sender, RoutedEventArgs e)
        {
            AddJob addJobWindow = new AddJob(_recruitmentSystem);

            if (addJobWindow.ShowDialog() == true)
            {
                RefreshJobs();
            }
        }
        private void ButtonJobRemove_Click(object sender, RoutedEventArgs e)
        {
            foreach (var selectedItem in ListViewJobs.SelectedItems)
            {
                Job job = (Job)selectedItem;
                if (job.ContractorAssigned.StartsWith("Yes"))
                {
                    Contractor contractor = (Contractor)_recruitmentSystem.GetContractorByID(job.ContractorId);
                    MessageBox.Show($"{contractor.FirstName} {contractor.LastName} is currently assigned to \"{job.Title}\"." +
                                $"\nThe contractor will be marked as available.", "Job Removal Notice", MessageBoxButton.OK, MessageBoxImage.Warning);
                    contractor.StartDate = null;
                }
                _recruitmentSystem.RemoveJob(job);
            }
            RefreshJobs();
            RefreshContractors();
        }
        private void RefreshJobs()
        {
            ListViewJobs.ItemsSource = null;
            ListViewJobs.ItemsSource = _recruitmentSystem.GetJobs();
        }

        private void ButtonJobComplete_Click(object sender, RoutedEventArgs e)
        {
            Job job = (Job)ListViewJobs.SelectedItem;

            // Check if the job is already completed
            if (job.ContractorAssigned.StartsWith("Completed"))
            {
                MessageBox.Show($"The job \"{job.Title}\" has already been completed. Please select a different job to proceed.",
                    "Job Already Completed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            // Check if the job has a contractor assigned
            else if (!job.ContractorAssigned.StartsWith("Not Assigned"))
            {
                Contractor assignedContractor = (Contractor)_recruitmentSystem.GetContractorByID(job.ContractorId);

                // Get user input how many hours contractor spent
                AddContractorHours addContractorHours = new AddContractorHours(job.Title, assignedContractor.FirstName, assignedContractor.LastName, assignedContractor.HourlyWage);
                if (addContractorHours.ShowDialog() == true)
                {
                    _recruitmentSystem.CompleteJob(job, assignedContractor, addContractorHours.HoursWorkedByContractor);
                }
            }
            // Job is not yet assigned
            else
            {
                MessageBox.Show($"The job \"{job.Title}\" does not have a contractor assigned. Please assign a contractor before marking the job as complete.",
                    "Contractor Not Assigned", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            RefreshJobs();
            RefreshContractors();
        }

        private void ButtonAssignContractorToAJob_Click(object sender, RoutedEventArgs e)
        {
            Contractor contractor = (Contractor)ListViewContractors.SelectedItem;
            Job job = (Job)ListViewJobs.SelectedItem;

            if (contractor == null)
            {
                MessageBox.Show("Please select a contractor from the list before proceeding.", "Contractor Not Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (job == null)
            {
                MessageBox.Show("Please select a job from the list before assigning a contractor.", "Job Not Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Verify that the selected job is not already assigned to a contractor
            if (job.ContractorAssigned.StartsWith("Not Assigned"))
            {
                // Check if the selected contractor is already assigned to another job
                if (!_recruitmentSystem.IsContractorAssignedToJob(contractor))
                {
                    _recruitmentSystem.AssignContractorToAJob(contractor, job);
                }
                else
                {
                    MessageBox.Show($"{contractor.FirstName} {contractor.LastName} is already assigned to another job. Please select a different contractor.",
                        "Contractor Already Assigned", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            else
            {
                MessageBox.Show($"{job.Title} is already in process or completed. Please select another job.",
                        "Job Already Assigned", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            RefreshContractors();
            RefreshJobs();
        }
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to close the application?\n\nWARNING: Any unsaved data will be lost.",
                "Confirm Exit", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Close();
            }
        }
        private void AddContractorFilter()
        {
            ComboBoxContractorFilter.Items.Add("Available Contractors Only");
            ComboBoxContractorFilter.Items.Add("Sort by First Name (A–Z)");
            ComboBoxContractorFilter.Items.Add("Sort by Last Name (A–Z)");
            ComboBoxContractorFilter.Items.Add("Sort by Hourly Wage (Low to High)");
        }
        private void ClearComboBoxes()
        {
            ComboBoxContractorFilter.Items.Clear();
            ComboBoxJobFilter.Items.Clear();
        }

        private void ComboBoxContractorFilter_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            switch(ComboBoxContractorFilter.SelectedIndex)
            {
                case 0:
                    _recruitmentSystem.ShowAvailableContractorsOnly();
                    break;
                case 1:
                    _recruitmentSystem.SortContractorsByFirstName();
                    break;
                case 2:
                    _recruitmentSystem.SortContractorsByLastName();
                    break;
                case 3:
                    _recruitmentSystem.SortContractorsByHourlyWage();
                    break;
            }
            RefreshContractors();
        }

        private void ButtonContractorShowAll_Click(object sender, RoutedEventArgs e)
        {
            _recruitmentSystem.ShowAllContractors();
            RefreshContractors();
            ComboBoxContractorFilter.SelectedItem = null;
        }
        private void AddJobFilter()
        {
            ComboBoxJobFilter.Items.Add("Show Available Jobs Only");
            ComboBoxJobFilter.Items.Add("Show In-Progress Jobs Only");
            ComboBoxJobFilter.Items.Add("Show Completed Jobs Only");
            ComboBoxJobFilter.Items.Add("Sort by Job Title (A–Z)");
            ComboBoxJobFilter.Items.Add("Sort by Cost (Low to High)");
            ComboBoxJobFilter.Items.Add("Filter by Cost Range");
        }
        private void ComboBoxJobFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBoxJobFilter.SelectedIndex)
            {
                case 0:
                    _recruitmentSystem.ShowAvailableJobsOnly();
                    break;
                case 1:
                    _recruitmentSystem.ShowInProgressJobsOnly();
                    break;
                case 2:
                    _recruitmentSystem.ShowCompletedJobsOnly();
                    break;
                case 3:
                    _recruitmentSystem.SortJobsByTitle();
                    break;
                case 4:
                    _recruitmentSystem.SortJobsByCost();
                    break;
                case 5:
                    AddJobFilterbyCostRange addJobFilterbyCostRangeWindow = new AddJobFilterbyCostRange();
                    if (addJobFilterbyCostRangeWindow.ShowDialog() == true)
                    {
                        _recruitmentSystem.JobsFilterbyCostRange(addJobFilterbyCostRangeWindow.MinCostRange, addJobFilterbyCostRangeWindow.MaxCostRange);
                    }
                    break;
            }
            RefreshJobs();
        }

        private void ButtonJobShowAll_Click(object sender, RoutedEventArgs e)
        {
            _recruitmentSystem.ShowAllJobs();
            RefreshJobs();
            ComboBoxJobFilter.SelectedItem = null;
        }

        private void ButtonJobCreateReport_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to create the report in CSV format?\nOnly visible items in the ListView will be exported.", "Export to CSV", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                string reportName = _recruitmentSystem.JobCreateReport();

                MessageBox.Show($"{reportName} has been successfully created.\nFile location:\n{System.IO.Path.GetFullPath(reportName)}.", "Report Created");
            }
        }
    }
}