using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppDataAccess.Entities
{
    public class LoginLogEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string? UserName { get; set; }
        public int CreatedOn { get; set; } = (int)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds;
        [StringLength(1000)]
        public string? UserAgent { get; set; }
        public UserEntity? UserData { get; set; }
        public CompanyEntity? Company { get; set; }
        public string? LoginStatus { get; set; }
    }
}
