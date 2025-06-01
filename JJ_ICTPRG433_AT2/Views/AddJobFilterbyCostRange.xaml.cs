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

namespace JJ_ICTPRG433_440_AT2.Views
{
    /// <summary>
    /// Interaction logic for AddJobFilterbyCostRange.xaml
    /// </summary>
    public partial class AddJobFilterbyCostRange : Window
    {
        public double MinCostRange { get; private set; }
        public double MaxCostRange { get; private set; }
        public AddJobFilterbyCostRange()
        {
            InitializeComponent();
        }

        private void ButtonAddCostRangeSave_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidData())
            {
                MinCostRange = Convert.ToDouble(TextBoxMinCostRange.Text);
                MaxCostRange = Convert.ToDouble(TextBoxMaxCostRange.Text);

                DialogResult = true;
            }
        }

        private void ButtonAddCostRangeCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private bool IsValidData()
        {
            return Validator.IsPresent(TextBoxMinCostRange, "Minimum Cost Range") &&
                Validator.IsDouble(TextBoxMinCostRange, "Minimum Cost Range") &&
                Validator.IsGreaterThanZero(TextBoxMinCostRange, "Minimum Cost Range") &&
                Validator.IsPresent(TextBoxMaxCostRange, "Maximum Cost Range") &&
                Validator.IsDouble(TextBoxMaxCostRange, "Maximum Cost Range") &&
                Validator.IsGreaterThanZero(TextBoxMaxCostRange, "Maximum Cost Range") &&
                Validator.ValidateGreaterValue(TextBoxMaxCostRange, "Maximum Cost Range", TextBoxMinCostRange, "Minimum Cost Range");
        }
    }
}
