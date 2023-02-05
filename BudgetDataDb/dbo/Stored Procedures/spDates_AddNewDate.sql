CREATE PROCEDURE [dbo].[spDates_AddNewDate]

	@Date datetime2

AS

	INSERT INTO dbo.[Dates] ([Date])
	VALUES (@Date);

RETURN 0
