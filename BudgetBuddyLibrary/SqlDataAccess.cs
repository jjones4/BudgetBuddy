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
            int numRowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedure.NameOfStoredProcedure, connection))
                    {
                        connection.Open();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(storedProcedure.SqlParameterList.ToArray());
                        numRowsAffected = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return numRowsAffected;
        }

        // Read
        public List<object[]> Read<T>(StoredProcedureModel storedProcedure, string connectionString)
        {
            List<object[]> output = new List<object[]>();

            Type t = typeof(T);
            int numFields = t.GetProperties().Count();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedure.NameOfStoredProcedure, connection))
                    {
                        object[] fields = new object[numFields];
                        connection.Open();

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(storedProcedure.SqlParameterList.ToArray());

                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                dr.GetValues(fields);

                                output.Add(fields);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return output;
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
