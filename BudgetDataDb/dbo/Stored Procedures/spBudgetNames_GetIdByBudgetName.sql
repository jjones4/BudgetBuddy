CREATE PROCEDURE [dbo].[spBudgetNames_GetIdByBudgetName]

	@BudgetName nvarchar(50)

AS

	SELECT [Id]
	FROM dbo.BudgetNames
	WHERE BudgetName = @BudgetName;

RETURN 0
