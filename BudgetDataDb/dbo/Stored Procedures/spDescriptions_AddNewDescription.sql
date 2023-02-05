CREATE PROCEDURE [dbo].[spDescriptions_AddNewDescription]

	@Description nvarchar(50)

AS
	
	INSERT INTO dbo.[Descriptions] ([Description])
	VALUES (@Description);

RETURN 0
