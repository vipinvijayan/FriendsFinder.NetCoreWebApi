
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
    public class SecurityRepository : ConnectionBase, ISecurityRepository
    {

        public SecurityRepository(string connectionString) : base(connectionString)
        {

        }
        public SecurityRepository(IDbConnection connection) : base(connection)
        {

        }


        public async Task<string> SaveRefreshToken(RefreshTokenModel param)
        {

            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", param.UserId);
            dp.Add("@RefreshToken", param.RefreshToken);
            dp.Add("@Expires", param.Expires);
            dp.Add("@CreatedByIp", param.CreatedByIP);
            dp.Add("@Browser", param.Browser);





            using (var Connection = GetConnection())
            {
                string Result = await Connection.ExecuteScalarAsync<string>("SaveRefreshToken", dp, null, null, CommandType.StoredProcedure);
                return Result;
            }
        }

        public async Task<bool> GetActiveStatusOfToken(string token)
        {

            string query = "SELECT count(id) FROM RefreshTokenData where refreshToken='" + token + "' and IsActive=1 and IsDeleted=0;";


            using (var Connection = GetConnection())
            {
                var result = await Connection.ExecuteScalarAsync<int>(query);

                return result != 0;
            }


        }
        public async Task<RefreshTokenModel?> GetRefreshToken(string refreshToken)
        {

            string query = @"SELECT [RefreshToken],[Expires],[CreatedOn],[CreatedByIp],[IsActive],[UserId],[Browser] FROM [dbo].[RefreshTokenData] where RefreshToken=@RefreshToken;";


            using (var Connection = GetConnection())
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@RefreshToken", refreshToken);

                return await Connection.QueryFirstOrDefaultAsync<RefreshTokenModel>(query, dp);
            }
        }
        public async Task<string?> DeleteAllRefreshTokenOfUser(int userId)
        {

            DynamicParameters dp = new DynamicParameters();
            dp.Add("@UserId", userId);


            using (var Connection = GetConnection())
            {
                return await Connection.ExecuteScalarAsync<string>("DeleteAllRefreshTokenOfUser", dp, null, null, CommandType.StoredProcedure);

            }
        }
    }
}
