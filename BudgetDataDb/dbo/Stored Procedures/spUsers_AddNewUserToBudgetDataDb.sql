CREATE PROCEDURE [dbo].[spUsers_AddNewUserToBudgetDataDb]
	@UserName nvarchar(256),
	@AspNetUserId nvarchar(450)

AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	INSERT INTO [dbo].[Users] (UserName, AspNetUserId)
	VALUES (@UserName, @AspNetUserId);

RETURN 0
END