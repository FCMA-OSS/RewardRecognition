USE [RewardRecognition]
GO
/****** Object:  Table [dbo].[RewardType]    Script Date: 2/18/2015 2:21:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
go

IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.[FK_Reward_RewardType]')
   AND parent_object_id = OBJECT_ID(N'dbo.Reward')
)
ALTER TABLE Reward DROP Constraint [FK_Reward_RewardType]

IF OBJECT_ID('dbo.[RewardType]', 'U') IS NOT NULL
	Drop Table [dbo].[RewardType]
GO
CREATE TABLE [dbo].[RewardType](
	[RewardTypeID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [char](1) NOT NULL,
	[Description] [varchar](255) NOT NULL,
	[Amount] [decimal](18, 0) NULL,
	[NeedApproval] [bit] NOT NULL,
	IsActive [bit] NOT NULL,
	[CreatedDate] [datetime]  NOT NULL DEFAULT GETDATE(),
	[CreatedBy] [varchar](50) NOT NULL DEFAULT SUSER_SNAME(),
 CONSTRAINT [PK_AwardType] PRIMARY KEY CLUSTERED 
(
	[RewardTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[RewardType] ON 

INSERT [dbo].[RewardType] ([RewardTypeID], [Code], [Description], [Amount], [NeedApproval], IsActive) VALUES (1, N'A', N'$25 Gift Card', CAST(25 AS Decimal(18, 0)), 0, 1)
INSERT [dbo].[RewardType] ([RewardTypeID], [Code], [Description], [Amount], [NeedApproval], IsActive) VALUES (2, N'B', N'$50 Gift Card', CAST(50 AS Decimal(18, 0)), 1, 1)
INSERT [dbo].[RewardType] ([RewardTypeID], [Code], [Description], [Amount], [NeedApproval], IsActive) VALUES (3, N'C', N'$100 Gift Card', CAST(100 AS Decimal(18, 0)), 1, 1)
SET IDENTITY_INSERT [dbo].[RewardType] OFF
ALTER TABLE [dbo].[RewardType] ADD  CONSTRAINT [DF_RewardType_NeedApproval]  DEFAULT ((0)) FOR [NeedApproval]
GO
ALTER TABLE [dbo].[RewardType] ADD  CONSTRAINT [DF_RewardType_Archive]  DEFAULT ((1)) FOR IsActive
GO
