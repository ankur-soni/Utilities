USE [@DatabaseName];

IF NOT EXISTS(SELECT * FROM sys.columns 
        WHERE [name] = N'Code' AND [object_id] = OBJECT_ID(N'Country'))
BEGIN

ALTER TABLE [Country]
ADD [Code] nvarchar(max) NULL

END

GO
