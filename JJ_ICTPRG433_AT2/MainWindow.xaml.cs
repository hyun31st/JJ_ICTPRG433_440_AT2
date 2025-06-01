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
            AddConstractor addContractorWindow = new AddConstractor();

            if (addContractorWindow.ShowDialog() == true)
            {
                _recruitmentSystem.AddContractor(Guid.NewGuid().ToString(), 
                    addContractorWindow.ContractorFirstName, 
                    addContractorWindow.ContractorLastName,
                    null,
                    addContractorWindow.ContractorHourlyWage);
                RefreshContractors();
            }  
        }
        private void ButtonContractorRemove_Click(object sender, RoutedEventArgs e)
        {
            foreach (var selectedItem in ListViewContractors.SelectedItems)
            {
                Contractor contractor = (Contractor)selectedItem;
                _recruitmentSystem.RemoveContractor(contractor);
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
            AddJob addJobWindow = new AddJob();

            if (addJobWindow.ShowDialog() == true)
            {
                _recruitmentSystem.AddJob(Guid.NewGuid().ToString(), addJobWindow.JobTitle, DateTime.Now, addJobWindow.JobCost, null, "Not Assigned", "-", null, null);

                RefreshJobs();
            }
        }
        private void ButtonJobRemove_Click(object sender, RoutedEventArgs e)
        {
            foreach (var selectedItem in ListViewJobs.SelectedItems)
            {
                Job job = (Job)selectedItem;
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
            _recruitmentSystem.CompleteJob(job);

            RefreshJobs();
            RefreshContractors();
        }

        private void ButtonAssignContractorToAJob_Click(object sender, RoutedEventArgs e)
        {
            Contractor contractor = (Contractor)ListViewContractors.SelectedItem;
            Job job = (Job)ListViewJobs.SelectedItem;

            _recruitmentSystem.AssignContractorToAJob(contractor, job);

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
                    _recruitmentSystem.JobsFilterbyCostRange();
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
            _recruitmentSystem.JobCreateReport();
        }
    }
}