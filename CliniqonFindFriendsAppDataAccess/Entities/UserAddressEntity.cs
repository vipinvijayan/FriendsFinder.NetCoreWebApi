using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppDataAccess.Entities
{
    public class UserAddressEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(50)]
        public string AddressType { get; set; } = "Home";
        [StringLength(10)]
        public string AddressFor { get; set; } = "Pickup";
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        [StringLength(10)]
        public string PostalCode { get; set; } = string.Empty;
        [StringLength(50)]
        public string City { get; set; } = string.Empty;
        [StringLength(50)]
        public string State { get; set; } = string.Empty;
        [StringLength(50)]
        public string Country { get; set; } = string.Empty;
        [StringLength(15)]
        public string ContactNo { get; set; } = string.Empty;
        [StringLength(100)]
        public string LandMark { get; set; } = string.Empty;

        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public int CreatedOn { get; set; } = (int)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds;
        public int UpdatedOn { get; set; } = (int)DateTime.UtcNow.Subtract(DateTime.UnixEpoch).TotalSeconds;
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
