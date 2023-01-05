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
    public class SqlDataAccess : ISqlDataAccess
    {
        // Create - returns the number of records affected
        public int Create(StoredProcedureModel storedProcedure, string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedure.NameOfStoredProcedure, connection))
                    {
                        connection.Open();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(storedProcedure.SqlParameterList.ToArray());
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return 0;
        }

        // Read
        public List<List<object>> Read(StoredProcedureModel storedProcedure, string connectionString)
        {
            return new List<List<object>>();
        }

        // Update - returns the number of records affected
        public int Update(StoredProcedureModel storedProcedure, string connectionString)
        {
            return 0;
        }

        // Delete - returns the number of records affected
        public int Delete(StoredProcedureModel storedProcedure, string connectionString)
        {
            return 0;
        }
    }
}
