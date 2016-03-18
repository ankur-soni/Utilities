-- DECLARATION :
DECLARE @nowUtc datetime,
@now datetime

DECLARE @return_value int,
@UserId uniqueidentifier


set @nowUtc = getutcdate() 
set @now = getdate()