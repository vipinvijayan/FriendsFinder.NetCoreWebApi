using CliniqonFindFriendsAppBuisinessLogic.Interfaces;
using CliniqonFindFriendsAppDataAccess.Entities;
using CliniqonFindFriendsAppDataAccess.IRepository;
using CliniqonFindFriendsAppDataAccess.Models;
using CliniqonFindFriendsAppDataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliniqonFindFriendsAppBuisinessLogic.Implementation
{
    public class UserB : IUserB
    {
        readonly IUserRepository _userRepository;
        readonly IPasswordB _passwordB;
        public UserB(IUserRepository userRepository, IPasswordB passwordB)
        {
            _userRepository = userRepository;
            _passwordB = passwordB;
        }

        public async Task<string> RegisterUser(UserRegistrationParam userRegistrationParam, string salt)
        {
            string hashedPassword = _passwordB.EncryptPassword(userRegistrationParam.Password, salt);
            return await _userRepository.RegisterUser(userRegistrationParam, hashedPassword);
        }
        public async Task<UserProfileModel?> UserLogin(UserLoginParams userLoginParams, string salt)
        {
            string hashedPassword = _passwordB.EncryptPassword(userLoginParams.Password, salt);
            return await _userRepository.UserLogin(userLoginParams, hashedPassword);
        }



        public async Task<UserProfileModel?> GetUserDetails(int UserId)
        {
            return await _userRepository.GetUserDetails(UserId);
        }

        public async Task<IQueryable<UserProfileListModel>?> GetAllOtherCompanyUsers(int UserId)
        {
            return await _userRepository.GetAllOtherCompanyUsers(UserId);
        }
        public async Task<string?> SaveCompanyFriendsData(CompanyFriendModel companyFriendModelParam)
        {
            return await _userRepository.SaveCompanyFriendsData(companyFriendModelParam);
        }

        public async Task<IQueryable<UserProfileModel>?> GetAllCompanyFriends(SearchParam searchParam)
        {
            return await _userRepository.GetAllCompanyFriends(searchParam);
        }

        public async Task<IQueryable<UserProfileModel>?> GetAllCompanyFriendsByMatch(IdParam idParam)
        {
            return await _userRepository.GetAllCompanyFriendsByMatch(idParam);
        }

        public async Task<IQueryable<UserProfileListModel>?> GetProfilesByProfileMatchPercentage(IdParam idParam)
        {
            return await _userRepository.GetProfilesByProfileMatchPercentage(idParam);
        }

    }
}
