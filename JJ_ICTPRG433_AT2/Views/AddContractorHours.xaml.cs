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
using JJ_ICTPRG433_440_AT2.Helpers;
using JJ_ICTPRG433_440_AT2.Services;

namespace JJ_ICTPRG433_440_AT2.Views
{
    /// <summary>
    /// Interaction logic for AddContractorHours.xaml
    /// </summary>
    public partial class AddContractorHours : Window
    {
        public double HoursWorkedByContractor { get; private set; }
        public AddContractorHours(string jobTitle, string contractorFirstName, string contractorLastName, double? contractorHourlyWage)
        {
            InitializeComponent();
            LabelJobTitle.Content = jobTitle;
            LabelContractorName.Content = contractorFirstName + " " + contractorLastName;
            LabelContractorHourlyWage.Content = "$ " + contractorHourlyWage;
            TextBoxHoursWorkedByContractor.Focus();
        }

        private void ButtonAddhourSave_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidData())
            {
                HoursWorkedByContractor = Convert.ToDouble(TextBoxHoursWorkedByContractor.Text);

                DialogResult = true;
            }
        }
        private bool IsValidData()
        {
            return Validator.IsPresent(TextBoxHoursWorkedByContractor, "Hours Worked By Contractor") &&
                Validator.IsDouble(TextBoxHoursWorkedByContractor, "Hours Worked By Contractor") &&
                Validator.IsGreaterThanZero(TextBoxHoursWorkedByContractor, "Hours Worked By Contractor");
        }

        private void ButtonAddhourCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
