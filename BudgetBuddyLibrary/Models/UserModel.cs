using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddyLibrary.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string AspNetUserId { get; set; } = string.Empty;
    }
}
