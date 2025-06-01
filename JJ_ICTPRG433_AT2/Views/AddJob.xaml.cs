using JJ_ICTPRG433_440_AT2.Helpers;
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
        public string JobTitle { get; private set; }
        public double JobCost { get; private set; }
        public AddJob()
        {
            InitializeComponent();
            TextBoxJobTitle.Focus();
        }

        private void ButtonAddJobSave_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidData())
            {
                JobTitle = TextBoxJobTitle.Text;
                JobCost = Convert.ToDouble(TextBoxJobCost.Text);

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
