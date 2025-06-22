using JJ_ICTPRG433_440_AT2.Helpers;
using JJ_ICTPRG433_440_AT2.Models;
using JJ_ICTPRG433_440_AT2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace JJ_ICTPRG433_440_AT2
{
    /// <summary>
    /// Interaction logic for AddJob.xaml
    /// </summary>
    public partial class AddJob : Window
    {
        public RecruitmentSystem recruitmentSystem;
        public AddJob(RecruitmentSystem recruitmentSystem)
        {
            this.recruitmentSystem = recruitmentSystem;
            InitializeComponent();
            TextBoxJobTitle.Focus();
        }

        private void ButtonAddJobSave_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidData())
            {
                Job newJob = new Job()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = TextBoxJobTitle.Text,
                    CreateDate = DateTime.Now,
                    Cost = Convert.ToDouble(TextBoxJobCost.Text),
                    CompleteDate = null,
                    ContractorAssigned = "Not Assigned",
                    Remark = "-",
                    ContractorId = null,
                    ContractorHours = null
                };
                recruitmentSystem.AddJob(newJob);
                

                DialogResult = true;
            }
            
        }

        private void ButtonAddJobCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private bool IsValidData()
        {
            return Validator.IsPresent(TextBoxJobTitle, "Job Title") &&
                Validator.ValidateNoCommas(TextBoxJobTitle, "Job Title") &&
                Validator.IsPresent(TextBoxJobCost, "Job Cost") &&
                Validator.IsDouble(TextBoxJobCost, "Job Cost") &&
                Validator.IsGreaterThanZero(TextBoxJobCost, "Job Cost");
        }
    }
}
