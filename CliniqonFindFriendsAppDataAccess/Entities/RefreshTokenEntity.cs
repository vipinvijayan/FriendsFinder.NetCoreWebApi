using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppDataAccess.Entities
{
    public class RefreshTokenEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [MaxLength(300)]
        public string RefreshToken { get; set; }
        public long Expires { get; set; }
        public long CreatedOn { get; set; }
        [MaxLength(30)]
        public string? CreatedByIp { get; set; }
        [MaxLength(300)]
        public string? ReplaceByToken { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }


    }
}
