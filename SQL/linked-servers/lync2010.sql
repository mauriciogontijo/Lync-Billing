/****** Object:  LinkedServer [X.X.X.X]    Script Date: 11/14/2013 13:26:49 ******/
EXEC master.dbo.sp_addlinkedserver @server = N'X.X.X.X', @srvproduct=N'SQL Server'
 /* For security reasons the linked server remote logins password is changed with ######## */
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'X.X.X.X',@useself=N'False',@locallogin=NULL,@rmtuser=N'ebill',@rmtpassword='########'

GO

EXEC master.dbo.sp_serveroption @server=N'X.X.X.X', @optname=N'collation compatible', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'X.X.X.X', @optname=N'data access', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'X.X.X.X', @optname=N'dist', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'X.X.X.X', @optname=N'pub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'X.X.X.X', @optname=N'rpc', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'X.X.X.X', @optname=N'rpc out', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'X.X.X.X', @optname=N'sub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'X.X.X.X', @optname=N'connect timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'X.X.X.X', @optname=N'collation name', @optvalue=null
GO

EXEC master.dbo.sp_serveroption @server=N'X.X.X.X', @optname=N'lazy schema validation', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'X.X.X.X', @optname=N'query timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'X.X.X.X', @optname=N'use remote collation', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'X.X.X.X', @optname=N'remote proc transaction promotion', @optvalue=N'true'
GO


