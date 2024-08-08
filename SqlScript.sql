/****** Object:  Database [CliniqonFindFriendsDB]    Script Date: 2024-08-08 18:12:59 ******/
CREATE DATABASE [CliniqonFindFriendsDB]  (EDITION = 'GeneralPurpose', SERVICE_OBJECTIVE = 'GP_S_Gen5_1', MAXSIZE = 32 GB) WITH CATALOG_COLLATION = SQL_Latin1_General_CP1_CI_AS, LEDGER = OFF;
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET COMPATIBILITY_LEVEL = 160
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET  MULTI_USER 
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET ENCRYPTION ON
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 100, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
/*** The scripts of database scoped configurations in Azure should be executed inside the target database connection. ***/
GO
-- ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 8;
GO
/****** Object:  Table [dbo].[CompanyFriends]    Script Date: 2024-08-08 18:12:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyFriends](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[FriendId] [int] NOT NULL,
	[ProfileMatchPercentage] [float] NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [bigint] NULL,
	[UpdatedBy] [int] NULL,
	[UpdatedOn] [bigint] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_CompanyFriends] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompanyUsersData]    Script Date: 2024-08-08 18:12:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyUsersData](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserUniqueId] [varchar](50) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[DateOfBirth] [bigint] NOT NULL,
	[Designation] [varchar](50) NOT NULL,
	[Gender] [varchar](10) NOT NULL,
	[ProfilePicture] [varchar](1000) NOT NULL,
	[Country] [varchar](100) NOT NULL,
	[FavoriteColor] [varchar](20) NOT NULL,
	[FavoriteActor] [varchar](100) NOT NULL,
	[CreatedOn] [bigint] NOT NULL,
	[UpdatedOn] [bigint] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_CompanyUsersData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[CompanyFriendsProfileView]    Script Date: 2024-08-08 18:12:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CompanyFriendsProfileView]
AS
SELECT dbo.CompanyUsersData.UserUniqueId, dbo.CompanyUsersData.Name, dbo.CompanyUsersData.Email, dbo.CompanyUsersData.DateOfBirth, dbo.CompanyUsersData.Designation, dbo.CompanyUsersData.Gender, dbo.CompanyUsersData.ProfilePicture, dbo.CompanyUsersData.Country, dbo.CompanyUsersData.FavoriteColor, 
           dbo.CompanyUsersData.FavoriteActor, dbo.CompanyUsersData.CreatedOn, dbo.CompanyUsersData.IsActive, dbo.CompanyFriends.UserId, dbo.CompanyFriends.FriendId, dbo.CompanyFriends.IsActive AS CompanyFriendIsActive, dbo.CompanyUsersData.IsDeleted, dbo.CompanyFriends.IsDeleted AS CompanyFriendIsDeleted, 
           dbo.CompanyFriends.ProfileMatchPercentage
FROM   dbo.CompanyUsersData LEFT OUTER JOIN
           dbo.CompanyFriends ON dbo.CompanyUsersData.Id = dbo.CompanyFriends.FriendId
WHERE (dbo.CompanyFriends.IsDeleted = 0) AND (dbo.CompanyUsersData.IsDeleted = 0) AND (dbo.CompanyFriends.IsActive = 1) AND (dbo.CompanyUsersData.IsActive = 1) AND (dbo.CompanyUsersData.Gender = 'Male') AND (dbo.CompanyUsersData.FavoriteColor LIKE '%%') AND (dbo.CompanyUsersData.FavoriteActor LIKE '%%')
GO
/****** Object:  Table [dbo].[ErrorLog]    Script Date: 2024-08-08 18:12:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ErrorLog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ErrorMessage] [varchar](4000) NULL,
	[ErrorSeverity] [int] NULL,
	[ErrorState] [int] NULL,
	[ErrorComingFrom] [varchar](20) NULL,
	[ErrorPage] [varchar](100) NULL,
	[CreatedOn] [bigint] NULL,
 CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefreshTokenData]    Script Date: 2024-08-08 18:12:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefreshTokenData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RefreshToken] [varchar](100) NOT NULL,
	[Expires] [bigint] NOT NULL,
	[CreatedOn] [bigint] NOT NULL,
	[CreatedByIp] [varchar](45) NULL,
	[IsActive] [bit] NOT NULL,
	[UserId] [int] NOT NULL,
	[Browser] [varchar](300) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK__RefreshT__3214EC27B37FC211] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserLogin]    Script Date: 2024-08-08 18:12:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLogin](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyUserId] [int] NULL,
	[UserName] [varchar](100) NULL,
	[Password] [varchar](300) NULL,
	[CreatedOn] [bigint] NULL,
	[UpdatedOn] [bigint] NULL,
	[UpdatedBy] [int] NULL,
	[IsActive] [bit] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_UserLogin] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CompanyFriends] ADD  CONSTRAINT [DF_CompanyFriends_ProfileMatchPercentage]  DEFAULT ((0)) FOR [ProfileMatchPercentage]
GO
ALTER TABLE [dbo].[CompanyFriends] ADD  CONSTRAINT [DF_CompanyFriends_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[CompanyFriends] ADD  CONSTRAINT [DF_CompanyFriends_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[CompanyUsersData] ADD  CONSTRAINT [DF_CompanyUsersData_ProfilePicture]  DEFAULT ('https://bazaarnear.com/man.png') FOR [ProfilePicture]
GO
ALTER TABLE [dbo].[CompanyUsersData] ADD  CONSTRAINT [DF_CompanyUsersData_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[CompanyUsersData] ADD  CONSTRAINT [DF_CompanyUsersData_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[RefreshTokenData] ADD  CONSTRAINT [DF__RefreshTo__Creat__52593CB8]  DEFAULT (NULL) FOR [CreatedByIp]
GO
ALTER TABLE [dbo].[RefreshTokenData] ADD  CONSTRAINT [DF__RefreshTo__Brows__5441852A]  DEFAULT (NULL) FOR [Browser]
GO
ALTER TABLE [dbo].[RefreshTokenData] ADD  CONSTRAINT [DF_RefreshTokenData_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[UserLogin] ADD  CONSTRAINT [DF_UserLogin_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[UserLogin] ADD  CONSTRAINT [DF_UserLogin_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[CompanyFriends]  WITH CHECK ADD  CONSTRAINT [FK_CompanyFriends_CompanyUsersData] FOREIGN KEY([UserId])
REFERENCES [dbo].[CompanyUsersData] ([Id])
GO
ALTER TABLE [dbo].[CompanyFriends] CHECK CONSTRAINT [FK_CompanyFriends_CompanyUsersData]
GO
ALTER TABLE [dbo].[CompanyFriends]  WITH CHECK ADD  CONSTRAINT [FK_CompanyFriends_CompanyUsersData1] FOREIGN KEY([FriendId])
REFERENCES [dbo].[CompanyUsersData] ([Id])
GO
ALTER TABLE [dbo].[CompanyFriends] CHECK CONSTRAINT [FK_CompanyFriends_CompanyUsersData1]
GO
ALTER TABLE [dbo].[UserLogin]  WITH CHECK ADD  CONSTRAINT [FK_UserLogin_CompanyUsersData] FOREIGN KEY([CompanyUserId])
REFERENCES [dbo].[CompanyUsersData] ([Id])
GO
ALTER TABLE [dbo].[UserLogin] CHECK CONSTRAINT [FK_UserLogin_CompanyUsersData]
GO
/****** Object:  StoredProcedure [dbo].[GetProfilesByProfileMatchPercentage]    Script Date: 2024-08-08 18:12:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetProfilesByProfileMatchPercentage](
@UserId int

)

As
Begin
--Begin Try
Declare @TotalRowCount int,@Counter int=1;
CREATE TABLE #TempUserProfiles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	UserId int,
    DateOfBirth bigint,
    Country varchar(100),
	FavoriteColor varchar(20),
	FavoriteActor varchar(100)
);
CREATE TABLE #TempUserProfilesMatched (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	UserId int,
    Designation varchar(50),
    ProfilePicture varchar(1000),
	Name varchar(100),
	ProfileMatchPercentage float
);
insert into #TempUserProfiles(UserId,DateOfBirth,Country,FavoriteColor,FavoriteActor) 
select Id,DateOfBirth,Country,FavoriteColor,FavoriteActor from CompanyUsersData where CompanyUsersData.Id!=@UserId;
set @TotalRowCount=(select count(Id) from #TempUserProfiles);
WHILE @Counter <= @TotalRowCount
BEGIN
   Declare @ProfileMatchPercentage float=0,@MatchingProfileId int;
   set @MatchingProfileId=(select UserId from #TempUserProfiles where #TempUserProfiles.Id=@Counter);
   -- Getting the Prifle Match Percentage
		WITH RowComparison AS (
		SELECT
        
			-- Calculate number of matches
			(CASE WHEN cud1.DateOfBirth = cud2.DateOfBirth THEN 1 ELSE 0 END +
			 CASE WHEN cud1.Country = cud2.Country THEN 1 ELSE 0 END +
			   CASE WHEN cud1.FavoriteColor = cud2.FavoriteColor THEN 1 ELSE 0 END +
			 CASE WHEN cud1.FavoriteActor = cud2.FavoriteActor THEN 1 ELSE 0 END) AS MatchCount
		FROM
			CompanyUsersData cud1
		JOIN
			CompanyUsersData cud2
		ON
			cud1.Id != cud2.Id where cud1.Id =@UserId and cud2.id=@MatchingProfileId)
		SELECT top 1 @ProfileMatchPercentage= (MatchCount * 100.0 / 4 ) -- Divide by the number of columns being compared (4 in this case)
		FROM RowComparison ;
	
	Insert into #TempUserProfilesMatched(UserId,Designation,Name,ProfilePicture,ProfileMatchPercentage) 
	select id,Designation,Name,ProfilePicture,@ProfileMatchPercentage from CompanyUsersData where id=@MatchingProfileId;
    SET @Counter = @Counter + 1;
END;

select UserId as Id,Designation,ProfilePicture,Name,ProfileMatchPercentage from #TempUserProfilesMatched where #TempUserProfilesMatched.ProfileMatchPercentage > 50 
Order By #TempUserProfilesMatched.ProfileMatchPercentage Desc;
Drop table #TempUserProfilesMatched;
Drop Table #TempUserProfiles;
End

GO
/****** Object:  StoredProcedure [dbo].[SaveCompanyFriendsData]    Script Date: 2024-08-08 18:12:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SaveCompanyFriendsData](
@UserId int
,@FriendId int
,@CreatedBy int
)
As
Begin
	Declare @CurrentUnixDatetime bigint;
	set @CurrentUnixDatetime=(SELECT DATEDIFF(SECOND, '1970-01-01', GETUTCDATE()));

	Begin TRY
	
		Declare @ReturnMessage varchar(10),@CompanyUserId int,@ProfileMatchPercentage float;
	
	

		-- Getting the Prifle Match Percentage
		WITH RowComparison AS (
		SELECT
        
			-- Calculate number of matches
			(CASE WHEN cud1.DateOfBirth = cud2.DateOfBirth THEN 1 ELSE 0 END +
			 CASE WHEN cud1.Country = cud2.Country THEN 1 ELSE 0 END +
			   CASE WHEN cud1.FavoriteColor = cud2.FavoriteColor THEN 1 ELSE 0 END +
			 CASE WHEN cud1.FavoriteActor = cud2.FavoriteActor THEN 1 ELSE 0 END) AS MatchCount
		FROM
			CompanyUsersData cud1
		JOIN
			CompanyUsersData cud2
		ON
			cud1.Id != cud2.Id where cud1.Id =@UserId and cud2.id=@FriendId)
		SELECT top 1 @ProfileMatchPercentage= (MatchCount * 100.0 / 4 ) -- Divide by the number of columns being compared (4 in this case)
		FROM RowComparison ;
	
		INSERT INTO [dbo].[CompanyFriends]([UserId],[FriendId],[CreatedBy],[CreatedOn],[ProfileMatchPercentage])
		VALUES(@UserId,@FriendId,@CreatedBy,@CurrentUnixDatetime,@ProfileMatchPercentage);
		set @ReturnMessage='Success';
	
			
	End TRY
    Begin CATCH
	
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT @ErrorMessage = ERROR_MESSAGE(),@ErrorSeverity = ERROR_SEVERITY(),@ErrorState = ERROR_STATE();
        -- loging the error to error Log Table
        INSERT INTO ErrorLog (ErrorMessage, ErrorSeverity, ErrorState,ErrorComingFrom,ErrorPage,CreatedOn)
        VALUES (@ErrorMessage, @ErrorSeverity, @ErrorState,'Stored Procedure','SaveCompanyUserData',@CurrentUnixDatetime);
		--Set Failed Message to return message
		set @ReturnMessage='Failed';
    END CATCH
END;
select @ReturnMessage;
GO
/****** Object:  StoredProcedure [dbo].[SaveCompanyUserData]    Script Date: 2024-08-08 18:12:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SaveCompanyUserData](
@Name varchar(100)
,@UserName varchar(100)
,@Email varchar(100)
,@Password varchar(300)
,@DateOfBirth bigint
,@Designation varchar(50)
,@Gender varchar(10)
,@ProfilePicture varchar(1000)
,@Country varchar(100)
,@FavoriteColor varchar(20)
,@FavoriteActor varchar(100)
)
As
Begin
	Declare @CurrentUnixDatetime bigint,@CompanyUserId int,@AlreadyCount int,@ReturnMessage varchar(10);
	Declare @ProfilePictureIfNotExist varchar(50);
	if(@ProfilePicture='')
	Begin
		if(@Gender='female')
			set @ProfilePictureIfNotExist='https://bazaarnear.com/female.png';
		else
			set @ProfilePictureIfNotExist='https://bazaarnear.com/man.png';
	End;
	else
		set @ProfilePictureIfNotExist=@ProfilePicture;
	set @CurrentUnixDatetime=(SELECT DATEDIFF(SECOND, '1970-01-01', GETUTCDATE()));
	set @AlreadyCount=(select count(Id) from CompanyUsersData where [Name]=@Name and [Email]=@Email 
	and [DateOfBirth]=@DateOfBirth and [Designation]=@Designation and [Gender]=@Gender and [Country]=@Country 
	and [IsActive]=1 and [IsDeleted]=0 )
	if(@AlreadyCount>0) 
	Begin
		set  @ReturnMessage='Already';
	end;
	else
	Begin
		Begin TRY
			BEGIN TRANSACTION;
				
				--insert into Company User Data
				INSERT INTO [dbo].[CompanyUsersData]([UserUniqueId],[Name],[Email],[DateOfBirth],[Designation],[Gender],
				[ProfilePicture],[Country],[FavoriteColor],[FavoriteActor],[CreatedOn])
				VALUES((SUBSTRING(CONVERT(VARCHAR(255), NEWID()), 1, 8)),@Name,@Email,@DateOfBirth, @Designation,@Gender,@ProfilePictureIfNotExist,@Country,@FavoriteColor,
				@FavoriteActor,@CurrentUnixDatetime);
				set @CompanyUserId=(select SCOPE_IDENTITY());
				INSERT INTO [dbo].[UserLogin]([CompanyUserId],[UserName],[Password],[CreatedOn])VALUES
				(@CompanyUserId,@UserName,@Password,@CurrentUnixDatetime);
				set @ReturnMessage='Success';
			COMMIT TRANSACTION;
			
		End TRY
		Begin CATCH
			ROLLBACK TRANSACTION;
			DECLARE @ErrorMessage NVARCHAR(4000);
			DECLARE @ErrorSeverity INT;
			DECLARE @ErrorState INT;

			SELECT @ErrorMessage = ERROR_MESSAGE(),@ErrorSeverity = ERROR_SEVERITY(),@ErrorState = ERROR_STATE();
			-- loging the error to error Log Table
			INSERT INTO ErrorLog (ErrorMessage, ErrorSeverity, ErrorState,ErrorComingFrom,ErrorPage,CreatedOn)
			VALUES (@ErrorMessage, @ErrorSeverity, @ErrorState,'Stored Procedure','SaveCompanyUserData',@CurrentUnixDatetime);
			--Set Failed Message to return message
			set @ReturnMessage='Failed';
		END CATCH
	End;
END;
select @ReturnMessage;
GO
/****** Object:  StoredProcedure [dbo].[SaveRefreshToken]    Script Date: 2024-08-08 18:12:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SaveRefreshToken](
@RefreshToken varchar(100)
,@Expires bigint
,@CreatedByIp varchar(45)
,@UserId int
,@Browser varchar(300)
)
As
Begin
	Begin TRY
		Declare @CurrentUnixDatetime bigint,@ReturnMessage varchar(10);
		set @CurrentUnixDatetime=(SELECT DATEDIFF(SECOND, '1970-01-01', GETUTCDATE()));
		INSERT INTO [dbo].[RefreshTokenData]([RefreshToken],[Expires],[CreatedOn],[CreatedByIp],[IsActive],[IsDeleted],[UserId],[Browser])
		VALUES(@RefreshToken,@Expires,@CurrentUnixDatetime,@CreatedByIp,1,0,@UserId,@Browser);
		set  @ReturnMessage= 'Success';
		End TRY
		Begin CATCH
			ROLLBACK TRANSACTION;
			DECLARE @ErrorMessage NVARCHAR(4000);
			DECLARE @ErrorSeverity INT;
			DECLARE @ErrorState INT;

			SELECT @ErrorMessage = ERROR_MESSAGE(),@ErrorSeverity = ERROR_SEVERITY(),@ErrorState = ERROR_STATE();
			-- loging the error to error Log Table
			INSERT INTO ErrorLog (ErrorMessage, ErrorSeverity, ErrorState,ErrorComingFrom,ErrorPage,CreatedOn)
			VALUES (@ErrorMessage, @ErrorSeverity, @ErrorState,'Stored Procedure','SaveRefreshToken',@CurrentUnixDatetime);
			--Set Failed Message to return message
			set @ReturnMessage='Failed';
		END CATCH
select @ReturnMessage;
End;
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Manager,Juniou' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompanyUsersData', @level2type=N'COLUMN',@level2name=N'Designation'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Male.Female,Not Want to Specify' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CompanyUsersData', @level2type=N'COLUMN',@level2name=N'Gender'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Stored Procedure or Backend API ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ErrorLog', @level2type=N'COLUMN',@level2name=N'ErrorComingFrom'
GO
ALTER DATABASE [CliniqonFindFriendsDB] SET  READ_WRITE 
GO
