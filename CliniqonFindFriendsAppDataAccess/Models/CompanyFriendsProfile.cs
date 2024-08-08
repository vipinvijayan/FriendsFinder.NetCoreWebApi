using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppDataAccess.Models
{
    public class CompanyFriendsProfile
    {
        public string? UserUniqueId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public long DateOfBirth { get; set; }
        public string? Designation { get; set; }
        public string? Gender { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Country { get; set; }
        public string? FavoriteColor { get; set; }
        public string? FavoriteActor { get; set; }
        public string? CreatedOn { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }


    }
}
