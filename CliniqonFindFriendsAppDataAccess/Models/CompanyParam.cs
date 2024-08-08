using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppDataAccess.Models
{
    public class CompanyParam
    {

        public string CompanyName { get; set; }
        public string? CompanyDescription { get; set; }
        public string? CompanyAddress { get; set; }
        public string? CompanyCity { get; set; }
        public string? CompanyState { get; set; }
        public string? CompanyCountry { get; set; }
        public string? CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }
        public int CreatedBy { get; set; }
        public string? Place { get; set; }
        public string? LandMark { get; set; }
    }
}
