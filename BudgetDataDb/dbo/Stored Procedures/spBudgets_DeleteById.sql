CREATE PROCEDURE [dbo].[spBudgets_DeleteById]

	@Id int

AS
	
	DELETE FROM dbo.[Budgets]
	WHERE [UserBudgetId] = @Id;

	DELETE FROM [dbo].[UsersBudgetNames]
	WHERE [Id] = @Id;

RETURN 0
