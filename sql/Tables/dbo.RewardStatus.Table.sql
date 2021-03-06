USE [RewardRecognition]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 2/18/2015 2:21:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
go

IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.[FK_Reward_RewardStatus]')
   AND parent_object_id = OBJECT_ID(N'dbo.Reward')
)
	ALTER TABLE Reward DROP Constraint FK_Reward_RewardStatus



IF OBJECT_ID('dbo.[RewardStatus]', 'U') IS NOT NULL
	Drop Table [dbo].RewardStatus
GO

CREATE TABLE [dbo].RewardStatus(
	[RewardStatusID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [char](1) NOT NULL,
	[Description] [varchar](50) NOT NULL,
	IsActive [bit] NOT NULL,
	[CreatedDate] [datetime]  NOT NULL DEFAULT GETDATE(),
	[CreatedBy] [varchar](50) NOT NULL DEFAULT SUSER_SNAME(),
 CONSTRAINT [PK_RewardStatus] PRIMARY KEY CLUSTERED 
(
	[RewardStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].RewardStatus ON 

INSERT [dbo].RewardStatus ([RewardStatusID], [Code], [Description], IsActive) VALUES (1, N'P', N'Pending Approval', 1)
INSERT [dbo].RewardStatus ([RewardStatusID], [Code], [Description], IsActive) VALUES (2, N'A', N'Approved', 1)
INSERT [dbo].RewardStatus ([RewardStatusID], [Code], [Description], IsActive) VALUES (3, N'D', N'Denied', 1)
INSERT [dbo].RewardStatus ([RewardStatusID], [Code], [Description], IsActive) VALUES (4, N'W', N'Withdrawn', 1)
INSERT [dbo].RewardStatus ([RewardStatusID], [Code], [Description], IsActive) VALUES (5, N'R', N'Redeemed', 1)
SET IDENTITY_INSERT [dbo].RewardStatus OFF
ALTER TABLE [dbo].RewardStatus ADD  CONSTRAINT [DF_Status_Archive]  DEFAULT ((1)) FOR IsActive
GO

