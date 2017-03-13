CREATE PROCEDURE [dbo].[sp_InsertCorrectedFiles] @path nvarchar(500), @inputDataFileName nvarchar(500), @mode int
AS
BEGIN

DECLARE @Command nvarchar(256)
DECLARE @FileName VARCHAR(100)
DECLARE @FileExists int
DECLARE @inputFileFullPath nvarchar(1000) = @path + @inputDataFileName;
DECLARE @TableName nvarchar(100)

SELECT @TableName = CASE WHEN @mode = 1 
THEN  '[EudoxusOsy].[dbo].[KpsOnly]'
ELSE '[EudoxusOsy].[dbo].[ReceiptsOnly]'
END

DECLARE @truncatesql nvarchar(500) = 'TRUNCATE table ' + @TableName
exec(@truncatesql)

SET @FileName = @path + 'ErrorRows.csv'
SET @Command = 'del ' + @FileName
EXEC master..xp_FileExist @FileName, @FileExists out
IF @FileExists =1
EXEC master..xp_cmdShell @Command

SET @FileName = @path + 'ErrorRows.csv.Error.Txt'
SET @Command = 'del ' + @FileName
EXEC master..xp_FileExist @FileName, @FileExists out
IF @FileExists =1
EXEC master..xp_cmdShell @Command


DECLARE @sql nvarchar(max) = 'BULK INSERT ' + @TableName+ '
    FROM ''' + @inputFileFullPath + ''' 
	WITH
    (
    FIRSTROW = 1,
    FIELDTERMINATOR = '','',  --CSV field delimiter
    ROWTERMINATOR = ''0x0a'',   --Use to shift the control to next row
	ERRORFILE = ''' + @path + 'ErrorRows.csv' + ''',
    TABLOCK
    )'

exec(@sql)

END
GO


