CREATE TABLE [dbo].[Sys_Buttons]
(
    [SysNo] INT NOT NULL PRIMARY KEY IDENTITY,
    ButtonName VARCHAR(20) NOT NULL,
    [ButtonNo] VARCHAR(20) NOT NULL,
    [ButtonIcon] VARCHAR(20) NOT NULL,
    [MenuNo] VARCHAR(50) NULL
)
