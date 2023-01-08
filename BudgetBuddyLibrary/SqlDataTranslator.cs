using BudgetBuddyLibrary.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddyLibrary
{
    public class SqlDataTranslator : ISqlDataTranslator
    {
        public int AddNewUserToBudgetDataDb(UserModel userToAdd, string connectionString)
        {
            SqlDataAccess sqlDataAccess= new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spUsers_AddNewUserToBudgetDataDb",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@UserName", SqlDbType.NVarChar)
                    {
                        Value = userToAdd.UserName
                    },
                    new SqlParameter("@AspNetUserId", SqlDbType.NVarChar)
                    {
                        Value = userToAdd.AspNetUserId
                    }
                }
            };

            int numRowsAffected = sqlDataAccess.Create(storedProcedure, connectionString);

            return numRowsAffected;
        }

        public int GetUserIdByAspNetUserId(UserModel user, string connectionString)
        {
            int id = 0;

            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spUsers_GetUserIdByAspNetUserId",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@AspNetUserId", SqlDbType.NVarChar)
                    {
                        Value = user.AspNetUserId
                    }
                }
            };

            List<object[]> rawSqlRows = sqlDataAccess.Read<UserModel>(storedProcedure, connectionString);
            List<UserModel> users = new List<UserModel>();

            // There should only be one user that is returned
            // so this foreach loop may not be necessary
            foreach (object[] row in rawSqlRows)
            {
                users.Add(new UserModel()
                {
                    Id = (int)row[0],
                    UserName = (string)row[1],
                    AspNetUserId = (string)row[2]
                });
            }

            // There should only be one user that is returned
            // since AspNetUserId should be unique
            return users.First().Id;
        }
    }
}
