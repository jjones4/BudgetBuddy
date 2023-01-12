using BudgetBuddyLibrary.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddyLibrary
{
    public class SqlDataTranslator : ISqlDataTranslator
    {
        public async Task<int> AddNewUserToBudgetDataDb(UserModel userToAdd, string connectionString)
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

            int numRowsAffected = await sqlDataAccess.Create(storedProcedure, connectionString);

            return numRowsAffected;
        }

        public async Task<int> GetUserIdByAspNetUserId(string aspNetUserId, string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spUsers_GetUserIdByAspNetUserId",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@AspNetUserId", SqlDbType.NVarChar)
                    {
                        Value = aspNetUserId
                    }
                }
            };

            List<object[]> rawSqlRows = await sqlDataAccess.Read<UserModel>(storedProcedure, connectionString);
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

        public async Task<List<UsersBudgetNamesModel>> GetUsersBudgetNamesRowsByUserId(int userId, string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spUsersBudgetNames_GetAllRowsForGivenUserId",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@UserId", SqlDbType.Int)
                    {
                        Value = userId
                    }
                }
            };

            List<object[]> rawSqlRows = await sqlDataAccess.Read<UsersBudgetNamesModel>(storedProcedure, connectionString);
            List<UsersBudgetNamesModel> usersBudgetNames = new List<UsersBudgetNamesModel>();

            foreach (object[] row in rawSqlRows)
            {
                if (row[4] is DBNull)
                {
                    usersBudgetNames.Add(new UsersBudgetNamesModel()
                    {
                        Id = (int)row[0],
                        UserId = (int)row[1],
                        BudgetNameId = (int)row[2],
                        IsDefaultBudget = (bool)row[3],
                        Threshhold = null
                    });
                }
                else
                {
                    usersBudgetNames.Add(new UsersBudgetNamesModel()
                    {
                        Id = (int)row[0],
                        UserId = (int)row[1],
                        BudgetNameId = (int)row[2],
                        IsDefaultBudget = (bool)row[3],
                        Threshhold = (decimal)row[4]
                    });
                }
            }

            return usersBudgetNames;
        }

        public async Task<List<LineItemModel>> GetLineItemsByUserBudgetId(int userBudgetId, string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spBudgets_GetBudgetByUserBudgetId",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@UserBudgetId", SqlDbType.Int)
                    {
                        Value = userBudgetId
                    }
                }
            };

            List<object[]> rawSqlRows = await sqlDataAccess.Read<LineItemModel>(storedProcedure, connectionString);
            List<LineItemModel> lineItems = new List<LineItemModel>();

            foreach (object[] row in rawSqlRows)
            {
                lineItems.Add(new LineItemModel()
                {
                    Id = (int)row[0],
                    UserBudgetId = (int)row[1],
                    DateOfTransaction = (DateTime)row[2],
                    AmountOfTransaction = (decimal)row[3],
                    DescriptionOfTransaction = (string)row[4],
                    IsCredit = (bool)row[5]
                });
            }

            return lineItems;
        }
    }
}
