using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppDataAccess.Models
{
    public class UserProfileModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? UserUniqueId { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Designation { get; set; }
        public string? Country { get; set; }
        public string? FavoriteColor { get; set; }
        public string? FavoriteActor { get; set; }
        public string? RegisteredOn { get; set; }
        public string? AccessToken { get; set; }
        public string ProfileMatchPercentage { get; set; }
        public long DateOfBirth { get; set; }



    }
}
