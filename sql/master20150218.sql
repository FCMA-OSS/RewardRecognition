:setvar Dir "c:\_tfs\AppDev\intranet\RewardRecognition\main\sql"

-- Create Database
PRINT '---------------------------------------------------------------------------------'
PRINT 'STARTING EXECUTION OF CreateDatabase SCRIPTS'
PRINT '-----------------------------------------------'

--PRINT 'Executing $(Dir)\CreateDatabase\RewardRecognition.Database.sql'
--:r $(Dir)\CreateDatabase\RewardRecognition.Database.sql
--Go

-- Tables
PRINT '---------------------------------------------------------------------------------'
PRINT 'STARTING EXECUTION OF Tables SCRIPTS'
PRINT '-----------------------------------------------'

PRINT 'Executing $(Dir)\Tables\dbo.RewardStatus.Table.sql'
:r $(Dir)\Tables\dbo.RewardStatus.Table.sql
Go

PRINT 'Executing $(Dir)\Tables\dbo.RewardType.Table.sql'
:r $(Dir)\Tables\dbo.RewardType.Table.sql
Go

PRINT 'Executing $(Dir)\Tables\dbo.Reason.Table.sql'
:r $(Dir)\Tables\dbo.RewardReason.Table.sql
Go

PRINT 'Executing $(Dir)\Tables\dbo.RewardHistory.Table.sql'
:r $(Dir)\Tables\dbo.RewardHistory.Table.sql
Go

PRINT 'Executing $(Dir)\Tables\dbo.Reward.Table.sql'
:r $(Dir)\Tables\dbo.Reward.Table.sql
Go


--FOREIGN KEYS
PRINT 'Executing $(Dir)\Tables\Create_Foreign_Keys.sql'
:r $(Dir)\Tables\Create_Foreign_Keys.sql
Go
