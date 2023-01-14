CREATE PROCEDURE [dbo].[spBudgetNames_GetBudgetNamesByLoggedInUserId]

	@UserId int

AS

	SELECT [bn].[Id], [bn].[BudgetName]
	FROM [dbo].[UsersBudgetNames] ubn
	INNER JOIN [dbo].[BudgetNames] bn ON ubn.BudgetNameId = bn.Id
	WHERE ubn.[UserId] = @UserId;

RETURN 0
