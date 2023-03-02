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
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

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

        public async Task<int> AddNewBudgetNameToBudgetNamesTable(string budgetName, string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spBudgetNames_AddNewBudgetName",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@BudgetName", SqlDbType.NVarChar)
                    {
                        Value = budgetName
                    }
                }
            };

            int numRowsAffected = await sqlDataAccess.Create(storedProcedure, connectionString);

            return numRowsAffected;
        }

        public async Task<int> AddNewLineItemToBudgetsTable(int budgetId, 
            int dateId,
            int amountId, 
            int descriptionId, 
            bool isCredit, 
            string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spBudgets_AddNewLineItem",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@UserBudgetId", SqlDbType.Int)
                    {
                        Value = budgetId
                    },
                    new SqlParameter("@DateId", SqlDbType.Int)
                    {
                        Value = dateId
                    },
                    new SqlParameter("@AmountId", SqlDbType.Int)
                    {
                        Value = amountId
                    },
                    new SqlParameter("@DescriptionId", SqlDbType.Int)
                    {
                        Value = descriptionId
                    },
                    new SqlParameter("@IsCredit", SqlDbType.Bit)
                    {
                        Value = isCredit
                    }
                }
            };

            int numRowsAffected = await sqlDataAccess.Create(storedProcedure, connectionString);

            return numRowsAffected;
        }

        public async Task<int> AddNewDateToDatesTable(DateTime date, string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spDates_AddNewDate",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@Date", SqlDbType.DateTime2)
                    {
                        Value = date
                    }
                }
            };

            int numRowsAffected = await sqlDataAccess.Create(storedProcedure, connectionString);

            return numRowsAffected;
        }

        public async Task<int> AddNewAmountToAmountsTable(decimal amount, string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spAmounts_AddNewAmount",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@Amount", SqlDbType.Money)
                    {
                        Value = amount
                    }
                }
            };

            int numRowsAffected = await sqlDataAccess.Create(storedProcedure, connectionString);

            return numRowsAffected;
        }

        public async Task<int> AddNewDescriptionToDescriptionsTable(string description, string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spDescriptions_AddNewDescription",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@Description", SqlDbType.NVarChar)
                    {
                        Value = description
                    }
                }
            };

            int numRowsAffected = await sqlDataAccess.Create(storedProcedure, connectionString);

            return numRowsAffected;
        }

        public async Task<int> AddNewBudgetToUsersBudgetNamesTable(int userId,
            int budgetNameId,
            bool isDefaultBudget,
            decimal? threshhold,
            string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spUsersBudgetNames_AddNewBudget",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@UserId", SqlDbType.Int)
                    {
                        Value = userId
                    },
                    new SqlParameter("@BudgetNameId", SqlDbType.Int)
                    {
                        Value = budgetNameId
                    },
                    new SqlParameter("@IsDefaultBudget", SqlDbType.Bit)
                    {
                        Value = isDefaultBudget
                    },
                    new SqlParameter("@Threshhold", SqlDbType.Money)
                    {
                        Value = threshhold
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

        public async Task<List<BudgetNameModel>> GetIdByBudgetName(string budgetName, string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spBudgetNames_GetIdByBudgetName",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@BudgetName", SqlDbType.NVarChar)
                    {
                        Value = budgetName
                    }
                }
            };

            List<object[]> rawSqlRows = await sqlDataAccess.Read<UserModel>(storedProcedure, connectionString);
            List<BudgetNameModel> users = new List<BudgetNameModel>();

            // There should only be one budget id that is returned
            // so this foreach loop may not be necessary
            foreach (object[] row in rawSqlRows)
            {
                users.Add(new BudgetNameModel()
                {
                    Id = (int)row[0],
                    BudgetName = (string)row[1]
                });
            }

            // There should only be one user that is returned
            // since AspNetUserId should be unique
            return users;
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

        public async Task<List<DateModel>> GetDateRowsByDate(DateTime date, string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spDates_GetDateRowByDate",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@Date", SqlDbType.DateTime2)
                    {
                        Value = date
                    }
                }
            };

            List<object[]> rawSqlRows = await sqlDataAccess.Read<DateModel>(storedProcedure, connectionString);
            List<DateModel> dates = new List<DateModel>();

            foreach (object[] row in rawSqlRows)
            {
                dates.Add(new DateModel()
                {
                    Id = (int)row[0],
                    DateOfTransaction = (DateTime)row[1]
                });
            }

            return dates;
        }

        public async Task<List<AmountModel>> GetAmountRowsByAmount(decimal amount, string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spAmounts_GetAmountRowByAmount",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@Amount", SqlDbType.Money)
                    {
                        Value = amount
                    }
                }
            };

            List<object[]> rawSqlRows = await sqlDataAccess.Read<AmountModel>(storedProcedure, connectionString);
            List<AmountModel> amounts = new List<AmountModel>();

            foreach (object[] row in rawSqlRows)
            {
                amounts.Add(new AmountModel()
                {
                    Id = (int)row[0],
                    AmountOfTransaction = (Decimal)row[1]
                });
            }

            return amounts;
        }

        public async Task<List<DescriptionModel>> GetDescriptionRowsByDescription(string description, string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spDescriptions_GetDescriptionRowByDescription",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@Description", SqlDbType.NVarChar)
                    {
                        Value = description
                    }
                }
            };

            List<object[]> rawSqlRows = await sqlDataAccess.Read<DescriptionModel>(storedProcedure, connectionString);
            List<DescriptionModel> descriptions = new List<DescriptionModel>();

            foreach (object[] row in rawSqlRows)
            {
                descriptions.Add(new DescriptionModel()
                {
                    Id = (int)row[0],
                    DescriptionOfTransaction = (string)row[1]
                });
            }

            return descriptions;
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

        public async Task<List<BudgetNameModel>> GetBudgetNameByDefaultBudgetId(int defaultUserBudgetId, string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spUsersBudgetNames_GetBudgetNameByDefaultBudgetId",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@Id", SqlDbType.Int)
                    {
                        Value = defaultUserBudgetId
                    }
                }
            };

            List<object[]> rawSqlRows = await sqlDataAccess.Read<BudgetNameModel>(storedProcedure, connectionString);
            List<BudgetNameModel> budgetNames = new List<BudgetNameModel>();

            foreach (object[] row in rawSqlRows)
            {
                budgetNames.Add(new BudgetNameModel()
                {
                    Id = (int)row[0],
                    BudgetName = (string)row[1]
                });
            }

            return budgetNames;
        }

        public async Task<List<BudgetNameModel>> GetBudgetNamesByLoggedInUserId(int loggedInUserId, string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spBudgetNames_GetBudgetNamesByLoggedInUserId",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@UserId", SqlDbType.Int)
                    {
                        Value = loggedInUserId
                    }
                }
            };

            List<object[]> rawSqlRows = await sqlDataAccess.Read<BudgetNameModel>(storedProcedure, connectionString);
            List<BudgetNameModel> budgetNames = new List<BudgetNameModel>();

            foreach (object[] row in rawSqlRows)
            {
                budgetNames.Add(new BudgetNameModel()
                {
                    Id = (int)row[0],
                    BudgetName = (string)row[1]
                });
            }

            return budgetNames;
        }

        public async Task<List<BudgetNameModel>> GetAllBudgetNames(string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spBudgetNames_GetBudgetNamesAll",
            };

            List<object[]> rawSqlRows = await sqlDataAccess.Read<BudgetNameModel>(storedProcedure, connectionString);
            List<BudgetNameModel> budgetNames = new List<BudgetNameModel>();

            foreach (object[] row in rawSqlRows)
            {
                budgetNames.Add(new BudgetNameModel()
                {
                    Id = (int)row[0],
                    BudgetName = (string)row[1]
                });
            }

            return budgetNames;
        }

        public async Task<int> GetUserBudgetNameIdByLoggedInUserIdAndBudgetNameId(int loggedInUserId, int budgetNameId, string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spUsersBudgetNames_GetUserBudgetNameIdByLoggedInUserIdAndBudgetNamesId",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@UserId", SqlDbType.Int)
                    {
                        Value = loggedInUserId
                    },
                     new SqlParameter("@BudgetNameId", SqlDbType.Int)
                    {
                        Value = budgetNameId
                    }
                }
            };

            List<object[]> rawSqlRows = await sqlDataAccess.Read<UsersBudgetNamesModel>(storedProcedure, connectionString);
            List<UsersBudgetNamesModel> usersBudgetNames = new List<UsersBudgetNamesModel>();

            // There should only be one user that is returned
            // so this foreach loop may not be necessary
            foreach (object[] row in rawSqlRows)
            {
                usersBudgetNames.Add(new UsersBudgetNamesModel()
                {
                    Id = (int)row[0]
                });
            }

            // There should only be one user that is returned
            // since AspNetUserId should be unique
            return usersBudgetNames.First().Id;
        }

        public async Task<string> GetBudgetNameById(int budgetNameId, string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spBudgetNames_GetBudgetNameById",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@Id", SqlDbType.Int)
                    {
                        Value = budgetNameId
                    }
                }
            };

            List<object[]> rawSqlRows = await sqlDataAccess.Read<BudgetNameModel>(storedProcedure, connectionString);
            List<BudgetNameModel> budgetNames = new List<BudgetNameModel>();

            foreach (object[] row in rawSqlRows)
            {
                budgetNames.Add(new BudgetNameModel()
                {
                    BudgetName = (string)row[0]
                });
            }

            return budgetNames.First().BudgetName;
        }

        public async Task<int> UpdateLineItemInBudgetsTable(int lineItemId,
            int dateId,
            int amountId,
            int descriptionId,
            bool isCredit,
            string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spBudgets_UpdateLineItemById",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@LineItemId", SqlDbType.Int)
                    {
                        Value = lineItemId
                    },
                    new SqlParameter("@DateId", SqlDbType.Int)
                    {
                        Value = dateId
                    },
                    new SqlParameter("@AmountId", SqlDbType.Int)
                    {
                        Value = amountId
                    },
                    new SqlParameter("@DescriptionId", SqlDbType.Int)
                    {
                        Value = descriptionId
                    },
                    new SqlParameter("@IsCredit", SqlDbType.Bit)
                    {
                        Value = isCredit
                    }
                }
            };

            int numRowsAffected = await sqlDataAccess.Update(storedProcedure, connectionString);

            return numRowsAffected;
        }

        public async Task<int> ClearDefaultBudgetFlagsByUserId(int userId,
            string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spUsersBudgetNames_ClearAllDefaultFlagsByUserId",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@UserId", SqlDbType.Int)
                    {
                        Value = userId
                    }
                }
            };

            int numRowsAffected = await sqlDataAccess.Update(storedProcedure, connectionString);

            return numRowsAffected;
        }

        public async Task<int> DeleteLineItemById(int lineItemId,
            string connectionString)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            StoredProcedureModel storedProcedure = new StoredProcedureModel()
            {
                NameOfStoredProcedure = "dbo.spBudgets_DeleteLineItemById",

                SqlParameterList = new List<SqlParameter>()
                {
                    new SqlParameter("@LineItemId", SqlDbType.Int)
                    {
                        Value = lineItemId
                    }
                }
            };

            int numRowsAffected = await sqlDataAccess.Delete(storedProcedure, connectionString);

            return numRowsAffected;
        }
    }
}
