--添加列
ALTER TABLE dbo.Sys_Menu ADD MenuNo VARCHAR(20) DEFAULT ''

--更指定列的长度
ALTER TABLE dbo.Sys_Menu ALTER COLUMN MenuNo VARCHAR(50)

