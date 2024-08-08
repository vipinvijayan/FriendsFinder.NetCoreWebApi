using CliniqonFindFriendsAppDataAccess.Entities;
using CliniqonFindFriendsAppDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppBuisinessLogic.Interfaces
{
    public interface IUserB
    {
        Task<UserProfileModel?> UserLogin(UserLoginParams param, string salt);
        Task<string> RegisterUser(UserRegistrationParam param, string salt);

        Task<UserProfileModel?> GetUserDetails(int UserId);

        Task<IQueryable<UserProfileListModel>?> GetAllOtherCompanyUsers(int UserId);
        Task<string?> SaveCompanyFriendsData(CompanyFriendModel companyFriendModelParam);
        Task<IQueryable<UserProfileModel>?> GetAllCompanyFriends(SearchParam searchParam);
        Task<IQueryable<UserProfileModel>?> GetAllCompanyFriendsByMatch(IdParam idParam);

        Task<IQueryable<UserProfileListModel>?> GetProfilesByProfileMatchPercentage(IdParam idParam);
    }
}
