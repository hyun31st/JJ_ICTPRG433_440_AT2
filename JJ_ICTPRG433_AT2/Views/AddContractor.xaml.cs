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
    /// Interaction logic for AddConstractor.xaml
    /// </summary>

    public partial class AddConstractor : Window
    {
        RecruitmentSystem recruitmentSystem;
        public AddConstractor(RecruitmentSystem recruitmentSystem)
        {
            this.recruitmentSystem = recruitmentSystem;
            InitializeComponent();
            TextBoxContractorFirstName.Focus();
        }


        private void ButtonAddContractorCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonAddContractorSave_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidData())
            {
                Contractor newContractor = new Contractor()
                {
                    FirstName = TextBoxContractorFirstName.Text,
                    LastName = TextBoxContractorLastName.Text,
                    HourlyWage = Convert.ToDouble(TextBoxContractorHourlyWage.Text)
                };
                recruitmentSystem.AddContractor(newContractor);
                DialogResult = true;
            }

            
        }
        private bool IsValidData()
        {
            return Validator.IsPresent(TextBoxContractorFirstName, "First Name") &&
                Validator.IsPresent(TextBoxContractorLastName, "Last Name") &&
                Validator.IsPresent(TextBoxContractorHourlyWage, "Hourly Wage") &&
                Validator.IsDouble(TextBoxContractorHourlyWage, "Hourly Wage") &&
                Validator.IsGreaterThanZero(TextBoxContractorHourlyWage, "Hourly Wage");
        }
    }
}
