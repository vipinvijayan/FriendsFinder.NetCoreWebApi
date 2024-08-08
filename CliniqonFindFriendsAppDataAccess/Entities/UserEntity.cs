using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppDataAccess.Entities
{
    public class UserEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        [StringLength(15)]
        public string MobileNo { get; set; } = string.Empty;
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int CreatedOn { get; set; } = (int)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds;
        public int UpdatedOn { get; set; } = (int)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds;
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
        [Required]
        [StringLength(100)]
        public string Username { get; set; }
        [Required]
        [StringLength(15)]
        public string Password { get; set; }
        public int CompanyId { get; set; }
        public CompanyEntity Company { get; set; }
        public List<UserAddressEntity> UserAddress { get; set; } = new List<UserAddressEntity>();
    }
}
