USE master

DECLARE @SQL varchar( max)

SELECT @SQL = COALESCE(@SQL ,'') + 'Kill ' + Convert( varchar, SPId) + ';'
FROM MASTER ..SysProcesses
WHERE DBId = DB_ID('[@DatabaseName]') AND SPId <> @@SPId

--SELECT @SQL
EXEC(@SQL )
