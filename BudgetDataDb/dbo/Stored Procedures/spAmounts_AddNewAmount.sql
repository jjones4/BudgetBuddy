CREATE PROCEDURE [dbo].[spAmounts_AddNewAmount]

	@Amount money

AS
	
	INSERT INTO dbo.[Amounts] ([Amount])
	VALUES (@Amount);

RETURN 0
