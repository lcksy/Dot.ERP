CREATE TABLE [dbo].[Sys_Menu]
(
    [SysNo] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ParentSysNo] INT NOT NULL, 
    [OrderNum] INT NOT NULL, 
    [Status] INT NOT NULL, 
    [Name] VARCHAR(20) NOT NULL, 
    [Href] VARCHAR(50) NULL, 
    [Icon] VARCHAR(50) NULL
)
