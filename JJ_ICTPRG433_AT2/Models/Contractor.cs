using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ_ICTPRG433_440_AT2.Models
{
    public class Contractor
    {
        public string Id {  get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public DateTime? StartDate { get; set; }
        public double? HourlyWage { get; set; }

        public Contractor() 
        {
            Id = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            StartDate = null;
            HourlyWage = null;
        }

        public Contractor(string id, string firstName, string lastName, DateTime? startDate, double? hourlyWage)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            StartDate = startDate;
            HourlyWage = hourlyWage;
        }
        public override string ToString()
        {
            return $"{FirstName,20}\t{LastName,-20}\t{StartDate?.ToShortDateString() ?? "Not Assigned",25}\t$ {HourlyWage,5}";
            // Stackoverflow(https://stackoverflow.com/questions/644017/net-format-a-string-with-fixed-spaces)
        }
    }
}
