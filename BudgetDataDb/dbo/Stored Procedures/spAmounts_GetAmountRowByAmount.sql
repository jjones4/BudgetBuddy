CREATE PROCEDURE [dbo].[spAmounts_GetAmountRowByAmount]

	@Amount money

AS
	SELECT [Id], [Amount]
	FROM dbo.Amounts
	WHERE [Amount] = @Amount;

RETURN 0
