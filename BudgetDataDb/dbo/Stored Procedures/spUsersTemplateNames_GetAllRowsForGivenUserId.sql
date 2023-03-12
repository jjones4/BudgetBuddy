CREATE PROCEDURE [dbo].[spUsersTemplateNames_GetAllRowsForGivenUserId]

	@UserId int

AS

	SELECT Id, UserId, TemplateNameId
	FROM [dbo].UsersTemplateNames
	WHERE UserId = @UserId;

RETURN 0
