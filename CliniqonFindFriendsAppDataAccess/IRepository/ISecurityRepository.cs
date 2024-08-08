using CliniqonFindFriendsAppDataAccess.Entities;
using CliniqonFindFriendsAppDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppDataAccess.IRepository
{
    public interface ISecurityRepository
    {
        Task<string> SaveRefreshToken(RefreshTokenModel param);
        Task<bool> GetActiveStatusOfToken(string token);
        Task<string?> DeleteAllRefreshTokenOfUser(int userId);
        Task<RefreshTokenModel?> GetRefreshToken(string refreshToken);

    }
}
