CREATE PROCEDURE [dbo].[spUsersBudgetNames_GetAllRowsForGivenUserId]

	@UserId int

AS
	SELECT Id, UserId, BudgetNameId, IsDefaultBudget, Threshhold
	FROM [dbo].UsersBudgetNames
	WHERE UserId = @UserId;

RETURN 0
