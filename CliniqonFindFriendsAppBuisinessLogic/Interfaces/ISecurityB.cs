using CliniqonFindFriendsAppDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppBussinessLogic.Interfaces
{
    public interface ISecurityB
    {
        Task<string> GenerateJWTToken(string userName, int userId, string userAgent);
        Task<string> SaveRefreshToken(UserProfileModel param, string ipAddress,string userAgent);
        Task<bool> GetActiveStatusOfToken(string token);
        Task<string> DeleteAllRefreshTokenOfUser(int userId);
        Task<RefreshTokenModel> GetRefreshToken(string refreshToken);

    }
}
