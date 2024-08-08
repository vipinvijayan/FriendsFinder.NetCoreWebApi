using Dapper;
using CliniqonFindFriendsAppDataAccess.Entities;
using CliniqonFindFriendsAppDataAccess.Infrastructure;
using CliniqonFindFriendsAppDataAccess.IRepository;
using CliniqonFindFriendsAppDataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppDataAccess.Repository
{
    public class UserRepository : ConnectionBase, IUserRepository
    {
        public UserRepository(string connectionString) : base(connectionString)
        {

        }
        public UserRepository(IDbConnection connection) : base(connection)
        {

        }
        public async Task<string?> RegisterUser(UserRegistrationParam userRegistrationParam, string hashedPassword)
        {
            DateTimeOffset dtoF = new DateTimeOffset(userRegistrationParam.DateOfBirth);
            DynamicParameters dp = new DynamicParameters();
            dp.Add("@Name", userRegistrationParam.Name);
            dp.Add("@UserName", userRegistrationParam.Username);
            dp.Add("@Email", userRegistrationParam.Email);
            dp.Add("@Password", hashedPassword);
            dp.Add("@DateOfBirth", dtoF.ToUnixTimeSeconds());
            dp.Add("@Designation", userRegistrationParam.Designation);
            dp.Add("@Gender", userRegistrationParam.Gender);
            dp.Add("@ProfilePicture", userRegistrationParam.ProfilePicture);
            dp.Add("@Country", userRegistrationParam.Country);
            dp.Add("@FavoriteColor", userRegistrationParam.FavoriteColor);
            dp.Add("@FavoriteActor", userRegistrationParam.FavoriteActor);
            using (var Connection = GetConnection())
            {
                return await Connection.ExecuteScalarAsync<string>("SaveCompanyUserData", dp, null, null, CommandType.StoredProcedure);
            }
        }
        public async Task<UserProfileModel?> UserLogin(UserLoginParams userLoginParams, string hashedPassword)
        {

            string query = @"SELECT CompanyUserId FROM UserLogin WHERE UserLogin.UserName=@UserName AND UserLogin.Password=@Password AND UserLogin.IsActive=1 AND UserLogin.IsDeleted=0;";

            int result = 0;
            using (var Connection = GetConnection())
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@UserName", userLoginParams.Username);
                dp.Add("@Password", hashedPassword);
                result = await Connection.QueryFirstOrDefaultAsync<int>(query, dp);
            }
            if (result <= 0)
            {
                return null;
            }
            else
            {
                string profileQuery = @"SELECT Id,UserUniqueId,Name,Email,ProfilePicture,Country,FavoriteColor,FavoriteActor,CreatedOn as RegisteredOn
                FROM CompanyUsersData WHERE CompanyUsersData.Id=@UserId AND CompanyUsersData.IsActive=1 AND CompanyUsersData.IsDeleted=0;";
                using (var Connection = GetConnection())
                {
                    DynamicParameters dp = new DynamicParameters();
                    dp.Add("@UserId", result);

                    return await Connection.QueryFirstOrDefaultAsync<UserProfileModel>(profileQuery, dp);

                }
            }

        }
        public async Task<UserProfileModel?> GetUserDetails(int UserId)
        {

            string query = @"SELECT [Id],[UserUniqueId],[Name],[Email],[DateOfBirth],[Designation],[Gender],[ProfilePicture],[Country],[FavoriteColor]
            ,[FavoriteActor],[CreatedOn] as RegisteredOn,[UpdatedOn],[UpdatedBy],[IsActive],[IsDeleted] FROM [dbo].[CompanyUsersData] WHERE CompanyUsersData.Id=@UserId AND [CompanyUsersData].IsActive=1 AND [CompanyUsersData].IsDeleted=0;";


            using (var Connection = GetConnection())
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@UserId", UserId);

                return await Connection.QueryFirstOrDefaultAsync<UserProfileModel>(query, dp);
            }
        }

        public async Task<IQueryable<UserProfileListModel>?> GetAllOtherCompanyUsers(int UserId)
        {

            string query = @"SELECT [Id],[UserUniqueId],[Name],[Designation],[ProfilePicture] FROM [dbo].[CompanyUsersData] WHERE [CompanyUsersData].Id!=@UserId AND [CompanyUsersData].IsActive=1 AND [CompanyUsersData].IsDeleted=0;";


            using (var Connection = GetConnection())
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@UserId", UserId);

                var userProfiles = await Connection.QueryAsync<UserProfileListModel>(query, dp);
                return userProfiles.AsQueryable();
            }
        }
        public async Task<string?> SaveCompanyFriendsData(CompanyFriendModel companyFriendModelParam)
        {

            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", companyFriendModelParam.UserId);
            dp.Add("@FriendId", companyFriendModelParam.FriendId);
            dp.Add("@CreatedBy", companyFriendModelParam.CreatedBy);

            using (var Connection = GetConnection())
            {
                return await Connection.ExecuteScalarAsync<string>("SaveCompanyFriendsData", dp, null, null, CommandType.StoredProcedure);
            }
        }

        public async Task<IQueryable<UserProfileModel>?> GetAllCompanyFriends(SearchParam searchParam)
        {

            string query = @"SELECT dbo.CompanyUsersData.Id,dbo.CompanyUsersData.UserUniqueId, dbo.CompanyUsersData.Name, dbo.CompanyUsersData.Email, dbo.CompanyUsersData.DateOfBirth, dbo.CompanyUsersData.Designation,
            dbo.CompanyUsersData.Gender, dbo.CompanyUsersData.ProfilePicture, dbo.CompanyUsersData.Country, dbo.CompanyUsersData.FavoriteColor,dbo.CompanyUsersData.FavoriteActor, 
            dbo.CompanyUsersData.CreatedOn, dbo.CompanyUsersData.IsActive, dbo.CompanyFriends.UserId, dbo.CompanyFriends.FriendId, dbo.CompanyFriends.ProfileMatchPercentage,
            dbo.CompanyFriends.IsActive AS CompanyFriendIsActive,dbo.CompanyUsersData.IsDeleted,dbo.CompanyFriends.IsDeleted AS CompanyFriendIsDeleted 
            FROM   dbo.CompanyUsersData LEFT OUTER JOIN dbo.CompanyFriends ON dbo.CompanyUsersData.Id = dbo.CompanyFriends.FriendId 
            WHERE  (dbo.CompanyFriends.UserId =@UserId) AND (dbo.CompanyFriends.IsDeleted = 0) AND (dbo.CompanyUsersData.IsDeleted = 0) AND (dbo.CompanyFriends.IsActive = 1) AND (dbo.CompanyUsersData.IsActive = 1)";

            if (!string.IsNullOrWhiteSpace(searchParam.Keyword))
            {
                query += " AND ((dbo.CompanyUsersData.Name LIKE '%@Keyword%') OR (dbo.CompanyUsersData.FavoriteColor LIKE '%@Keyword%') OR (dbo.CompanyUsersData.FavoriteActor LIKE '%@Keyword%'))";
            }

            if (!string.IsNullOrWhiteSpace(searchParam.Gender))
            {
                query += " AND (dbo.CompanyUsersData.Gender = @Gender) ";
            }

            if ((searchParam.IsDateChecked))
            {

                query += " AND (dbo.CompanyUsersData.DateOfBirth between @FromDate and @ToDate)";
            }
            if (searchParam.TakeAll)
            {
                query += " ORDER BY dbo.CompanyUsersData.CreatedOn Desc";
            }
            else
            {
                searchParam.FixUp();
                query += " ORDER BY dbo.CompanyUsersData.CreatedOn Desc";
                query += " OFFSET " + searchParam.Skip + "ROWS";
                query += " FETCH NEXT " + searchParam.Take + " ROWS ONLY;";
            }
            using (var Connection = GetConnection())
            {
                DateTimeOffset dtoF = new DateTimeOffset(searchParam.FromDate);
                DateTimeOffset dtoT = new DateTimeOffset(searchParam.ToDate);
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@UserId", searchParam.RequestBy);
                dp.Add("@Gender", searchParam.Gender);
                dp.Add("@Keyword", searchParam.Keyword);
                dp.Add("@FromDate", dtoF.ToUnixTimeSeconds());
                dp.Add("@ToDate", dtoF.ToUnixTimeSeconds());
                var userProfiles = await Connection.QueryAsync<UserProfileModel>(query, dp);
                return userProfiles.AsQueryable();
            }
        }

        public async Task<IQueryable<UserProfileModel>?> GetAllCompanyFriendsByMatch(IdParam idParam)
        {

            string query = @"SELECT dbo.CompanyUsersData.Id,dbo.CompanyUsersData.UserUniqueId, dbo.CompanyUsersData.Name, dbo.CompanyUsersData.Email, dbo.CompanyUsersData.DateOfBirth, dbo.CompanyUsersData.Designation,
            dbo.CompanyUsersData.Gender, dbo.CompanyUsersData.ProfilePicture, dbo.CompanyUsersData.Country, dbo.CompanyUsersData.FavoriteColor,dbo.CompanyUsersData.FavoriteActor, 
            dbo.CompanyUsersData.CreatedOn, dbo.CompanyUsersData.IsActive, dbo.CompanyFriends.UserId, dbo.CompanyFriends.FriendId, dbo.CompanyFriends.ProfileMatchPercentage,
            dbo.CompanyFriends.IsActive AS CompanyFriendIsActive,dbo.CompanyUsersData.IsDeleted,dbo.CompanyFriends.IsDeleted AS CompanyFriendIsDeleted 
            FROM   dbo.CompanyUsersData LEFT OUTER JOIN dbo.CompanyFriends ON dbo.CompanyUsersData.Id = dbo.CompanyFriends.FriendId 
            WHERE (dbo.CompanyFriends.IsDeleted = 0) AND (dbo.CompanyUsersData.IsDeleted = 0) AND (dbo.CompanyFriends.IsActive = 1) AND (dbo.CompanyUsersData.IsActive = 1)";

            if (idParam.Id > 0)
            {
                query += " AND (dbo.CompanyFriends.UserId =@UserId)";
            }
            query += " Order By  dbo.CompanyFriends.ProfileMatchPercentage desc";

            using (var Connection = GetConnection())
            {

                DynamicParameters dp = new DynamicParameters();
                dp.Add("@UserId", idParam.Id);
                var userProfiles = await Connection.QueryAsync<UserProfileModel>(query, dp);
                return userProfiles.AsQueryable();
            }
        }


        public async Task<IQueryable<UserProfileListModel>?> GetProfilesByProfileMatchPercentage(IdParam idParam)
        {
            using (var Connection = GetConnection())
            {

                DynamicParameters dp = new DynamicParameters();
                dp.Add("@UserId", idParam.Id);
                var userProfiles = await Connection.QueryAsync<UserProfileListModel>("GetProfilesByProfileMatchPercentage", dp, null, null, CommandType.StoredProcedure);

                return userProfiles.AsQueryable();
            }
        }
    }
}
