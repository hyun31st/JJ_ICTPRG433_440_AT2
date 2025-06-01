using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace JJ_ICTPRG433_440_AT2.Helpers
{
    //Anne B, Joel M(2016): Murach's C# 2015 (page 210)
    public static class Validator
    {
        private static string title = "Entry Error";
        public static string Title
        {
            get { return title; }
            set { title = value; }
        }
        public static bool IsPresent(TextBox text, string tag)
        {
            if (text.Text.Trim() == "")
            {
                MessageBox.Show(tag + " is a required field.", Title);
                text.Clear();
                text.Focus();
                return false;
            }
            return true;
        }
        public static bool IsDouble(TextBox text, string tag)
        {
            double number = 0;
            if (double.TryParse(text.Text, out number))
            {
                return true;
            }
            else
            {
                MessageBox.Show(tag + " must be a number.", Title);
                text.Clear();
                text.Focus();
                return false;
            }
        }
        public static bool IsGreaterThanZero(TextBox text, string tag)
        {
            double number = 0;
            double.TryParse(text.Text, out number);
            if (number > 0)
            {
                return true;
            }
            else
            {
                MessageBox.Show(tag + " must be greater than 0.", Title);
                text.Clear();
                text.Focus();
                return false;
            }
        }
        public static bool ValidateGreaterValue(TextBox greaterText, string greaterTag, TextBox lesserText, string lesserTag)
        {
            double greaterNumber = 0;
            double lesserNumber = 0;
            double.TryParse(greaterText.Text, out greaterNumber);
            double.TryParse(lesserText.Text, out lesserNumber);
            if (greaterNumber > lesserNumber)
            {
                return true;
            }
            else
            {
                MessageBox.Show(greaterTag + " must be greater than " + lesserTag + ".", Title);
                greaterText.Clear();
                greaterText.Focus();
                return false;
            }
        }
        public static bool ValidateNoCommas(TextBox text, string tag)
        {
            if (text.Text.Contains(','))
            {
                MessageBox.Show(tag + " should not contain commas, as this may interfere with CSV report generation.\nPlease remove the comma and try again.", Title);
                text.Focus();
                return false;
            }
            return true;
        }
    }
}
