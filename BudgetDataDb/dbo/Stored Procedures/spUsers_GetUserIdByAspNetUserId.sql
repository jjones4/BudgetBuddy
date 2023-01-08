CREATE PROCEDURE [dbo].[spUsers_GetUserIdByAspNetUserId]

	@AspNetUserId nvarchar(450)

AS

	SELECT Id, UserName, AspNetUserId
	FROM [dbo].[Users]
	WHERE AspNetUserId = @AspNetUserId;

RETURN 0
