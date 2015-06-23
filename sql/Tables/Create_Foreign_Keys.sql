

ALTER TABLE [dbo].[Reward]  WITH CHECK ADD  CONSTRAINT FK_Reward_RewardStatus FOREIGN KEY([RewardStatusID])
REFERENCES [dbo].RewardStatus ([RewardStatusID])
GO

ALTER TABLE [dbo].[Reward] CHECK CONSTRAINT FK_Reward_RewardStatus
GO

ALTER TABLE [dbo].[Reward]  WITH CHECK ADD  CONSTRAINT [FK_Reward_RewardReason] FOREIGN KEY([RewardReasonID])
REFERENCES [dbo].[RewardReason] ([RewardReasonID])
GO

ALTER TABLE [dbo].[Reward]  WITH CHECK ADD  CONSTRAINT [FK_Reward_RewardType] FOREIGN KEY([RewardTypeID])
REFERENCES [dbo].[RewardType] ([RewardTypeID])
GO
ALTER TABLE [dbo].[Reward] CHECK CONSTRAINT [FK_Reward_RewardType]
GO

ALTER TABLE [dbo].[Reward] CHECK CONSTRAINT [FK_Reward_RewardReason]
GO
ALTER TABLE [dbo].[Reward] CHECK CONSTRAINT [FK_Reward_RewardStatus]
GO
