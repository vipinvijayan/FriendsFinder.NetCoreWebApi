using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppBuisinessLogic.Interfaces
{
    public interface IPasswordB
    {
        string EncryptPassword(string password, string salt);
    }
}
