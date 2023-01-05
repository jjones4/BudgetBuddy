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
        public void AddNewUserToBudgetDataDb(UserModel userToAdd, string connectionString)
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

            sqlDataAccess.Create(storedProcedure, connectionString);
        }
    }
}
