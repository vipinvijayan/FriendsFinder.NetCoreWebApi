using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppDataAccess.Entities
{
    public class CompanyEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; }
        [MaxLength(300)]
        public string? CompanyDescription { get; set; }
        [MaxLength(300)]
        public string? CompanyAddress { get; set; }
        [MaxLength(100)]
        public string? Place { get; set; }
        [MaxLength(100)]
        public string? LandMark { get; set; }
        [MaxLength(100)]
        public string? CompanyCity { get; set; }
        [MaxLength(100)]
        public string? CompanyState { get; set; }
        [MaxLength(100)]
        public string? CompanyCountry { get; set; }
        [MaxLength(15)]
        public string? CompanyPhone { get; set; }
        [MaxLength(100)]
        public string? CompanyEmail { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int CreatedOn { get; set; } = (int)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds;
        public int UpdatedOn { get; set; } = (int)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds;
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
