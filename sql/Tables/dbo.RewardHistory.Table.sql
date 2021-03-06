USE [RewardRecognition]
GO
/****** Object:  Table [dbo].[RewardHistory]    Script Date: 2/18/2015 2:21:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
IF OBJECT_ID('dbo.[RewardHistory]', 'U') IS NOT NULL
Drop Table [dbo].[RewardHistory]
GO
CREATE TABLE [dbo].[RewardHistory](
	[RewardHistoryID] [int]  NOT NULL,
	[RewardID] [int] IDENTITY(1,1) NOT NULL,
	[RewardTypeID] [int] NOT NULL,
	[OtherDescription] [varchar](255) NULL,
	[OtherValue] [decimal](18, 0) NULL,
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
 CONSTRAINT [PK_AwardHistory] PRIMARY KEY CLUSTERED 
(
	[RewardHistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

