USE [RewardRecognition]
GO
/****** Object:  Table [dbo].[Reward]    Script Date: 2/18/2015 2:21:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.[FK_Reward_RewardType]')
   AND parent_object_id = OBJECT_ID(N'dbo.Reward')
)
	ALTER TABLE Reward DROP Constraint [FK_Reward_RewardType]

IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.[FK_Reward_RewardReason]')
   AND parent_object_id = OBJECT_ID(N'dbo.Reward')
)
	ALTER TABLE Reward DROP Constraint [FK_Reward_RewardReason]

IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.[FK_Reward_RewardStatus]')
   AND parent_object_id = OBJECT_ID(N'dbo.Reward')
)
	ALTER TABLE Reward DROP Constraint [FK_Reward_RewardStatus]

IF OBJECT_ID('dbo.[Reward]', 'U') IS NOT NULL
	Drop Table [dbo].[Reward]
GO
CREATE TABLE [dbo].[Reward](
	[RewardID] [int] IDENTITY(10000,1) NOT NULL,
	[RewardTypeID] [int] NOT NULL,
	[RewardReasonID] [int] NOT NULL,
	[OtherReason] [varchar](4000) NULL,
	[Recipient] [varchar](10) NOT NULL,
	[RecipientFullName] [varchar](100) NULL,
	[Supervisor] [varchar](10) NULL,
	[RewardStatusID] [int] NOT NULL,
	[CreatedDate] [datetime]  NOT NULL DEFAULT GETDATE(),
	[CreatedBy] [varchar](50) NOT NULL DEFAULT SUSER_SNAME(),
	[CreatedByFullName] [varchar](100) NULL,
	[LastChangedDate] [datetime] NULL,
	[ChangedBy] [varchar](10) NULL,
	[RedeemedDate] [datetime] NULL,
	[RedeemedBy] [varchar](10) NULL,
	[PresentationDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Reward] PRIMARY KEY CLUSTERED 
(
	[RewardID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
