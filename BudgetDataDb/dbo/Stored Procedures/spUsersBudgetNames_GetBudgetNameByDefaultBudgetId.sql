CREATE PROCEDURE [dbo].[spUsersBudgetNames_GetBudgetNameByDefaultBudgetId]
	
	@Id int

AS
	
	SELECT [bn].[Id], [bn].[BudgetName]
	FROM [dbo].[UsersBudgetNames] ubn
	INNER JOIN [dbo].[BudgetNames] bn ON ubn.BudgetNameId = bn.Id
	WHERE ubn.[Id] = @id;

RETURN 0
