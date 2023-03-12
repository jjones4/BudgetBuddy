CREATE PROCEDURE [dbo].[spTemplates_DeleteById]

	@Id int

AS

	DELETE FROM dbo.[Templates]
	WHERE [UserTemplateId] = @Id;

	DELETE FROM [dbo].[UsersTemplateNames]
	WHERE [Id] = @Id;

RETURN 0
