using BudgetBuddyLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddyLibrary
{
    public interface ISqlDataAccess
    {
        public Task<int> Create(StoredProcedureModel storedProcedure, string connectionString);

        public Task<List<object[]>> Read<T>(StoredProcedureModel storedProcedure, string connectionString);

        public Task<int> Update(StoredProcedureModel storedProcedure, string connectionString);

        public Task<int> Delete(StoredProcedureModel storedProcedure, string connectionString);
    }
}
