CREATE PROCEDURE [dbo].[spBudgetNames_GetBudgetNameById]

	@Id int

AS

	SELECT BudgetName
	FROM [dbo].[BudgetNames]
	WHERE Id = @Id;

RETURN 0
