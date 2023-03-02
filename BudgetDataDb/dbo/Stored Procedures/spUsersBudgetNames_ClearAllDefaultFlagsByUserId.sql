CREATE PROCEDURE [dbo].[spUsersBudgetNames_ClearAllDefaultFlagsByUserId]

	@UserId int

AS
	UPDATE dbo.UsersBudgetNames
	SET IsDefaultBudget = 0
	WHERE UserId = @UserId;

RETURN 0
