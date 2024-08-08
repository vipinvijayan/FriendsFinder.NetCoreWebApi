using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppDataAccess.Models
{
    public class RefreshTokenModel
    {
        public int UserId { get; set; }
        [MaxLength(300)]
        public string RefreshToken { get; set; }
        public long Expires { get; set; }
        public long CreatedOn { get; set; }
        [MaxLength(30)]
        public string? CreatedByIP { get; set; }
        [MaxLength(300)]
        public bool IsActive { get; set; }

        public string? Browser { get; set; }
        public string OperatingSystem { get; set; }
        /// <summary>
        /// Is Token Expired its validity
        /// </summary>
        public bool IsExpired => DateTimeOffset.UtcNow.ToUnixTimeSeconds() >= Expires;

    }
}
