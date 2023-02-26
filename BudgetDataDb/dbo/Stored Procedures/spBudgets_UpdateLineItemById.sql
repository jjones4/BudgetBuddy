CREATE PROCEDURE [dbo].[spBudgets_UpdateLineItemById]

	@lineItemId int,
	@DateId int,
	@AmountId int,
	@DescriptionId int,
	@IsCredit bit

AS
	
	UPDATE dbo.[Budgets]
	SET DateId = @DateId, AmountId = @AmountId, [DescriptionId] = @DescriptionId, Credit = @IsCredit
	WHERE [Id] = @lineItemId;

RETURN 0
