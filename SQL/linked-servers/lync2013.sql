/****** Object:  LinkedServer [10.1.1.170\LYNC2013MON01]    Script Date: 11/14/2013 13:27:45 ******/
EXEC master.dbo.sp_addlinkedserver @server = N'10.1.1.170\LYNC2013MON01', @srvproduct=N'SQL Server'
 /* For security reasons the linked server remote logins password is changed with ######## */
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'10.1.1.170\LYNC2013MON01',@useself=N'False',@locallogin=NULL,@rmtuser=N'ebill',@rmtpassword='########'

GO

EXEC master.dbo.sp_serveroption @server=N'10.1.1.170\LYNC2013MON01', @optname=N'collation compatible', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'10.1.1.170\LYNC2013MON01', @optname=N'data access', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'10.1.1.170\LYNC2013MON01', @optname=N'dist', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'10.1.1.170\LYNC2013MON01', @optname=N'pub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'10.1.1.170\LYNC2013MON01', @optname=N'rpc', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'10.1.1.170\LYNC2013MON01', @optname=N'rpc out', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'10.1.1.170\LYNC2013MON01', @optname=N'sub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'10.1.1.170\LYNC2013MON01', @optname=N'connect timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'10.1.1.170\LYNC2013MON01', @optname=N'collation name', @optvalue=null
GO

EXEC master.dbo.sp_serveroption @server=N'10.1.1.170\LYNC2013MON01', @optname=N'lazy schema validation', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'10.1.1.170\LYNC2013MON01', @optname=N'query timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'10.1.1.170\LYNC2013MON01', @optname=N'use remote collation', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'10.1.1.170\LYNC2013MON01', @optname=N'remote proc transaction promotion', @optvalue=N'true'
GO


