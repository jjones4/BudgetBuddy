CREATE PROCEDURE [dbo].[spUsers_AddNewUserToBudgetDataDb]
	@UserName nvarchar(256),
	@AspNetUserId nvarchar(450)

AS
BEGIN

	SET NOCOUNT OFF;

	-- Insert statements for procedure here
	INSERT INTO [dbo].[Users] (UserName, AspNetUserId)
	VALUES (@UserName, @AspNetUserId);

RETURN 0
END