CREATE PROCEDURE [dbo].[spDates_GetDateRowByDate]

	@Date datetime2

AS
	SELECT [Id], [Date]
	FROM dbo.Dates
	WHERE [Date] = @Date;

RETURN 0
