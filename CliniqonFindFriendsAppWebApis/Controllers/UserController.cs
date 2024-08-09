using CliniqonFindFriendsAppBuisinessLogic.Interfaces;
using CliniqonFindFriendsAppBussinessLogic.Interfaces;
using CliniqonFindFriendsAppDataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySqlX.XDevAPI.Common;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace DrCleanerAppWebApis.Controllers
{

    [ApiController]

    public class UserController : ControllerBase
    {

        private readonly IUserB _userB;
        private readonly IConfiguration _configuration;
        private readonly ISecurityB _securityB;

        public UserController(IUserB userB, IConfiguration configuration, ISecurityB securityB)
        {
            _userB = userB;

            _configuration = configuration;
            _securityB = securityB;

        }

        #region API Call

        /// <summary>
        /// This api used to login using their credentials
        /// </summary>
        /// <param name="UserLoginParams"></param>
        /// <returns> UserProfileModel or "Failed Message"</returns>
        [HttpPost, Route("user/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginParams userLginParam)
        {
            try
            {
                if (userLginParam != null)
                {
                    string userAgent = Request.Headers["User-Agent"].ToString();
                    //Calling login method
                    UserProfileModel? userProfileModel = await loginCall(userLginParam.Username, userLginParam.Password);
                    if (userProfileModel != null)
                    {


                        string refreshToken = await _securityB.SaveRefreshToken(userProfileModel, ipAddress(), userAgent);
                        setTokenCookie(refreshToken);
                        userProfileModel.AccessToken = await _securityB.GenerateJWTToken(userProfileModel.UserUniqueId, userProfileModel.Id, userAgent);
                        ResponseObj updatecartObj = new ResponseObj();
                        updatecartObj.Result = GeneralDTO.SuccessMessage;
                        updatecartObj.ResponseData = userProfileModel;

                        return Ok(updatecartObj);


                    }
                    else
                    {

                        var responseObj = new ResponseObj();
                        responseObj.Result = GeneralDTO.FailedMessage;
                        responseObj.ResponseData = "Login Failed";
                        return Ok(responseObj);
                    }

                }
                else
                {
                    throw new ArgumentException($"Unknown parameter: {userLginParam}");

                }
            }
            catch (ValidationException ex)
            {
                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ex.Message;
                return Ok(responseObj);


            }
            catch (ArgumentException ar)
            {
                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ar.Message;
                return Ok(responseObj);
            }
            catch (Exception ex)
            {

                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ex.Message;
                return Ok(responseObj);
            }
        }


        /// <summary>
        /// Saving New user to the system
        /// </summary>
        /// <param name="UserRegistrationParam"></param>
        /// <returns></returns>
        [HttpPost, Route("user/registration")]
        [AllowAnonymous]
        public async Task<IActionResult> Registration(UserRegistrationParam? userRegistrationParam)
        {
            try
            {
                string userAgent = Request.Headers["User-Agent"].ToString();
                var saltKey = _configuration.GetSection("AppSettings").GetValue<string>("SaltKey");
                var responseObj = new ResponseObj();

                if (userRegistrationParam != null)
                {

                    string result = await _userB.RegisterUser(userRegistrationParam, saltKey);
                    if (result == GeneralDTO.SuccessMessage)
                    {
                        //Calling login method
                        UserProfileModel? userProfileModel = await loginCall(userRegistrationParam.Username, userRegistrationParam.Password);
                        if (userProfileModel != null)
                        {
                            //if login success creating jwttoken
                            string refreshToken = await _securityB.SaveRefreshToken(userProfileModel, ipAddress(), userAgent);
                            //setting jwt token to cookie
                            setTokenCookie(refreshToken);
                            userProfileModel.AccessToken = await _securityB.GenerateJWTToken(userProfileModel.UserUniqueId, userProfileModel.Id, userAgent);
                            responseObj.Result = GeneralDTO.SuccessMessage;
                            responseObj.ResponseData = userProfileModel;

                        }
                        else
                        {

                            responseObj.Result = GeneralDTO.FailedMessage;
                            responseObj.ResponseData = "Registration Failed";

                        }

                    }
                    else if (result == GeneralDTO.AlreadyMessage)
                    {

                        responseObj.Result = GeneralDTO.FailedMessage;
                        responseObj.ResponseData = "User Already Registered";

                    }
                }
                else
                {

                    throw new ArgumentException($"Unknown parameter: {userRegistrationParam}");

                }
                return Ok(responseObj);
            }
            catch (ValidationException ex)
            {
                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ex.Message;
                return Ok(responseObj);
            }
            catch (ArgumentException ar)
            {
                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ar.Message;
                return Ok(responseObj);
            }
            catch (Exception ex)
            {
                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ex.Message;
                return Ok(responseObj);
            }
        }

        /// <summary>
        /// Deleting all user tokens from Database
        /// </summary>
        /// <param name="IdParam">User Id</param>
        /// <returns>Success / Failed</returns>
        [HttpPost, Route("user/revokeuser")]
        [Authorize]
        public async Task<IActionResult> RevokeUserToken(IdParam idParam)
        {
            try
            {
                if (idParam != null)
                {

                    string result = await _securityB.DeleteAllRefreshTokenOfUser(idParam.Id);
                    ResponseObj responseObj = new ResponseObj();
                    responseObj.Result = GeneralDTO.SuccessMessage;
                    responseObj.ResponseData = result;
                    return Ok(responseObj);
                }
                else
                {

                    throw new ArgumentException($"Unknown parameter: {idParam}");
                }
            }
            catch (ValidationException ex)
            {
                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ex.Message;
                return Ok(responseObj);


            }
            catch (ArgumentException ar)
            {
                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ar.Message;
                return Ok(responseObj);
            }
            catch (Exception ex)
            {

                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ex.Message;
                return Ok(responseObj);
            }
        }

        /// <summary>
        /// Getting the new Access token if the refresh token has validity and access token validity expired
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("user/refreshtoken")]
        public async Task<IActionResult> RefreshToken()
        {

            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null)
                return BadRequest("Invalid refresh token");

            var dbRefreshToken = await _securityB.GetRefreshToken(refreshToken);
            UserProfileModel? userData = await _userB.GetUserDetails(dbRefreshToken.UserId);

            if (!dbRefreshToken.IsActive)
            {
                //Remove this tokens for the user
                // await _userB.RevokeToken(refreshToken);
                return Unauthorized("token is revoked by admin. Pls login again");
            }

            if (userData == null)
            {
                //Remove all tokens for the user as user is deactivated
                await _securityB.DeleteAllRefreshTokenOfUser(dbRefreshToken.UserId);
                return Unauthorized("Account is Locked");
            }



            if (dbRefreshToken.RefreshToken != refreshToken
                || dbRefreshToken.IsExpired)
            {
                return Unauthorized("Invalid refresh token or token expired");
            }
            string userAgent = Request.Headers["User-Agent"].ToString();
            string newRefreshToken = await _securityB.SaveRefreshToken(userData, ipAddress(), userAgent);
            setTokenCookie(newRefreshToken);
            string accessToken = await _securityB.GenerateJWTToken(userData.UserUniqueId, userData.Id, dbRefreshToken.Browser);
            ResponseObj responseObj = new ResponseObj();
            responseObj.Result = GeneralDTO.SuccessMessage;
            responseObj.ResponseData = accessToken;
            return Ok(responseObj);
        }

        /// <summary>
        /// Getting the User Details by User Id
        /// </summary>
        /// <param name="idParam"></param>
        /// <returns></returns>
        [HttpPost, Route("user/getuserdetails")]
        [Authorize]
        public async Task<IActionResult> GetUserDetails(IdParam idParam)
        {
            try
            {
                if (idParam != null)
                {

                    var result = await _userB.GetUserDetails(idParam.Id);
                    ResponseObj responseObj = new ResponseObj();
                    responseObj.Result = GeneralDTO.SuccessMessage;
                    responseObj.ResponseData = result;
                    return Ok(responseObj);
                }
                else
                {

                    throw new ArgumentException($"Unknown parameter: {idParam}");
                }
            }
            catch (ValidationException ex)
            {
                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ex.Message;
                return Ok(responseObj);


            }
            catch (ArgumentException ar)
            {
                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ar.Message;
                return Ok(responseObj);
            }
            catch (Exception ex)
            {

                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ex.Message;
                return Ok(responseObj);
            }
        }

        /// <summary>
        /// Get the users on the company except the current user
        /// </summary>
        /// <param name="idParam"></param>
        /// <returns></returns>
        [HttpPost, Route("user/getallothercompanyusers")]
        [Authorize]
        public async Task<IActionResult> GetAllOtherCompanyUsers(IdParam idParam)
        {
            try
            {
                if (idParam != null)
                {

                    var result = await _userB.GetAllOtherCompanyUsers(idParam.Id);
                    ResponseObj responseObj = new ResponseObj();
                    responseObj.Result = GeneralDTO.SuccessMessage;
                    responseObj.ResponseData = result;
                    return Ok(responseObj);
                }
                else
                {

                    throw new ArgumentException($"Unknown parameter: {idParam}");
                }
            }
            catch (ValidationException ex)
            {
                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ex.Message;
                return Ok(responseObj);


            }
            catch (ArgumentException ar)
            {
                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ar.Message;
                return Ok(responseObj);
            }
            catch (Exception ex)
            {

                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ex.Message;
                return Ok(responseObj);
            }
        }

        /// <summary>
        /// Saving the user to friends list
        /// </summary>
        /// <param name="companyFriendModelParam"></param>
        /// <returns></returns>
        [HttpPost, Route("user/savecompanyfriendsdata")]
        [Authorize]
        public async Task<IActionResult> SaveCompanyFriendsData(CompanyFriendModel companyFriendModelParam)
        {
            try
            {
                if (companyFriendModelParam != null)
                {

                    var result = await _userB.SaveCompanyFriendsData(companyFriendModelParam);
                    ResponseObj responseObj = new ResponseObj();
                    responseObj.Result = GeneralDTO.SuccessMessage;
                    responseObj.ResponseData = result;
                    return Ok(responseObj);
                }
                else
                {

                    throw new ArgumentException($"Unknown parameter: {companyFriendModelParam}");
                }
            }
            catch (ValidationException ex)
            {
                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ex.Message;
                return Ok(responseObj);


            }
            catch (ArgumentException ar)
            {
                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ar.Message;
                return Ok(responseObj);
            }
            catch (Exception ex)
            {

                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ex.Message;
                return Ok(responseObj);
            }
        }

        /// <summary>
        /// Get all the friends connected
        /// </summary>
        /// <param name="searchParam"></param>
        /// <returns></returns>
        [HttpPost, Route("user/getallcompanyfriends")]
        [Authorize]
        public async Task<IActionResult> GetAllCompanyFriends(SearchParam searchParam)
        {
            try
            {
                if (searchParam != null)
                {

                    var result = await _userB.GetAllCompanyFriends(searchParam);
                    ResponseObj responseObj = new ResponseObj();
                    responseObj.Result = GeneralDTO.SuccessMessage;
                    responseObj.ResponseData = result;
                    return Ok(responseObj);
                }
                else
                {

                    throw new ArgumentException($"Unknown parameter: {searchParam}");
                }
            }
            catch (ValidationException ex)
            {
                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ex.Message;
                return Ok(responseObj);


            }
            catch (ArgumentException ar)
            {
                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ar.Message;
                return Ok(responseObj);
            }
            catch (Exception ex)
            {

                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ex.Message;
                return Ok(responseObj);
            }
        }

        /// <summary>
        /// Get all the profile matched friends
        /// </summary>
        /// <param name="idParam"></param>
        /// <returns></returns>
        [HttpPost, Route("user/getallcompanyfriendsbymatch")]
        [Authorize]
        public async Task<IActionResult> GetAllCompanyFriendsByMatch(IdParam idParam)
        {
            try
            {
                if (idParam != null)
                {

                    var result = await _userB.GetAllCompanyFriendsByMatch(idParam);
                    ResponseObj responseObj = new ResponseObj();
                    responseObj.Result = GeneralDTO.SuccessMessage;
                    responseObj.ResponseData = result;
                    return Ok(responseObj);
                }
                else
                {

                    throw new ArgumentException($"Unknown parameter: {idParam}");
                }
            }
            catch (ValidationException ex)
            {
                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ex.Message;
                return Ok(responseObj);


            }
            catch (ArgumentException ar)
            {
                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ar.Message;
                return Ok(responseObj);
            }
            catch (Exception ex)
            {

                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ex.Message;
                return Ok(responseObj);
            }
        }

        /// <summary>
        /// Get all the users which profile matched greater than 50%
        /// </summary>
        /// <param name="idParam"></param>
        /// <returns></returns>
        [HttpPost, Route("user/getprofilesbymatchpercentage")]
        [Authorize]
        public async Task<IActionResult> GetProfilesByProfileMatchPercentage(IdParam idParam)
        {
            try
            {
                if (idParam != null)
                {

                    var result = await _userB.GetProfilesByProfileMatchPercentage(idParam);
                    ResponseObj responseObj = new ResponseObj();
                    responseObj.Result = GeneralDTO.SuccessMessage;
                    responseObj.ResponseData = result;
                    return Ok(responseObj);
                }
                else
                {

                    throw new ArgumentException($"Unknown parameter: {idParam}");
                }
            }
            catch (ValidationException ex)
            {
                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ex.Message;
                return Ok(responseObj);


            }
            catch (ArgumentException ar)
            {
                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ar.Message;
                return Ok(responseObj);
            }
            catch (Exception ex)
            {

                var responseObj = new ResponseObj();
                responseObj.Result = GeneralDTO.FailedMessage;
                responseObj.ResponseData = ex.Message;
                return Ok(responseObj);
            }
        }

        /// <summary>
        /// File Upload for profile picture
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        [HttpPost, Route("user/uploadfile")]
        [Authorize]
        public async Task<IActionResult> UploadFile([FromBody] UploadImageParam parms)
        {
            try
            {
                string ImageName = "";
                ResponseObj responseobj = new ResponseObj();
                responseobj.Result = GeneralDTO.SuccessMessage;
                if (parms.ImageStr != null)
                {
                    string Base64String = parms.ImageStr.Split(',')[1];
                    // byte[] imageBytes = Convert.FromBase64String(parms.ImageStr);
                    //ImageName = configuration.GetSection("AppSettings").GetValue<string>("ImageNamePrefix");

                    ImageName = _configuration.GetSection("AppSettings").GetValue<string>("ImageNamePrefix") + GetRandomNumbers(8);
                    string uploadUrl = _configuration.GetSection("AppSettings").GetValue<string>("FileUploadUrl");

                    string imageUrl = await SaveFile(Base64String, ImageName, parms.Extension, parms.ImageType);
                    responseobj.ResponseData = uploadUrl + "/" + parms.ImageType + "/" + imageUrl;
                }

                return Ok(responseobj);
            }
            catch (Exception ex)
            {
                ResponseObj responseobj = new ResponseObj();
                responseobj.Result = GeneralDTO.SuccessMessage;
                responseobj.ResponseData = ex.Message + "_" + ex.InnerException != null ? ex.InnerException.Message : "";
                return Ok(responseobj);
            }
        }


        #endregion


        #region Private Methods

        /// <summary>
        /// Getting the Ip Address from the request
        /// </summary>
        /// <returns></returns>
        private string getIpAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        /// <summary>
        /// Setting refresh token to into Cookie
        /// </summary>
        /// <param name="refreshToken"></param>
        private void setTokenCookie(string refreshToken)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                SameSite = SameSiteMode.Lax,
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Delete("refreshToken");

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        /// <summary>
        /// Login using the credentials ,User agent for saving login log to check the browser type
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="userAgent"></param>
        /// <returns>UserProfileModel</returns>
        /// 
        private async Task<UserProfileModel?> loginCall(string username, string password)
        {
            var saltKey = _configuration.GetSection("AppSettings").GetValue<string>("SaltKey");
            //if user registration is success the try login with the credentials
            UserLoginParams loginParam = new UserLoginParams();
            loginParam.Username = username;
            loginParam.Password = password;


            return await _userB.UserLogin(loginParam, saltKey);

        }
        /// <summary>
        /// Getting the ip of the api call from the header
        /// </summary>
        /// <returns></returns>
        private string ipAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        /// <summary>
        /// Getting Random Numbers by provided length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private string GetRandomNumbers(int length)
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sOTP = String.Empty;
            int iOTPLength = length;

            string sTempChars = String.Empty;

            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)

            {
                int p = rand.Next(0, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                sOTP += sTempChars;
            }

            return sOTP;
        }

        /// <summary>
        /// Saving File to Folder
        /// </summary>
        /// <param name="ImgStr"></param>
        /// <param name="ImgName"></param>
        /// <param name="Extention"></param>
        /// <param name="ImageType"></param>
        /// <returns></returns>
        private async Task<string> SaveFile(string ImgStr, string ImgName, string Extention, string ImageType)
        {
            string path = _configuration.GetSection("AppSettings").GetValue<string>("FileUploadLocation");
            path = path + "/" + ImageType + "/";
            //HttpContext.Current.Server.MapPath("~/ImageStorage"); //Path

            //Check if directory exist
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
            }

            //string imageName = ImgName + ".jpg";

            //set the image path
            string ImageName = ImgName + Extention;
            string imgPath = Path.Combine(path, ImageName);
            if (System.IO.File.Exists(imgPath))
            {
                System.IO.File.Delete(imgPath); //Delete Image already excist
                ImageName = ImgName + "_" + DateTime.Now.Date.ToString("ddMMyyyy") + Extention;
                imgPath = Path.Combine(path, ImageName);
            }
            byte[] imageBytes = Convert.FromBase64String(ImgStr);

            await System.IO.File.WriteAllBytesAsync(imgPath, imageBytes);

            return ImageName;
        }
        #endregion
    }
}
