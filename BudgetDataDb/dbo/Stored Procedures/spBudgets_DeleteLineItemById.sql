CREATE PROCEDURE [dbo].[spBudgets_DeleteLineItemById]

	@LineItemId int

AS

	DELETE FROM dbo.[Budgets]
	WHERE [Id] = @LineItemId;

RETURN 0
