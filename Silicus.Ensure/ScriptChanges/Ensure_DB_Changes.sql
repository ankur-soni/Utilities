----Clear migration history
--delete from __MigrationHistory

--Create QuestionStatusDetails table

CREATE TABLE [dbo].[QuestionStatusDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QuestionId] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[ChangedBy] [int] NOT NULL,
	[ChangedDate] [datetime] NOT NULL,
	[Comment] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.QuestionStatusDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[QuestionStatusDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.QuestionStatusDetails_dbo.Question_Id] FOREIGN KEY([QuestionId])
REFERENCES [dbo].[Question] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[QuestionStatusDetails] CHECK CONSTRAINT [FK_dbo.QuestionStatusDetails_dbo.Question_Id]
GO

CREATE TABLE [dbo].[Technology](
	[TechnologyId] [int] IDENTITY(1,1) NOT NULL,
	[TechnologyName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](200) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL DEFAULT ((3900)),
 CONSTRAINT [PK_dbo.Technology] PRIMARY KEY CLUSTERED 
(
	[TechnologyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

INSERT INTO [dbo].[Technology]
           ([TechnologyName]
           ,[Description]
           ,[IsActive]
           ,[ModifiedDate]
           ,[ModifiedBy]
           ,[CreatedDate]
           ,[CreatedBy])
     VALUES
           ('.NET'
           ,'.NET'
           ,1
           ,null
           ,null
           ,GETDATE()
           ,3900)
GO

--Modify Question table
ALTER TABLE Question
ADD [Status] int NOT NULL DEFAULT(1)

ALTER TABLE Question
ADD [TechnologyId] int NOT NULL DEFAULT(1)

ALTER TABLE [dbo].[Question]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Question_dbo.Technology_TechnologyId] FOREIGN KEY([TechnologyId])
REFERENCES [dbo].[Technology] ([TechnologyId])
ON DELETE CASCADE

ALTER TABLE  [dbo].[Question] ALTER COLUMN ModifiedOn datetime NULL
ALTER TABLE  [dbo].[Question] ALTER COLUMN ModifiedBy int NULL

  ALTER TABLE [dbo].[UserTestDetails]
ALTER COLUMN Mark DECIMAL(5,2)
