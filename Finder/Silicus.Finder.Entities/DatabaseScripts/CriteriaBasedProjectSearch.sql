CREATE PROCEDURE [dbo].[sp_ProjectSelect]
 @ProjectName NVarchar(100),
 @ProjectCode NVarchar(10)
 
AS
    Set NoCount ON

    Declare @SQLQuery AS NVarchar(4000)
    Declare @ParamDefinition AS NVarchar(2000) 

    Set @SQLQuery = 'Select * From Project where (1=1) ' 

    If @ProjectName Is Not Null 
         Set @SQLQuery = @SQLQuery + ' And (ProjectName = @ProjectName)'

    If @ProjectCode Is Not Null
         Set @SQLQuery = @SQLQuery + ' And (ProjectCode = @ProjectCode)' 
  
    Set @ParamDefinition =      ' @ProjectName NVarchar(100),
                @ProjectCode NVarchar(10)'

    Execute sp_ProjectSelect @SQLQuery, 
                @ProjectName,
    @ProjectCode

GO