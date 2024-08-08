using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppDataAccess.Models
{
    public class UserProfileListModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? UserUniqueId { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Designation { get; set; }
        public float? ProfileMatchPercentage { get; set; }
    }
}
