using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddyLibrary.Models
{
    public class StoredProcedureModel
    {
        public string NameOfStoredProcedure {get; set; } = string.Empty;

        public List<SqlParameter> SqlParameterList { get; set; } = new List<SqlParameter>();
    }
}
