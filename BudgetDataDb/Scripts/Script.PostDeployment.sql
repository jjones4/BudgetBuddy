/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DROP TABLE IF EXISTS [dbo].[Templates];
	DROP TABLE IF EXISTS [dbo].[Budgets];
	DROP TABLE IF EXISTS [dbo].[Dates];
	DROP TABLE IF EXISTS [dbo].[Amounts];
	DROP TABLE IF EXISTS [dbo].[Descriptions];
	DROP TABLE IF EXISTS [dbo].[UsersBudgetNames];
	DROP TABLE IF EXISTS [dbo].[BudgetNames];
	DROP TABLE IF EXISTS [dbo].[UsersTemplateNames];
	DROP TABLE IF EXISTS [dbo].[TemplateNames];
	DROP TABLE IF EXISTS [dbo].[Users];

	CREATE TABLE [dbo].[Users]
	(
		Id INT PRIMARY KEY IDENTITY NOT NULL,
		UserName NVARCHAR(256) NOT NULL UNIQUE,
		AspNetUserId NVARCHAR(450) NOT NULL UNIQUE
	);

	-- Insert statements for procedure here
	INSERT INTO
		[dbo].[Users]
		(UserName, AspNetUserId)
	VALUES
		('abrown@abrown.abrown', '1ef9ae1a-6b60-4ccc-b97c-e8d868d0d249'),
		('bclayton@bclayton.bclayton', 'aaf71569-bfbc-4cb5-b6c1-1e29be7535f5'),
		('cduvall@cduvall.cduvall', '2b529782-fae5-4500-a454-89736d97db26');

	DECLARE @UserId1 int;
	DECLARE @UserId2 int;
	DECLARE @UserId3 int;

	SELECT @UserId1 = Id FROM dbo.Users WHERE UserName = 'abrown@abrown.abrown';
	SELECT @UserId2 = Id FROM dbo.Users WHERE UserName = 'bclayton@bclayton.bclayton';
	SELECT @UserId3 = Id FROM dbo.Users WHERE UserName = 'cduvall@cduvall.cduvall';

	CREATE TABLE [dbo].[BudgetNames]
	(
		Id INT PRIMARY KEY IDENTITY NOT NULL,
		BudgetName NVARCHAR(255) NOT NULL UNIQUE
	);

	INSERT INTO
		[dbo].[BudgetNames]
		(BudgetName)
	VALUES
		('Business'),
		('Family'),
		('Personal');

	CREATE TABLE [dbo].[UsersBudgetNames]
	(
		Id INT PRIMARY KEY IDENTITY NOT NULL,
		UserId int NOT NULL,
		BudgetNameId int NOT NULL,
		CONSTRAINT FK_User_UserBudgetNameUserId FOREIGN KEY (UserId)
		REFERENCES Users (Id)
		ON DELETE CASCADE,
		CONSTRAINT FK_BudgetName_UserBudgetNameBudgetNameId FOREIGN KEY (BudgetNameId)
		REFERENCES BudgetNames (Id)
		ON DELETE CASCADE,
		IsDefaultBudget bit,
		Threshhold money
	);

	DECLARE @BudgetNameId1 int;
	DECLARE @BudgetNameId2 int;
	DECLARE @BudgetNameId3 int;

	SELECT @BudgetNameId1 = Id FROM dbo.BudgetNames WHERE BudgetName = 'Business';
	SELECT @BudgetNameId2 = Id FROM dbo.BudgetNames WHERE BudgetName = 'Family';
	SELECT @BudgetNameId3 = Id FROM dbo.BudgetNames WHERE BudgetName = 'Personal';

	INSERT INTO
		[dbo].[UsersBudgetNames]
		(UserId, BudgetNameId)
	VALUES
		(@UserId1, @BudgetNameId1),
		(@UserId1, @BudgetNameId2),
		(@UserId2, @BudgetNameId1),
		(@UserId2, @BudgetNameId3),
		(@UserId3, @BudgetNameId2);

	CREATE TABLE [dbo].[TemplateNames]
	(
		Id INT PRIMARY KEY IDENTITY NOT NULL,
		TemplateName NVARCHAR(255) NOT NULL UNIQUE
	);

	INSERT INTO
		[dbo].[TemplateNames]
		(TemplateName)
	VALUES
		('Dining Out'),
		('Groceries'),
		('Rent');

	CREATE TABLE [dbo].[UsersTemplateNames]
	(
		Id INT PRIMARY KEY IDENTITY NOT NULL,
		UserId int NOT NULL,
		TemplateNameId int NOT NULL,
		CONSTRAINT FK_User_UserTemplateNameUserId FOREIGN KEY (UserId)
		REFERENCES Users (Id)
		ON DELETE CASCADE,
		CONSTRAINT FK_TemplateName_UserTemplateNameTemplateNameId FOREIGN KEY (TemplateNameId)
		REFERENCES TemplateNames (Id)
		ON DELETE CASCADE
	);

	DECLARE @TemplateNameId1 int;
	DECLARE @TemplateNameId2 int;
	DECLARE @TemplateNameId3 int;

	SELECT @TemplateNameId1 = Id FROM dbo.TemplateNames WHERE TemplateName = 'Dining Out';
	SELECT @TemplateNameId2 = Id FROM dbo.TemplateNames WHERE TemplateName = 'Groceries';
	SELECT @TemplateNameId3 = Id FROM dbo.TemplateNames WHERE TemplateName = 'Rent';

	INSERT INTO
		[dbo].[UsersTemplateNames]
		(UserId, TemplateNameId)
	VALUES
		(@UserId1, @TemplateNameId1),
		(@UserId1, @TemplateNameId2),
		(@UserId2, @TemplateNameId1),
		(@UserId2, @TemplateNameId3),
		(@UserId3, @TemplateNameId2);

	CREATE TABLE [dbo].[Dates]
	(
		Id INT PRIMARY KEY IDENTITY NOT NULL,
		Date DATETIME2 NOT NULL UNIQUE
	);

	INSERT INTO
		[dbo].[Dates]
		(Date)
	VALUES
		('20210504'),
		('20210903'),
		('20210904'),
		('20210905'),
		('20220829'),
		('20220830'),
		('20220901'),
		('20220902'),
		('20220903'),
		('20221005'),
		('20221008'),
		('20221010'),
		('20221019'),
		('20230221'),
		('20230301'),
		('20230303'),
		('20230321'),
		('20231014');

	CREATE TABLE [dbo].[Amounts]
	(
		Id INT PRIMARY KEY IDENTITY NOT NULL,
		Amount MONEY NOT NULL UNIQUE
	);

	INSERT INTO
		[dbo].[Amounts]
		(Amount)
	VALUES
		(2.25),
		(5.23),
		(6.00),
		(8.29),
		(11.23),
		(12.13),
		(15.87),
		(20.14),
		(23.14),
		(23.17),
		(70.22),
		(80.22),
		(100.25),
		(375.23),
		(750.82),
		(1000.23),
		(1100.50),
		(1200.14),
		(1200.23);

	CREATE TABLE [dbo].[Descriptions]
	(
		Id INT PRIMARY KEY IDENTITY NOT NULL,
		Description NVARCHAR(255) NOT NULL UNIQUE
	);

	INSERT INTO
		[dbo].[Descriptions]
		(Description)
	VALUES
		('Beer'),
		('Car Payment'),
		('Car Wash'),
		('Eating Out'),
		('Gas'),
		('Internet'),
		('Laundry'),
		('Paycheck'),
		('Power'),
		('Rent'),
		('Snacks'),
		('Take Out'),
		('Tip'),
		('Fees'),
		('Water');

	CREATE TABLE [dbo].[Budgets]
	(
		Id INT PRIMARY KEY IDENTITY NOT NULL,
		UserBudgetId int NOT NULL,
		DateId int NOT NULL,
		AmountId int NOT NULL,
		DescriptionId int NOT NULL,
		Credit bit NOT NULL,
		CONSTRAINT FK_UserBudgetName_BudgetsUserBudgetId FOREIGN KEY (UserBudgetId)
		REFERENCES UsersBudgetNames (Id)
		ON DELETE CASCADE,
		CONSTRAINT FK_Date_BudgetsDateId FOREIGN KEY (DateId)
		REFERENCES Dates (Id)
		ON DELETE CASCADE,
		CONSTRAINT FK_Amount_BudgetsAmountId FOREIGN KEY (AmountId)
		REFERENCES Amounts (Id)
		ON DELETE CASCADE,
		CONSTRAINT FK_Description_BudgetsDescriptionId FOREIGN KEY (DescriptionId)
		REFERENCES Descriptions (Id)
		ON DELETE CASCADE
	);

	INSERT INTO
		[dbo].[Budgets]
		(UserBudgetId, DateId, AmountId, DescriptionId, Credit)
	VALUES
		(1,3,8,5,0),
		(1,4,6,4,0),
		(1,6,15,8,1),
		(1,7,13,9,0),
		(1,7,14,2,0),
		(1,9,18,10,0),
		(1,10,10,1,0),
		(1,10,3,3,0),
		(1,14,2,13,0),
		(1,15,15,8,1),
		(2,2,8,5,0),
		(2,4,7,4,0),
		(2,6,15,8,1),
		(2,7,5,9,0),
		(2,8,12,6,0),
		(2,9,18,10,0),
		(2,10,10,1,0),
		(2,11,3,3,0),
		(2,14,1,7,0),
		(2,15,15,8,1),
		(3,9,9,5,0),
		(3,4,6,4,0),
		(3,6,15,8,1),
		(3,7,13,9,0),
		(3,8,12,6,0),
		(3,8,17,10,0),
		(3,13,10,11,0),
		(3,10,3,3,0),
		(3,14,2,11,0),
		(3,16,16,8,1),
		(4,1,8,5,0),
		(4,4,6,4,0),
		(4,5,15,8,1),
		(4,7,13,9,0),
		(4,8,11,6,0),
		(4,9,18,10,0),
		(4,18,10,1,0),
		(4,12,3,3,0),
		(4,14,4,11,0),
		(4,15,19,8,1),
		(5,3,8,5,0),
		(5,4,6,4,0),
		(5,6,15,8,1),
		(5,7,13,9,0),
		(5,8,12,6,0),
		(5,9,18,10,0),
		(5,10,10,1,0),
		(5,10,3,3,0),
		(5,17,2,11,0),
		(5,15,15,8,1);

	CREATE TABLE [dbo].[Templates]
	(
		Id INT PRIMARY KEY IDENTITY NOT NULL,
		UserTemplateId int NOT NULL,
		DateId int,
		AmountId int,
		DescriptionId int,
		Credit bit,
		CONSTRAINT FK_UserTemplateName_BudgetsUserTemplateId FOREIGN KEY (UserTemplateId)
		REFERENCES UsersTemplateNames (Id)
		ON DELETE CASCADE,
		CONSTRAINT FK_Date_TemplatesDateId FOREIGN KEY (DateId)
		REFERENCES Dates (Id)
		ON DELETE CASCADE,
		CONSTRAINT FK_Amount_TemplatesAmountId FOREIGN KEY (AmountId)
		REFERENCES Amounts (Id)
		ON DELETE CASCADE,
		CONSTRAINT FK_Description_TemplatesDescriptionId FOREIGN KEY (DescriptionId)
		REFERENCES Descriptions (Id)
		ON DELETE CASCADE
	);

	INSERT INTO
		[dbo].[Templates]
		(UserTemplateId, DescriptionId)
	VALUES
		(1,4),
		(1,13),
		(2,1),
		(2,11),
		(2,15),
		(3,4),
		(3,13),
		(4,10),
		(4,14),
		(4,15),
		(5,11),
		(5,15);

END