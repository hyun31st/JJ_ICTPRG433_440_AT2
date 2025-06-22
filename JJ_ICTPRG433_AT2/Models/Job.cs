using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ_ICTPRG433_440_AT2.Models
{
    public class Job
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime? CreateDate { get; set; }
        public double Cost { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string ContractorAssigned { get; set; }
        public string Remark { get; set; }
        public string? ContractorId { get; set; }
        public double? ContractorHours { get; set; }

        public Job()
        {
            Id = string.Empty;
            Title = string.Empty;
            CreateDate = null;
            Cost = 0;
            CompleteDate = null;
            ContractorAssigned = string.Empty;
            Remark = string.Empty;
            ContractorId = string.Empty;
            ContractorHours = null;
        }

        public Job(string id, string title, DateTime? createDate, double cost, DateTime? completeDate, 
            string contractorAssigned, string remark, string? contractorID, double? contractorHours)
        {
            Id = id;
            Title = title;
            CreateDate = createDate;
            Cost = cost;
            CompleteDate = completeDate;
            ContractorAssigned = contractorAssigned;
            Remark = remark;
            ContractorId = contractorID;
            ContractorHours = contractorHours;
        }
        public override string ToString()
        {
            string contractorAssignmentStatus = string.Empty;
            if (ContractorAssigned.StartsWith("Not Assigned"))
            {
                contractorAssignmentStatus = "Not Assigned";
            }
            else if (ContractorAssigned.StartsWith("Completed"))
            {
                contractorAssignmentStatus = "Completed";
            }
            else
            {
                contractorAssignmentStatus = "Yes";
            }
            string contractorWorkedHours = string.Empty;
            if (ContractorHours == null)
            {
                contractorWorkedHours = " ";
            }
            else
            {
                contractorWorkedHours = ContractorHours + " hrs";
            }
            return $"{Title,-30}\t{CreateDate?.ToShortDateString() ?? "Not Assigned"}\t$ {Cost,6}\t" +
                $"{CompleteDate?.ToShortDateString() ?? "Not Started",15}\t{contractorAssignmentStatus,20}" + 
                $"{Remark,40}" +
                $"{contractorWorkedHours}";
        //Stackoverflow(https://stackoverflow.com/questions/644017/net-format-a-string-with-fixed-spaces)
        }
    }
}
