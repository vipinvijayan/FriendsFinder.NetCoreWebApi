using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CliniqonFindFriendsAppDataAccess.Models
{
    public class UserRegistrationParam
    {
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Designation { get; set; }
        public string? Gender { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Country { get; set; }
        public string? FavoriteColor { get; set; }
        public string? FavoriteActor { get; set; }

    }
}
