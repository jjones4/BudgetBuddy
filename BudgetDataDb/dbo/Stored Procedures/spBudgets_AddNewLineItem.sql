CREATE PROCEDURE [dbo].[spBudgets_AddNewLineItem]

	@UserBudgetId int,
	@DateId int,
	@AmountId int,
	@DescriptionId int,
	@IsCredit bit

AS
	INSERT INTO dbo.[Budgets] ([UserBudgetId], [DateId], [AmountId], [DescriptionId], [Credit])
	VALUES (@UserBudgetId, @DateId, @AmountId, @DescriptionId, @IsCredit);

RETURN 0
