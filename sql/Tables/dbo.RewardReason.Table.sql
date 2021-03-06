USE [RewardRecognition]
GO
/****** Object:  Table [dbo].[Reason]    Script Date: 2/18/2015 2:21:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
Go 

IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'dbo.[FK_Reward_RewardReason]')
   AND parent_object_id = OBJECT_ID(N'dbo.Reward')
)
	ALTER TABLE [Reward] DROP Constraint [FK_Reward_RewardReason]

IF OBJECT_ID('dbo.[RewardReason]', 'U') IS NOT NULL
	Drop Table [dbo].[RewardReason]
GO
CREATE TABLE [dbo].[RewardReason](
	[RewardReasonID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [char](1) NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime]  NOT NULL DEFAULT GETDATE(),
	[CreatedBy] [varchar](50) NOT NULL DEFAULT SUSER_SNAME(),
 CONSTRAINT [PK_RewardReason] PRIMARY KEY CLUSTERED 
(
	[RewardReasonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[RewardReason] ON 

INSERT [dbo].[RewardReason] ([RewardReasonID], [Code], [Description], IsActive) VALUES (1, N'A', N'Assisted with tasks outside of core job responsibility', 1)
INSERT [dbo].[RewardReason] ([RewardReasonID], [Code], [Description], IsActive) VALUES (2, N'B', N'Participated on a project team or group in an exemplary manner', 1)
INSERT [dbo].[RewardReason] ([RewardReasonID], [Code], [Description], IsActive) VALUES (3, N'C', N'Exceeded expectations on a task or team project', 1)
INSERT [dbo].[RewardReason] ([RewardReasonID], [Code], [Description], IsActive) VALUES (4, N'D', N'Came up with an innovative solution for an urgent deadline', 1)
INSERT [dbo].[RewardReason] ([RewardReasonID], [Code], [Description], IsActive) VALUES (5, N'E', N'Went the extra mile for a customer', 1)
INSERT [dbo].[RewardReason] ([RewardReasonID], [Code], [Description], IsActive) VALUES (6, N'F', N'Worked extra hours to complete an urgent task', 1)
INSERT [dbo].[RewardReason] ([RewardReasonID], [Code], [Description], IsActive) VALUES (7, N'G', N'Showed exemplary core values and or basic principles', 1)
INSERT [dbo].[RewardReason] ([RewardReasonID], [Code], [Description], IsActive) VALUES (8, N'O', N'Other', 1)
SET IDENTITY_INSERT [dbo].[RewardReason] OFF
ALTER TABLE [dbo].[RewardReason] ADD  CONSTRAINT [DF_Reason_Archive]  DEFAULT ((1)) FOR IsActive
GO
