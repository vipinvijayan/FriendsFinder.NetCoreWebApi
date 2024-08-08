using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppDataAccess.Models
{
    public class CompanyFriendModel
    {
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public int CreatedBy { get; set; }

    }
}
