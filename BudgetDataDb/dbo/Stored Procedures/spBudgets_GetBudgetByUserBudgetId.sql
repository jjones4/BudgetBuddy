CREATE PROCEDURE [dbo].[spBudgets_GetBudgetByUserBudgetId]

	@UserBudgetId int

AS

	SELECT b.[Id], b.[UserBudgetId], d.[Date], a.[Amount], ds.[Description], b.[Credit]
	FROM [dbo].[Budgets] b
	INNER JOIN [dbo].[Dates] d ON b.DateId = d.Id
	INNER JOIN [dbo].[Amounts] a ON b.AmountId = a.Id
	INNER JOIN [dbo].Descriptions ds ON b.DescriptionId = ds.Id
	WHERE UserBudgetId = @UserBudgetId;

RETURN 0
