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
        public int Create(StoredProcedureModel storedProcedure, string connectionString);

        public List<List<object>> Read(StoredProcedureModel storedProcedure, string connectionString);

        public int Update(StoredProcedureModel storedProcedure, string connectionString);

        public int Delete(StoredProcedureModel storedProcedure, string connectionString);
    }
}
