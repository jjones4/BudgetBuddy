CREATE PROCEDURE [dbo].[spBudgets_GetLineItemById]

	@Id int

AS

	SELECT [Credit]
	FROM [dbo].[Budgets]
	WHERE Id = @Id;

RETURN 0
