CREATE PROCEDURE [dbo].[spUsersBudgetNames_AddNewBudget]
	@UserId int,
	@BudgetNameId int,
	@IsDefaultBudget bit,
	@Threshhold money = NULL

AS

	INSERT INTO dbo.[UsersBudgetNames]
	(UserId, BudgetNameId, IsDefaultBudget, Threshhold)
	VALUES (@UserId, @BudgetNameId, @IsDefaultBudget, @Threshhold);

RETURN 0
