﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddyLibrary.Models
{
    public class LineItemModel
    {
        public int Id { get; set; }

        public int UserBudgetId { get; set; }

        public DateTime DateOfTransaction { get; set; }

        public decimal AmountOfTransaction { get; set; }

        public string DescriptionOfTransaction { get; set; } = string.Empty;

        public bool IsCredit { get; set; } = false;
    }
}