CREATE PROCEDURE [dbo].[spBudgetNames_GetBudgetNamesAll]
	
AS
	SELECT [Id], [BudgetName]
	FROM [dbo].[BudgetNames];

RETURN 0
