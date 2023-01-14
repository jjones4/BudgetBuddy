CREATE PROCEDURE [dbo].[spUsersBudgetNames_GetUserBudgetNameIdByLoggedInUserIdAndBudgetNamesId]

	@UserId int,
	@BudgetNameId int

AS
	SELECT [Id]
	FROM [dbo].[UsersBudgetNames]
	WHERE UserId = @UserId
	AND BudgetNameId = @BudgetNameId;

RETURN 0
