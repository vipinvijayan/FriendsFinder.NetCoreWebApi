using CliniqonFindFriendsAppBuisinessLogic.Interfaces;
using CliniqonFindFriendsAppDataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppBuisinessLogic.Implementation
{
    public class PasswordB : IPasswordB
    {
        public PasswordB()
        {

        }

        public string EncryptPassword(string password, string salt)
        {


            // Hash the password with the salt
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password + "||" + salt);
                byte[] hash = sha256.ComputeHash(bytes);
                return (Convert.ToBase64String(hash));
            }
        }

    }
}
