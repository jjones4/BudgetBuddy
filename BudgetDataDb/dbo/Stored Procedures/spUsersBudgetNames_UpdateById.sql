CREATE PROCEDURE [dbo].[spUsersBudgetNames_UpdateById]

	@Id int,
	@BudgetNameId int,
	@IsDefaultBudget bit,
	@Threshhold money = NULL

AS

	UPDATE [dbo].[UsersBudgetNames]
	SET BudgetNameId = @BudgetNameId, IsDefaultBudget = @IsDefaultBudget, Threshhold = @Threshhold
	WHERE [Id] = @Id;

RETURN 0
