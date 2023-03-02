CREATE PROCEDURE [dbo].[spBudgetNames_AddNewBudgetName]

	@BudgetName nvarchar(50)

AS

	INSERT INTO dbo.BudgetNames (BudgetName)
	VALUES (@BudgetName);

RETURN 0
