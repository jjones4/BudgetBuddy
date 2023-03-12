CREATE PROCEDURE [dbo].[spTemplateNames_GetTemplateNameById]
	
	@Id int

AS

	SELECT TemplateName
	FROM [dbo].[TemplateNames]
	WHERE Id = @Id;

RETURN 0
