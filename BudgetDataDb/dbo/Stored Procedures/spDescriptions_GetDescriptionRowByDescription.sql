CREATE PROCEDURE [dbo].[spDescriptions_GetDescriptionRowByDescription]

	@Description nvarchar(50)

AS

	SELECT [Id], [Description]
	FROM dbo.Descriptions
	WHERE [Description] = @Description;

RETURN 0
