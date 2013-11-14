USE [master]
GO
/****** Object:  Database [tBill]    Script Date: 11/14/2013 13:35:06 ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'tBill')
BEGIN
CREATE DATABASE [tBill] ON  PRIMARY 
( NAME = N'tBill', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\tBill.mdf' , SIZE = 5523456KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'tBill_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\tBill_log.ldf' , SIZE = 9027840KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 COLLATE Latin1_General_BIN
END
GO
ALTER DATABASE [tBill] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [tBill].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [tBill] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [tBill] SET ANSI_NULLS OFF
GO
ALTER DATABASE [tBill] SET ANSI_PADDING OFF
GO
ALTER DATABASE [tBill] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [tBill] SET ARITHABORT OFF
GO
ALTER DATABASE [tBill] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [tBill] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [tBill] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [tBill] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [tBill] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [tBill] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [tBill] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [tBill] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [tBill] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [tBill] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [tBill] SET  DISABLE_BROKER
GO
ALTER DATABASE [tBill] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [tBill] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [tBill] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [tBill] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [tBill] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [tBill] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [tBill] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [tBill] SET  READ_WRITE
GO
ALTER DATABASE [tBill] SET RECOVERY FULL
GO
ALTER DATABASE [tBill] SET  MULTI_USER
GO
ALTER DATABASE [tBill] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [tBill] SET DB_CHAINING OFF
GO
USE [tBill]
GO
/****** Object:  User [mailsvc]    Script Date: 11/14/2013 13:35:06 ******/
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'mailsvc')
CREATE USER [mailsvc] FOR LOGIN [mailsvc] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [CCCLAB\rndadmin]    Script Date: 11/14/2013 13:35:06 ******/
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'CCCLAB\rndadmin')
CREATE USER [CCCLAB\rndadmin] FOR LOGIN [CCCLAB\rndadmin] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[Sites]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sites]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Sites](
	[SiteID] [int] IDENTITY(1,1) NOT NULL,
	[CountryCode] [nvarchar](3) COLLATE Latin1_General_CI_AI NULL,
	[SiteName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_Sites] PRIMARY KEY CLUSTERED 
(
	[SiteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Roles](
	[RoleName] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[RoleDescription] [nvarchar](100) COLLATE Latin1_General_CI_AI NULL,
	[RoleID] [int] NOT NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Rates_dxb-gw1.ae.ccg.local_2013_04_02]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rates_dxb-gw1.ae.ccg.local_2013_04_02]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Rates_dxb-gw1.ae.ccg.local_2013_04_02](
	[Rate_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[country_code_dialing_prefix] [bigint] NOT NULL,
	[rate] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_Rates_dxb-gw1.ae.ccg.local_2013_04_02] PRIMARY KEY CLUSTERED 
(
	[country_code_dialing_prefix] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Rates_10.7.32.83_2013_10_24]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rates_10.7.32.83_2013_10_24]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Rates_10.7.32.83_2013_10_24](
	[Rate_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[country_code_dialing_prefix] [bigint] NOT NULL,
	[rate] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_Rates_10.7.32.83_2013_10_24] PRIMARY KEY CLUSTERED 
(
	[country_code_dialing_prefix] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Rates_10.12.60.254_2013_04_02]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rates_10.12.60.254_2013_04_02]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Rates_10.12.60.254_2013_04_02](
	[Rate_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[country_code_dialing_prefix] [bigint] NOT NULL,
	[rate] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_Rates_10.12.60.254_2013_04_02] PRIMARY KEY CLUSTERED 
(
	[country_code_dialing_prefix] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Rates_10.1.1.5_2013_04_02]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rates_10.1.1.5_2013_04_02]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Rates_10.1.1.5_2013_04_02](
	[Rate_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[country_code_dialing_prefix] [bigint] NOT NULL,
	[rate] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_Rates_10.1.1.5_2013_04_02_1] PRIMARY KEY NONCLUSTERED 
(
	[country_code_dialing_prefix] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Rates_10.1.1.5_2013_04_02]') AND name = N'ClusteredIndex-20130523-101124')
CREATE UNIQUE CLUSTERED INDEX [ClusteredIndex-20130523-101124] ON [dbo].[Rates_10.1.1.5_2013_04_02] 
(
	[country_code_dialing_prefix] ASC,
	[rate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rates_10.1.1.57_2013_7_11]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rates_10.1.1.57_2013_7_11]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Rates_10.1.1.57_2013_7_11](
	[Rate_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[country_code_dialing_prefix] [bigint] NOT NULL,
	[rate] [decimal](18, 4) NOT NULL
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Rates_10.1.1.4_2013_04_02]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rates_10.1.1.4_2013_04_02]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Rates_10.1.1.4_2013_04_02](
	[Rate_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[country_code_dialing_prefix] [bigint] NOT NULL,
	[rate] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_Rates_10.1.1.4_2013_04_02] PRIMARY KEY NONCLUSTERED 
(
	[country_code_dialing_prefix] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Rates_10.1.1.4_2013_04_02]') AND name = N'ClusteredIndex-20130523-101106')
CREATE UNIQUE CLUSTERED INDEX [ClusteredIndex-20130523-101106] ON [dbo].[Rates_10.1.1.4_2013_04_02] 
(
	[country_code_dialing_prefix] ASC,
	[rate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rates_10.1.1.3_2013_04_02]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rates_10.1.1.3_2013_04_02]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Rates_10.1.1.3_2013_04_02](
	[Rate_ID] [bigint] IDENTITY(1,1) NOT NULL,
	[country_code_dialing_prefix] [bigint] NOT NULL,
	[rate] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_Rates_10.1.1.3_2013_04_02] PRIMARY KEY NONCLUSTERED 
(
	[country_code_dialing_prefix] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Rates_10.1.1.3_2013_04_02]') AND name = N'ClusteredIndex-20130523-101050')
CREATE UNIQUE CLUSTERED INDEX [ClusteredIndex-20130523-101050] ON [dbo].[Rates_10.1.1.3_2013_04_02] 
(
	[country_code_dialing_prefix] ASC,
	[rate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rates_10.1.0.12_2013_04_02]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rates_10.1.0.12_2013_04_02]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Rates_10.1.0.12_2013_04_02](
	[country_code_dialing_prefix] [bigint] NOT NULL,
	[rate] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_Rates_10.1.0.12_2013_04_02] PRIMARY KEY NONCLUSTERED 
(
	[country_code_dialing_prefix] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Rates_10.1.0.12_2013_04_02]') AND name = N'ClusteredIndex-20130523-100923')
CREATE UNIQUE CLUSTERED INDEX [ClusteredIndex-20130523-100923] ON [dbo].[Rates_10.1.0.12_2013_04_02] 
(
	[country_code_dialing_prefix] ASC,
	[rate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  UserDefinedTableType [dbo].[RatesTableType]    Script Date: 11/14/2013 13:35:06 ******/
IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'RatesTableType' AND ss.name = N'dbo')
CREATE TYPE [dbo].[RatesTableType] AS TABLE(
	[country_code_dialing_prefix] [bigint] NOT NULL,
	[rate] [decimal](18, 4) NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[country_code_dialing_prefix] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  Table [dbo].[Pools]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pools]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Pools](
	[PoolID] [int] IDENTITY(1,1) NOT NULL,
	[PoolFQDN] [nvarchar](256) COLLATE Latin1_General_BIN NOT NULL,
 CONSTRAINT [PK_Pools] PRIMARY KEY CLUSTERED 
(
	[PoolID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PhoneCallsExceptions]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCallsExceptions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PhoneCallsExceptions](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserUri] [nvarchar](450) COLLATE Latin1_General_CI_AI NULL,
	[Number] [varchar](100) COLLATE Latin1_General_BIN NULL,
	[Description] [nvarchar](450) COLLATE Latin1_General_CI_AI NULL,
 CONSTRAINT [PK_Vodafone Business Subscribers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCallsExceptions]') AND name = N'NonClusteredIndex-20131018-092614')
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131018-092614] ON [dbo].[PhoneCallsExceptions] 
(
	[UserUri] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCallsExceptions]') AND name = N'NonClusteredIndex-20131018-092636')
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131018-092636] ON [dbo].[PhoneCallsExceptions] 
(
	[Number] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhoneCalls2013]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCalls2013]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PhoneCalls2013](
	[SessionIdTime] [datetime] NOT NULL,
	[SessionIdSeq] [int] NOT NULL,
	[SourceUserUri] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DestinationUserUri] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SourceNumberUri] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DestinationNumberUri] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FromMediationServer] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ToMediationServer] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FromGateway] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ToGateway] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SourceUserEdgeServer] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DestinationUserEdgeServer] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ResponseTime] [datetime] NULL,
	[SessionEndTime] [datetime] NULL,
	[ServerFQDN] [nvarchar](257) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PoolFQDN] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[OnBehalf] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ReferedBy] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Duration] [numeric](8, 0) NULL,
	[marker_CallFrom] [bigint] NULL,
	[marker_CallTo] [bigint] NULL,
	[marker_CallToCountry] [varchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[marker_CallCost] [money] NULL,
	[marker_CallTypeID] [bigint] NULL,
	[marker_CallType] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[marker_TimeStamp] [datetime] NULL,
	[ui_MarkedOn] [datetime] NULL,
	[ui_UpdatedByUser] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ui_CallType] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ac_DisputeStatus] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ac_DisputeResolvedOn] [datetime] NULL,
	[ac_IsInvoiced] [nvarchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ac_InvoiceDate] [datetime] NULL,
	[Exclude] [bit] NULL,
 CONSTRAINT [PK_PhoneCalls2013] PRIMARY KEY CLUSTERED 
(
	[SessionIdTime] ASC,
	[SessionIdSeq] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCalls2013]') AND name = N'NonClusteredIndex-20131112-133958')
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131112-133958] ON [dbo].[PhoneCalls2013] 
(
	[SourceUserUri] ASC
)
INCLUDE ( [SessionIdTime],
[SessionIdSeq],
[DestinationUserUri],
[SourceNumberUri],
[DestinationNumberUri],
[FromMediationServer],
[ToMediationServer],
[FromGateway],
[ToGateway],
[SourceUserEdgeServer],
[DestinationUserEdgeServer],
[ResponseTime],
[SessionEndTime],
[ServerFQDN],
[PoolFQDN],
[OnBehalf],
[ReferedBy],
[Duration],
[marker_CallFrom],
[marker_CallTo],
[marker_CallToCountry],
[marker_CallCost],
[marker_CallTypeID],
[marker_CallType],
[marker_TimeStamp],
[ui_MarkedOn],
[ui_UpdatedByUser],
[ui_CallType],
[ac_DisputeStatus],
[ac_DisputeResolvedOn],
[ac_IsInvoiced],
[ac_InvoiceDate],
[Exclude]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCalls2013]') AND name = N'NonClusteredIndex-20131112-134103')
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131112-134103] ON [dbo].[PhoneCalls2013] 
(
	[marker_CallTypeID] ASC
)
INCLUDE ( [SessionIdTime],
[SessionIdSeq],
[SourceUserUri],
[DestinationUserUri],
[SourceNumberUri],
[DestinationNumberUri],
[FromMediationServer],
[ToMediationServer],
[FromGateway],
[ToGateway],
[SourceUserEdgeServer],
[DestinationUserEdgeServer],
[ResponseTime],
[SessionEndTime],
[ServerFQDN],
[PoolFQDN],
[OnBehalf],
[ReferedBy],
[Duration],
[marker_CallFrom],
[marker_CallTo],
[marker_CallToCountry],
[marker_CallCost],
[marker_CallType],
[marker_TimeStamp],
[ui_MarkedOn],
[ui_UpdatedByUser],
[ui_CallType],
[ac_DisputeStatus],
[ac_DisputeResolvedOn],
[ac_IsInvoiced],
[ac_InvoiceDate],
[Exclude]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCalls2013]') AND name = N'NonClusteredIndex-20131112-134223')
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131112-134223] ON [dbo].[PhoneCalls2013] 
(
	[ui_CallType] ASC
)
INCLUDE ( [SessionIdTime],
[SessionIdSeq],
[SourceUserUri],
[DestinationUserUri],
[SourceNumberUri],
[DestinationNumberUri],
[FromMediationServer],
[ToMediationServer],
[FromGateway],
[ToGateway],
[SourceUserEdgeServer],
[DestinationUserEdgeServer],
[ResponseTime],
[SessionEndTime],
[ServerFQDN],
[PoolFQDN],
[OnBehalf],
[ReferedBy],
[Duration],
[marker_CallFrom],
[marker_CallTo],
[marker_CallToCountry],
[marker_CallCost],
[marker_CallTypeID],
[marker_CallType],
[marker_TimeStamp],
[ui_MarkedOn],
[ui_UpdatedByUser],
[ac_DisputeStatus],
[ac_DisputeResolvedOn],
[ac_IsInvoiced],
[ac_InvoiceDate],
[Exclude]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCalls2013]') AND name = N'NonClusteredIndex-20131112-152758')
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131112-152758] ON [dbo].[PhoneCalls2013] 
(
	[ToGateway] ASC
)
INCLUDE ( [SessionIdTime],
[SessionIdSeq],
[SourceUserUri],
[DestinationUserUri],
[SourceNumberUri],
[DestinationNumberUri],
[FromMediationServer],
[ToMediationServer],
[FromGateway],
[SourceUserEdgeServer],
[DestinationUserEdgeServer],
[ResponseTime],
[SessionEndTime],
[ServerFQDN],
[PoolFQDN],
[OnBehalf],
[ReferedBy],
[Duration],
[marker_CallFrom],
[marker_CallTo],
[marker_CallToCountry],
[marker_CallCost],
[marker_CallTypeID],
[marker_CallType],
[marker_TimeStamp],
[ui_MarkedOn],
[ui_UpdatedByUser],
[ui_CallType],
[ac_DisputeStatus],
[ac_DisputeResolvedOn],
[ac_IsInvoiced],
[ac_InvoiceDate],
[Exclude]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhoneCalls2010]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCalls2010]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PhoneCalls2010](
	[SessionIdTime] [datetime] NOT NULL,
	[SessionIdSeq] [int] NOT NULL,
	[SourceUserUri] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DestinationUserUri] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SourceNumberUri] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DestinationNumberUri] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FromMediationServer] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ToMediationServer] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[FromGateway] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ToGateway] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SourceUserEdgeServer] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DestinationUserEdgeServer] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ResponseTime] [datetime] NULL,
	[SessionEndTime] [datetime] NULL,
	[ServerFQDN] [nvarchar](257) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PoolFQDN] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[OnBehalf] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ReferedBy] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Duration] [numeric](8, 0) NULL,
	[marker_CallFrom] [bigint] NULL,
	[marker_CallTo] [bigint] NULL,
	[marker_CallToCountry] [varchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[marker_CallCost] [money] NULL,
	[marker_CallTypeID] [bigint] NULL,
	[marker_CallType] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[marker_TimeStamp] [datetime] NULL,
	[ui_MarkedOn] [datetime] NULL,
	[ui_UpdatedByUser] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ui_CallType] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ac_DisputeStatus] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ac_DisputeResolvedOn] [datetime] NULL,
	[ac_IsInvoiced] [nvarchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ac_InvoiceDate] [datetime] NULL,
	[Exclude] [bit] NULL,
 CONSTRAINT [PK_PhoneCalls2010] PRIMARY KEY CLUSTERED 
(
	[SessionIdTime] ASC,
	[SessionIdSeq] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCalls2010]') AND name = N'NonClusteredIndex-20131114-114325')
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131114-114325] ON [dbo].[PhoneCalls2010] 
(
	[SourceUserUri] ASC
)
INCLUDE ( [SessionIdTime],
[SessionIdSeq],
[DestinationUserUri],
[SourceNumberUri],
[DestinationNumberUri],
[FromMediationServer],
[ToMediationServer],
[FromGateway],
[ToGateway],
[SourceUserEdgeServer],
[DestinationUserEdgeServer],
[ResponseTime],
[SessionEndTime],
[ServerFQDN],
[PoolFQDN],
[OnBehalf],
[ReferedBy],
[Duration],
[marker_CallFrom],
[marker_CallTo],
[marker_CallToCountry],
[marker_CallCost],
[marker_CallTypeID],
[marker_CallType],
[marker_TimeStamp],
[ui_MarkedOn],
[ui_UpdatedByUser],
[ui_CallType],
[ac_DisputeStatus],
[ac_DisputeResolvedOn],
[ac_IsInvoiced],
[ac_InvoiceDate],
[Exclude]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCalls2010]') AND name = N'NonClusteredIndex-20131114-114810')
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131114-114810] ON [dbo].[PhoneCalls2010] 
(
	[ui_CallType] ASC
)
INCLUDE ( [SessionIdTime],
[SessionIdSeq],
[SourceUserUri],
[DestinationUserUri],
[SourceNumberUri],
[DestinationNumberUri],
[FromMediationServer],
[ToMediationServer],
[FromGateway],
[ToGateway],
[SourceUserEdgeServer],
[DestinationUserEdgeServer],
[ResponseTime],
[SessionEndTime],
[ServerFQDN],
[PoolFQDN],
[OnBehalf],
[ReferedBy],
[Duration],
[marker_CallFrom],
[marker_CallTo],
[marker_CallToCountry],
[marker_CallCost],
[marker_CallTypeID],
[marker_CallType],
[marker_TimeStamp],
[ui_MarkedOn],
[ui_UpdatedByUser],
[ac_DisputeStatus],
[ac_DisputeResolvedOn],
[ac_IsInvoiced],
[ac_InvoiceDate],
[Exclude]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCalls2010]') AND name = N'NonClusteredIndex-20131114-114825')
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131114-114825] ON [dbo].[PhoneCalls2010] 
(
	[marker_CallTypeID] ASC
)
INCLUDE ( [SessionIdTime],
[SessionIdSeq],
[SourceUserUri],
[DestinationUserUri],
[SourceNumberUri],
[DestinationNumberUri],
[FromMediationServer],
[ToMediationServer],
[FromGateway],
[ToGateway],
[SourceUserEdgeServer],
[DestinationUserEdgeServer],
[ResponseTime],
[SessionEndTime],
[ServerFQDN],
[PoolFQDN],
[OnBehalf],
[ReferedBy],
[Duration],
[marker_CallFrom],
[marker_CallTo],
[marker_CallToCountry],
[marker_CallCost],
[marker_CallType],
[marker_TimeStamp],
[ui_MarkedOn],
[ui_UpdatedByUser],
[ui_CallType],
[ac_DisputeStatus],
[ac_DisputeResolvedOn],
[ac_IsInvoiced],
[ac_InvoiceDate],
[Exclude]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCalls2010]') AND name = N'NonClusteredIndex-20131114-115024')
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131114-115024] ON [dbo].[PhoneCalls2010] 
(
	[ToGateway] ASC
)
INCLUDE ( [SessionIdTime],
[SessionIdSeq],
[SourceUserUri],
[DestinationUserUri],
[SourceNumberUri],
[DestinationNumberUri],
[FromMediationServer],
[ToMediationServer],
[FromGateway],
[SourceUserEdgeServer],
[DestinationUserEdgeServer],
[ResponseTime],
[SessionEndTime],
[ServerFQDN],
[PoolFQDN],
[OnBehalf],
[ReferedBy],
[Duration],
[marker_CallFrom],
[marker_CallTo],
[marker_CallToCountry],
[marker_CallCost],
[marker_CallTypeID],
[marker_CallType],
[marker_TimeStamp],
[ui_MarkedOn],
[ui_UpdatedByUser],
[ui_CallType],
[ac_DisputeStatus],
[ac_DisputeResolvedOn],
[ac_IsInvoiced],
[ac_InvoiceDate],
[Exclude]) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhoneCalls]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCalls]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PhoneCalls](
	[SessionIdTime] [datetime] NOT NULL,
	[SessionIdSeq] [int] NOT NULL,
	[SourceUserUri] [nvarchar](450) COLLATE Latin1_General_CI_AI NULL,
	[DestinationUserUri] [nvarchar](450) COLLATE Latin1_General_CI_AI NULL,
	[SourceNumberUri] [nvarchar](450) COLLATE Latin1_General_CI_AI NULL,
	[DestinationNumberUri] [nvarchar](450) COLLATE Latin1_General_CI_AI NULL,
	[FromMediationServer] [nvarchar](256) COLLATE Latin1_General_CI_AI NULL,
	[ToMediationServer] [nvarchar](256) COLLATE Latin1_General_CI_AI NULL,
	[FromGateway] [nvarchar](256) COLLATE Latin1_General_CI_AI NULL,
	[ToGateway] [nvarchar](256) COLLATE Latin1_General_CI_AI NULL,
	[SourceUserEdgeServer] [nvarchar](256) COLLATE Latin1_General_CI_AI NULL,
	[DestinationUserEdgeServer] [nvarchar](256) COLLATE Latin1_General_CI_AI NULL,
	[ResponseTime] [datetime] NULL,
	[SessionEndTime] [datetime] NULL,
	[ServerFQDN] [nvarchar](257) COLLATE Latin1_General_CI_AI NULL,
	[PoolFQDN] [nvarchar](256) COLLATE Latin1_General_CI_AI NULL,
	[Duration] [numeric](8, 0) NULL,
	[marker_CallFrom] [bigint] NULL,
	[marker_CallTo] [bigint] NULL,
	[marker_CallToCountry] [varchar](3) COLLATE Latin1_General_CI_AI NULL,
	[marker_CallCost] [money] NULL,
	[marker_CallTypeID] [bigint] NULL,
	[marker_CallType] [nvarchar](450) COLLATE Latin1_General_CI_AI NULL,
	[marker_TimeStamp] [datetime] NULL,
	[ui_MarkedOn] [datetime] NULL,
	[ui_UpdatedByUser] [nvarchar](450) COLLATE Latin1_General_CI_AI NULL,
	[ui_CallType] [nvarchar](10) COLLATE Latin1_General_CI_AI NULL,
	[ac_DisputeStatus] [nvarchar](10) COLLATE Latin1_General_CI_AI NULL,
	[ac_DisputeResolvedOn] [datetime] NULL,
	[ac_IsInvoiced] [nvarchar](3) COLLATE Latin1_General_CI_AI NULL,
	[ac_InvoiceDate] [datetime] NULL,
	[Exclude] [bit] NULL,
 CONSTRAINT [PK_PhoneCalls] PRIMARY KEY CLUSTERED 
(
	[SessionIdTime] ASC,
	[SessionIdSeq] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCalls]') AND name = N'NonClusteredIndex-20130701-111641')
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20130701-111641] ON [dbo].[PhoneCalls] 
(
	[SourceUserUri] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCalls]') AND name = N'NonClusteredIndex-20130701-111947')
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20130701-111947] ON [dbo].[PhoneCalls] 
(
	[DestinationUserUri] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCalls]') AND name = N'NonClusteredIndex-20130701-112005')
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20130701-112005] ON [dbo].[PhoneCalls] 
(
	[DestinationNumberUri] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCalls]') AND name = N'NonClusteredIndex-20130701-112039')
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20130701-112039] ON [dbo].[PhoneCalls] 
(
	[marker_CallTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCalls]') AND name = N'NonClusteredIndex-20130701-112101')
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20130701-112101] ON [dbo].[PhoneCalls] 
(
	[marker_CallTo] ASC,
	[marker_CallToCountry] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCalls]') AND name = N'NonClusteredIndex-20130701-112119')
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20130701-112119] ON [dbo].[PhoneCalls] 
(
	[ui_CallType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[PhoneCalls]') AND name = N'NonClusteredIndex-20130701-112136')
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20130701-112136] ON [dbo].[PhoneCalls] 
(
	[ac_IsInvoiced] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhoneBook]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PhoneBook]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PhoneBook](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SipAccount] [nvarchar](240) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DestinationNumber] [nvarchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Type] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Name] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DestinationCountry] [nvarchar](3) COLLATE Latin1_General_CI_AI NULL,
 CONSTRAINT [PK_PhoneBook] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Persistence]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Persistence]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Persistence](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Module] [nvarchar](200) COLLATE Latin1_General_BIN NOT NULL,
	[Module_Key] [nvarchar](250) COLLATE Latin1_General_BIN NOT NULL,
	[Module_Value] [nvarchar](250) COLLATE Latin1_General_BIN NOT NULL,
 CONSTRAINT [PK_Defentions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[NumberingPlan]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NumberingPlan]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[NumberingPlan](
	[Dialing_prefix] [bigint] NOT NULL,
	[Country_Name] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[Two_Digits_country_code] [nvarchar](2) COLLATE Latin1_General_CI_AI NULL,
	[Three_Digits_Country_Code] [nvarchar](3) COLLATE Latin1_General_CI_AI NULL,
	[City] [varchar](255) COLLATE Latin1_General_CI_AI NULL,
	[Provider] [nvarchar](100) COLLATE Latin1_General_CI_AI NULL,
	[Type_Of_Service] [nvarchar](100) COLLATE Latin1_General_CI_AI NULL,
 CONSTRAINT [PK_NumberingPlan] PRIMARY KEY CLUSTERED 
(
	[Dialing_prefix] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[NumberingPlan]') AND name = N'Dialing_prefix-20130523-083555')
CREATE NONCLUSTERED INDEX [Dialing_prefix-20130523-083555] ON [dbo].[NumberingPlan] 
(
	[Dialing_prefix] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[NumberingPlan]') AND name = N'NonClusteredIndex-20130523-083624')
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20130523-083624] ON [dbo].[NumberingPlan] 
(
	[Dialing_prefix] ASC,
	[Three_Digits_Country_Code] ASC,
	[Type_Of_Service] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[NumberingPlan]') AND name = N'Three_Digits_Country_CodeIndex-20130523-083647')
CREATE NONCLUSTERED INDEX [Three_Digits_Country_CodeIndex-20130523-083647] ON [dbo].[NumberingPlan] 
(
	[Three_Digits_Country_Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MonitoringServersInfo]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MonitoringServersInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MonitoringServersInfo](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[instanceHostName] [nvarchar](100) COLLATE Latin1_General_BIN NOT NULL,
	[instanceName] [nvarchar](100) COLLATE Latin1_General_BIN NULL,
	[databaseName] [nvarchar](100) COLLATE Latin1_General_BIN NOT NULL,
	[userName] [nvarchar](100) COLLATE Latin1_General_BIN NOT NULL,
	[password] [nvarchar](100) COLLATE Latin1_General_BIN NOT NULL,
	[TelephonySolutionName] [nvarchar](100) COLLATE Latin1_General_BIN NOT NULL,
	[phoneCallsTable] [nvarchar](100) COLLATE Latin1_General_BIN NOT NULL,
	[description] [nvarchar](250) COLLATE Latin1_General_BIN NOT NULL,
	[created_at] [datetime] NULL,
 CONSTRAINT [PK_MonitoringServersInfo] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[MailTemplates]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MailTemplates]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MailTemplates](
	[TemplateID] [int] IDENTITY(1,1) NOT NULL,
	[TemplateSubject] [text] COLLATE Latin1_General_BIN NULL,
	[TemplateBody] [text] COLLATE Latin1_General_BIN NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[MailStatistics]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MailStatistics]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MailStatistics](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[EmailAddress] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[RecievedCount] [bigint] NULL,
	[RecievedSize] [bigint] NULL,
	[SentCount] [bigint] NULL,
	[SentSize] [bigint] NULL,
	[TimeStamp] [datetime] NOT NULL
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[TmpUsers]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TmpUsers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TmpUsers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SipAccount] [nvarchar](250) COLLATE Latin1_General_BIN NOT NULL,
 CONSTRAINT [PK_TmpUsers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Delegates]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Delegates]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Delegates](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SipAccount] [nvarchar](250) COLLATE Latin1_General_CI_AI NOT NULL,
	[DelegeeAccount] [nvarchar](250) COLLATE Latin1_General_CI_AI NOT NULL,
	[Description] [text] COLLATE Latin1_General_CI_AI NULL,
 CONSTRAINT [PK_Delegates] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[DIDs]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DIDs]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DIDs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[did] [nvarchar](100) COLLATE Latin1_General_BIN NOT NULL,
	[description] [nvarchar](250) COLLATE Latin1_General_BIN NULL,
 CONSTRAINT [PK_dids] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Currencies]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Currencies]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Currencies](
	[CountryName] [varchar](32) COLLATE Latin1_General_BIN NULL,
	[CurrencyName] [varchar](42) COLLATE Latin1_General_BIN NULL,
	[CurrencyISOName] [varchar](19) COLLATE Latin1_General_BIN NULL
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CallTypes]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CallTypes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CallTypes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CallType] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_CallTypes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[CallMarkerStatus]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CallMarkerStatus]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CallMarkerStatus](
	[markerId] [int] IDENTITY(1,1) NOT NULL,
	[phoneCallsTable] [nvarchar](100) COLLATE Latin1_General_BIN NOT NULL,
	[type] [nvarchar](100) COLLATE Latin1_General_BIN NOT NULL,
	[timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_CallMarkerStatus] PRIMARY KEY CLUSTERED 
(
	[markerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Announcements]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Announcements]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Announcements](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Announcement] [text] COLLATE Latin1_General_CI_AI NOT NULL,
	[Role] [nvarchar](50) COLLATE Latin1_General_CI_AI NULL,
	[AnnouncementDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Announcements] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ActiveDirectoryUsers]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActiveDirectoryUsers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ActiveDirectoryUsers](
	[AD_UserID] [int] NOT NULL,
	[SipAccount] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[AD_DisplayName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[AD_PhysicalDeliveryOfficeName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AD_Department] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_ActiveDirectoryUsers] PRIMARY KEY CLUSTERED 
(
	[SipAccount] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Gateways]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Gateways]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Gateways](
	[GatewayId] [int] IDENTITY(1,1) NOT NULL,
	[Gateway] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_Gateways] PRIMARY KEY CLUSTERED 
(
	[GatewayId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[GatewaysRates]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewaysRates]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewaysRates](
	[GatewaysRatesID] [int] IDENTITY(1,1) NOT NULL,
	[GatewayID] [int] NOT NULL,
	[RatesTableName] [nvarchar](256) COLLATE Latin1_General_BIN NULL,
	[StartingDate] [datetime] NULL,
	[EndingDate] [datetime] NULL,
	[ProviderName] [nvarchar](100) COLLATE Latin1_General_CI_AI NULL,
	[CurrencyCode] [nvarchar](3) COLLATE Latin1_General_BIN NULL,
 CONSTRAINT [PK_RatesPerGateways] PRIMARY KEY CLUSTERED 
(
	[GatewaysRatesID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[GatewaysDetails]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GatewaysDetails]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GatewaysDetails](
	[GatewayID] [int] NOT NULL,
	[SiteID] [int] NOT NULL,
	[PoolID] [int] NOT NULL,
	[Description] [nvarchar](256) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[DepartmentHeads]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DepartmentHeads]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DepartmentHeads](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[SipAccount] [nvarchar](256) COLLATE Latin1_General_BIN NOT NULL,
	[Department] [nvarchar](256) COLLATE Latin1_General_BIN NOT NULL,
	[SiteID] [int] NOT NULL,
 CONSTRAINT [PK_DepartmentHeads] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  View [dbo].[vw_Countries]    Script Date: 11/14/2013 13:35:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Countries]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_Countries]
AS
SELECT DISTINCT Country_Name AS Country, Three_Digits_Country_Code, Two_Digits_country_code
FROM            dbo.NumberingPlan
'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'vw_Countries', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "NumberingPlan"
            Begin Extent = 
               Top = 6
               Left = 306
               Bottom = 281
               Right = 541
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Countries'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'vw_Countries', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Countries'
GO
/****** Object:  UserDefinedFunction [dbo].[func_Rates_Per_Site]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[func_Rates_Per_Site]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================


CREATE FUNCTION [dbo].[func_Rates_Per_Site]
(	
	-- Add the parameters for the function here
	@RatesTableName nvarchar(250)
)
RETURNS TABLE 
AS
RETURN 
(
	select 
		Country_Name, 
		Two_Digits_country_code, 
		Three_Digits_Country_Code, 
		max(CASE WHEN Type_Of_Service=''fixedline'' then rate END) Fixedline,
		max(CASE WHEN Type_Of_Service=''gsm'' then rate END) GSM

	from
	(
		SELECT	DISTINCT
			numberingplan.Country_Name, 
			numberingplan.Two_Digits_country_code, 
			numberingplan.Three_Digits_Country_Code, 
			numberingplan.Type_Of_Service,
			fixedrate.rate as rate

		FROM  
			dbo.NumberingPlan as numberingplan
		
		LEFT JOIN
			dbo.[Rates_10.1.1.5_2013_04_02]  as fixedrate ON 
				numberingplan.Dialing_prefix = fixedrate.country_code_dialing_prefix 
		WHERE 
			numberingplan.Type_Of_Service=''gsm'' or
			numberingplan.Type_Of_Service=''fixedline''
	) src

	GROUP BY Country_Name,Two_Digits_country_code,Three_Digits_Country_Code
)
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fnc_phone2DialingPrefix]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnc_phone2DialingPrefix]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		W. Hadidi
-- Create date: 13/Mar/2013
-- Description:	Get a country code from phone number
-- =============================================
CREATE FUNCTION [dbo].[fnc_phone2DialingPrefix] 
(
	-- Add the parameters for the function here
	@PhoneNumber nvarchar(450)
)
RETURNS bigint
AS
BEGIN
	-- Declare the return variable here
	DECLARE @intFlag int
	DECLARE @returnId bigint
	Set @intFlag = 8

	-- Add the T-SQL statements to compute the return value here
	WHILE (@intFlag >= 1 )
		BEGIN
			IF @returnId is null
				BEGIN
					SET @returnId = (SELECT [Dialing_prefix] FROM [tBill].[dbo].[NumberingPlan] WHERE [Dialing_prefix] = left(right(@PhoneNumber,(Len(@PhoneNumber)-1)),@intFlag))
					SET @intFlag = @intFlag - 1
				END
			ELSE
				BREAK
		END

	-- Return the result of the function
	RETURN @returnId

END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fnc_GetCallType]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnc_GetCallType]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		W. Hadidi
-- Create date: 13/Mar/2013
-- Description:	Takes 2 phone and decide the type of the phone call
-- =============================================
CREATE FUNCTION [dbo].[fnc_GetCallType]
(
	-- Add the parameters for the function here
	@FromParsedNumber nvarchar(450),
	@ToParsedNumber nvarchar(450)
)
RETURNS nvarchar(450)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @FromCountry nvarchar(2),
			@ToCountry nvarchar(2),
			@FromCity nvarchar(250),
			@ToCity nvarchar(250),
			@FromType_Of_Service nvarchar(100),
			@ToType_Of_Service nvarchar(100),
			@results nvarchar(450)

	-- Add the T-SQL statements to compute the return value here
	SELECT @FromCountry=[Two_Digits_country_code] , @FromCity=[City] , @FromType_Of_Service=[Type_Of_Service] FROM [tBill].[dbo].[NumberingPlan] WHERE [Dialing_prefix] = @FromParsedNumber
	SELECT @ToCountry=[Two_Digits_country_code] , @ToCity=[City] , @ToType_Of_Service=[Type_Of_Service] FROM [tBill].[dbo].[NumberingPlan] WHERE [Dialing_prefix] = @ToParsedNumber

	IF @FromParsedNumber = @ToParsedNumber
			SET @results = ''Local''		
		ELSE
			BEGIN
				IF @FromCountry = @ToCountry
						BEGIN
							IF @ToType_Of_Service = ''gsm''
									SET @results = ''National - Mobile''
								ELSE
									SET @results = ''National - Fixedline''
						END
					ELSE
						BEGIN
							IF @ToType_Of_Service = ''gsm''
									SET @results = ''International - Mobile''
								ELSE
									SET @results = ''International - Fixedline''
						END
			END
	RETURN @results
END

' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fnc_DialingPrefix2Country]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnc_DialingPrefix2Country]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'-- =============================================
-- Author:		W. Hadidi
-- Create date: 18/May/2013
-- Description:	Convert a dailing prefix to a 3-
--				digits country code
-- =============================================
CREATE FUNCTION [dbo].[fnc_DialingPrefix2Country] 
(
	-- Add the parameters for the function here
	@Dialing_prefix [bigint]
)
RETURNS [nvarchar](3)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar[nvarchar](3)

	-- Add the T-SQL statements to compute the return value here
	SELECT @ResultVar=[Three_Digits_Country_Code]
	FROM [dbo].[NumberingPlan]
	WHERE Dialing_prefix = @Dialing_prefix

	-- Return the result of the function
	RETURN @ResultVar

END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_GatewaySummary_ForAll_Sites]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_GatewaySummary_ForAll_Sites]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_GatewaySummary_ForAll_Sites] 
( 

) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT TOP 100 PERCENT
		 [ToGateway] AS [ToGateway], 
		 YEAR(ResponseTime) AS [Year], 
		 MONTH(ResponseTime) AS [Month], 
		 SUM([Duration]) AS [CallsDuration], 
		 COUNT([SessionIdTime]) AS [CallsCount], 
		 SUM([marker_CallCost]) AS [CallsCost] 
	 FROM 
	 (
		 SELECT * FROM [PhoneCalls2010] 
		 WHERE 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [ToGateway] IS NOT NULL 
		 UNION ALL 

		 SELECT * FROM [PhoneCalls2013] 
		 WHERE 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [ToGateway] IS NOT NULL 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 [ToGateway], 
		 YEAR(ResponseTime), 
		 MONTH(ResponseTime) 
	 ORDER BY [ToGateway] ASC, YEAR( ResponseTime) ASC, MONTH(ResponseTime) ASC 
 
) ' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_DestinationsNumbers_ForUser]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_DestinationsNumbers_ForUser]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_DestinationsNumbers_ForUser] 
( 
	 @SipAccount  nvarchar(450), 
	 @Limits int 
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT TOP (@Limits) ISNULL([DestinationNumberUri], [DestinationUserUri]) AS PhoneNumber, 
		  [marker_CallToCountry] AS Country, 
		 SUM ([Duration]) AS CallsDuration, 
		 SUM ([marker_CallCost]) AS CallsCost, 
		 COUNT ([SessionIdTime]) AS CallsCount 
	 FROM 
	 (
		 SELECT * FROM [PhoneCalls2010] 
		 WHERE 
			 [SourceUserUri]=@SipAccount AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
		 UNION ALL 

		 SELECT * FROM [PhoneCalls2013] 
		 WHERE 
			 [SourceUserUri]=@SipAccount AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 ISNULL([DestinationNumberUri], [DestinationUserUri]), 
		 [marker_CallToCountry] 
	 ORDER BY CallsCount DESC 
) ' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_DestinationsNumbers_ForSiteDepartment]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_DestinationsNumbers_ForSiteDepartment]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_DestinationsNumbers_ForSiteDepartment] 
( 
	 @OfficeName  nvarchar(450), 
	 @DepartmentName nvarchar(450), 
 	 @Limits int 
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT TOP (@Limits) ISNULL([DestinationNumberUri], [DestinationUserUri]) AS PhoneNumber, 
		  [marker_CallToCountry] AS Country, 
		 SUM ([Duration]) AS CallsDuration, 
		 SUM ([marker_CallCost]) AS CallsCost, 
		 COUNT ([SessionIdTime]) AS CallsCount 
	 FROM 
	 (
		 SELECT * FROM [PhoneCalls2010] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 
			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [AD_Department]=@DepartmentName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
		 UNION ALL

		 SELECT * FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 
			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [AD_Department]=@DepartmentName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 ISNULL([DestinationNumberUri], [DestinationUserUri]), 
		 [marker_CallToCountry] 
	 ORDER BY CallsCount DESC 
) ' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_DestinationsNumbers_ForSite]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_DestinationsNumbers_ForSite]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_DestinationsNumbers_ForSite] 
( 
	 @OfficeName  nvarchar(450), 
	 @Limits int 
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT TOP (@Limits) ISNULL([DestinationNumberUri], [DestinationUserUri]) AS PhoneNumber, 
		  [marker_CallToCountry] AS Country, 
		 SUM ([Duration]) AS CallsDuration, 
		 SUM ([marker_CallCost]) AS CallsCost, 
		 COUNT ([SessionIdTime]) AS CallsCount 
	 FROM 
	 (
		 SELECT * FROM [PhoneCalls2010] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 
			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
		 UNION ALL

		 SELECT * FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 
			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 ISNULL([DestinationNumberUri], [DestinationUserUri]), 
		 [marker_CallToCountry] 
	 ORDER BY CallsCount DESC 
) ' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_DestinationCountries_ForUser]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_DestinationCountries_ForUser]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_DestinationCountries_ForUser] 
( 
	 @SipAccount  nvarchar(450), 
	 @Limits int 
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT TOP (@Limits) [Country_Name], 
		 SUM ([Duration]) AS CallsDuration, 
		 SUM ([marker_CallCost]) AS CallsCost, 
		 COUNT ([SessionIdTime]) AS CallsCount 
	 FROM 
	 (
		 SELECT * FROM [PhoneCalls2010] 
		 LEFT OUTER JOIN [NumberingPlan]  ON [PhoneCalls2010].[marker_CallTo] =   [NumberingPlan].[Dialing_prefix] 
		 WHERE 
			 [SourceUserUri]=@SipAccount AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
		 UNION ALL 

		 SELECT * FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [NumberingPlan]  ON [PhoneCalls2013].[marker_CallTo] =   [NumberingPlan].[Dialing_prefix] 
		 WHERE 
			 [SourceUserUri]=@SipAccount AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 [marker_CallToCountry], 
		 [Country_Name] 
	 ORDER BY CallsCost DESC 
) ' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_DestinationCountries_ForSiteDepartment]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_DestinationCountries_ForSiteDepartment]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_DestinationCountries_ForSiteDepartment] 
( 
	 @OfficeName  nvarchar(450), 
	 @DepartmentName nvarchar(450), 
 	 @Limits int 
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT TOP (@Limits) [Country_Name], 
		 SUM ([Duration]) AS CallsDuration, 
		 SUM ([marker_CallCost]) AS CallsCost, 
		 COUNT ([SessionIdTime]) AS CallsCount 
	 FROM 
	 (
		 SELECT * FROM [PhoneCalls2010] 
		 LEFT OUTER JOIN [NumberingPlan]  ON [PhoneCalls2010].[marker_CallTo] =   [NumberingPlan].[Dialing_prefix] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 
			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [AD_Department]=@DepartmentName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
		 UNION ALL

		 SELECT * FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [NumberingPlan]  ON [PhoneCalls2013].[marker_CallTo] =   [NumberingPlan].[Dialing_prefix] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 
			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [AD_Department]=@DepartmentName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 [marker_CallToCountry], 
		 [Country_Name] 
	 ORDER BY CallsCost DESC 
) ' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_DestinationCountries_ForSite]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_DestinationCountries_ForSite]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_DestinationCountries_ForSite] 
( 
	 @OfficeName  nvarchar(450), 
	 @Limits int 
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT TOP (@Limits) [Country_Name], 
		 SUM ([Duration]) AS CallsDuration, 
		 SUM ([marker_CallCost]) AS CallsCost, 
		 COUNT ([SessionIdTime]) AS CallsCount 
	 FROM 
	 (
		 SELECT * FROM [PhoneCalls2010] 
		 LEFT OUTER JOIN [NumberingPlan]  ON [PhoneCalls2010].[marker_CallTo] =   [NumberingPlan].[Dialing_prefix] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 
			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
		 UNION ALL

		 SELECT * FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [NumberingPlan]  ON [PhoneCalls2013].[marker_CallTo] =   [NumberingPlan].[Dialing_prefix] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 
			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 [marker_CallToCountry], 
		 [Country_Name] 
	 ORDER BY CallsCost DESC 
) ' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_ChargeableCalls_ForUser]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_ChargeableCalls_ForUser]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_ChargeableCalls_ForUser] 
( 
	 @SipAccount	nvarchar(450)
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT *,''PhoneCalls2010'' AS PhoneCallsTableName
	 FROM [PhoneCalls2010] 
	 WHERE [SourceUserUri]=@SipAccount AND [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
	 UNION ALL 
 	 SELECT *,''PhoneCalls2013'' AS PhoneCallsTableName
	 FROM [PhoneCalls2013] 
	 WHERE [SourceUserUri]=@SipAccount AND [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
	  
) ' 
END
GO
/****** Object:  Table [dbo].[UsersRoles]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UsersRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UsersRoles](
	[UsersRolesID] [int] IDENTITY(1,1) NOT NULL,
	[SipAccount] [nvarchar](256) COLLATE Latin1_General_CI_AI NOT NULL,
	[RoleID] [int] NOT NULL,
	[SiteID] [int] NULL,
	[PoolID] [int] NULL,
	[GatewayID] [int] NULL,
	[Notes] [nvarchar](256) COLLATE Latin1_General_CI_AI NULL,
 CONSTRAINT [PK_RolesPerUsers] PRIMARY KEY CLUSTERED 
(
	[UsersRolesID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  UserDefinedFunction [dbo].[TESTING]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TESTING]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
CREATE FUNCTION [dbo].[TESTING]
( 
	 @SipAccount	nvarchar(450)
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT *, ''PhoneCalls2010'' AS PhoneCallsTableName
	 FROM [PhoneCalls2010] 
	 WHERE [SourceUserUri]=@SipAccount AND [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
	 UNION ALL 
 	 SELECT *, ''PhoneCalls2013'' AS PhoneCallsTableName
	 FROM [PhoneCalls2013] 
	 WHERE [SourceUserUri]=@SipAccount AND [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
	  
) 
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_CallsSummary_ForUsers_PerGateway]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_CallsSummary_ForUsers_PerGateway]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_CallsSummary_ForUsers_PerGateway] 
( 
	 @Gateway	nvarchar(450)
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT TOP 100 PERCENT
		 [AD_UserID] AS [AD_UserID], 
		 [AD_DisplayName] AS [AD_DisplayName], 
		 [AD_Department] AS [AD_Department], 
		 [SourceUserUri] AS [SourceUserUri], 
		 YEAR(ResponseTime) AS [Year], 
		 MONTH(ResponseTime) AS [Month], 
		 (CAST(CAST(YEAR(ResponseTime) AS varchar) + ''/'' + CAST(MONTH(ResponseTime) AS varchar) + ''/'' +CAST(1 AS VARCHAR) AS DATETIME)) AS Date, 
		 SUM(CASE WHEN [ui_CallType] = ''Business'' THEN [Duration] END) AS [BusinessCallsDuration], 
		 SUM(CASE WHEN [ui_CallType] = ''Business'' THEN 1 END) AS [BusinessCallsCount], 
		 SUM(CASE WHEN [ui_CallType] = ''Business'' THEN [marker_CallCost] END) AS [BusinessCallsCost], 
		 SUM(CASE WHEN [ui_CallType] = ''Personal'' THEN [Duration] END) AS [PersonalCallsDuration], 
		 SUM(CASE WHEN [ui_CallType] = ''Personal'' THEN 1 END) AS [PersonalCallsCount], 
		 SUM(CASE WHEN [ui_CallType] = ''Personal'' THEN [marker_CallCost] END) AS [PersonalCallsCost], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN [Duration] END) AS [UnmarkedCallsDuration], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN 1 END) AS [UnmarkedCallsCount], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN [marker_CallCost] END) AS [UnmarkedCallsCost] 
	 FROM 
	 (
		 SELECT * FROM [PhoneCalls2010] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 			 [ToGateway]=@Gateway AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
		 UNION ALL

		 SELECT * FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 			 [ToGateway]=@Gateway AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 [SourceUserUri], 
		 [ToGateway], 
		 [AD_UserID], 
		 [AD_DisplayName]COLLATE SQL_Latin1_General_CP1_CI_AS , 
		 [AD_Department], 
		 YEAR(ResponseTime), 
		 MONTH(ResponseTime) 
	 ORDER BY YEAR( ResponseTime) ASC, MONTH(ResponseTime) ASC 
 
) ' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_CallsSummary_ForUser]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_CallsSummary_ForUser]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_CallsSummary_ForUser] 
( 
	 @SipAccount	nvarchar(450)
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT TOP 100 PERCENT
		 [SourceUserUri] AS [SourceUserUri], 
		 YEAR(ResponseTime) AS [Year], 
		 MONTH(ResponseTime) AS [Month], 
		 SUM(CASE WHEN [ui_CallType] = ''Business'' THEN [Duration] END) AS [BusinessCallsDuration], 
		 SUM(CASE WHEN [ui_CallType] = ''Business'' THEN 1 END) AS [BusinessCallsCount], 
		 SUM(CASE WHEN [ui_CallType] = ''Business'' THEN [marker_CallCost] END) AS [BusinessCallsCost], 
		 SUM(CASE WHEN [ui_CallType] = ''Personal'' THEN [Duration] END) AS [PersonalCallsDuration], 
		 SUM(CASE WHEN [ui_CallType] = ''Personal'' THEN 1 END) AS [PersonalCallsCount], 
		 SUM(CASE WHEN [ui_CallType] = ''Personal'' THEN [marker_CallCost] END) AS [PersonalCallsCost], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN [Duration] END) AS [UnmarkedCallsDuration], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN 1 END) AS [UnmarkedCallsCount], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN [marker_CallCost] END) AS [UnmarkedCallsCost] 
	 FROM 
	 (
		 SELECT * FROM [PhoneCalls2010] 
		 WHERE 			 [SourceUserUri]=@SipAccount AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
		 UNION ALL 

		 SELECT * FROM [PhoneCalls2013] 
		 WHERE 			 [SourceUserUri]=@SipAccount AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 [SourceUserUri], 
		 YEAR(ResponseTime), 
		 MONTH(ResponseTime) 
	 ORDER BY [SourceUserUri] ASC ,YEAR( ResponseTime) ASC, MONTH(ResponseTime) ASC 
 
) ' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_CallsSummary_ForSiteDepartment]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_CallsSummary_ForSiteDepartment]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_CallsSummary_ForSiteDepartment] 
( 
	 @OfficeName  nvarchar(450), 
	 @DepartmentName nvarchar(450) 
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT TOP 100 PERCENT
		 YEAR(ResponseTime) AS [Year], 
		 MONTH(ResponseTime) AS [Month], 
		 (CAST(CAST(YEAR(ResponseTime) AS varchar) + ''/'' + CAST(MONTH(ResponseTime) AS varchar) + ''/'' +CAST(1 AS VARCHAR) AS DATETIME)) AS Date, 
		 SUM(CASE WHEN [ui_CallType] = ''Business'' THEN [Duration] END) AS [BusinessCallsDuration], 
		 SUM(CASE WHEN [ui_CallType] = ''Business'' THEN 1 END) AS [BusinessCallsCount], 
		 SUM(CASE WHEN [ui_CallType] = ''Business'' THEN [marker_CallCost] END) AS [BusinessCallsCost], 
		 SUM(CASE WHEN [ui_CallType] = ''Personal'' THEN [Duration] END) AS [PersonalCallsDuration], 
		 SUM(CASE WHEN [ui_CallType] = ''Personal'' THEN 1 END) AS [PersonalCallsCount], 
		 SUM(CASE WHEN [ui_CallType] = ''Personal'' THEN [marker_CallCost] END) AS [PersonalCallsCost], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN [Duration] END) AS [UnmarkedCallsDuration], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN 1 END) AS [UnmarkedCallsCount], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN [marker_CallCost] END) AS [UnmarkedCallsCost] 
	 FROM 
	 (
		 SELECT * FROM [PhoneCalls2010] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [AD_Department]=@DepartmentName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
		 UNION ALL

		 SELECT * FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [AD_Department]=@DepartmentName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 YEAR(ResponseTime), 
		 MONTH(ResponseTime) 
	 ORDER BY YEAR( ResponseTime) ASC, MONTH(ResponseTime) ASC 
 
) ' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_ChargeableCalls_ForGateway]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_ChargeableCalls_ForGateway]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_ChargeableCalls_ForGateway] 
( 
	 @Gateway	nvarchar(450)
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT *,''PhoneCalls2010'' AS PhoneCallsTableName 
	 FROM [PhoneCalls2010] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
	 WHERE 
		 [ToGateway]=@Gateway AND 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
	 UNION ALL 
 	 SELECT *,''PhoneCalls2013'' AS PhoneCallsTableName 
	 FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
	 WHERE 
		 [ToGateway]=@Gateway AND 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) 
 
	  
) ' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_MailStatistics_PerSiteDepartment]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_MailStatistics_PerSiteDepartment]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_MailStatistics_PerSiteDepartment] 
( 
	 @OfficeName  nvarchar(450), 
	 @DepartmentName nvarchar(450), 
	 @FromDate datetime, 
 	 @ToDate dateTime 
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT TOP 100 PERCENT 
		 SUM ([RecievedCount]) AS RecievedCount, 
		 SUM ([RecievedSize]) AS RecievedSize, 
		 SUM ([SentCount]) AS SentCount, 
		 SUM ([SentSize]) AS SentSize 
	 FROM [MailStatistics] 
		 LEFT OUTER JOIN  [ActiveDirectoryUsers] ON [MailStatistics].[EmailAddress] = [ActiveDirectoryUsers].[SipAccount] 
	 WHERE 
		 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
		 [AD_Department]=@DepartmentName AND 
		 [TimeStamp] BETWEEN @FromDate and @ToDate 
  
) ' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_MailStatistics_ForUsers_PerSiteDepartment]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_MailStatistics_ForUsers_PerSiteDepartment]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_MailStatistics_ForUsers_PerSiteDepartment] 
( 
	 @OfficeName  nvarchar(450), 
	 @DepartmentName nvarchar(450), 
	 @FromDate datetime, 
 	 @ToDate dateTime 
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT TOP 100 PERCENT 
		 [MailStatistics].[EmailAddress], 
		 SUM ([RecievedCount]) AS RecievedCount, 
		 SUM ([RecievedSize]) AS RecievedSize, 
		 SUM ([SentCount]) AS SentCount, 
		 SUM ([SentSize]) AS SentSize 
	 FROM [MailStatistics] 
		 LEFT OUTER JOIN  [ActiveDirectoryUsers] ON [MailStatistics].[EmailAddress] = [ActiveDirectoryUsers].[SipAccount] 
	 WHERE 
		 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
		 [AD_Department]=@DepartmentName AND 
		 [TimeStamp] BETWEEN @FromDate and @ToDate 
 	 GROUP BY [MailStatistics].[EmailAddress] 
 
) ' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_GatewaySummary_PerUser]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_GatewaySummary_PerUser]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_GatewaySummary_PerUser] 
( 
	 @SipAccount	nvarchar(450)
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT TOP 100 PERCENT
		 [SourceUserUri] AS [SourceUserUri], 
		 [ToGateway] AS [ToGateway], 
		 YEAR(ResponseTime) AS [Year], 
		 MONTH(ResponseTime) AS [Month], 
		 SUM([Duration]) AS [CallsDuration], 
		 COUNT([SessionIdTime]) AS [CallsCount], 
		 SUM([marker_CallCost]) AS [CallsCost] 
	 FROM 
	 (
		 SELECT * FROM [PhoneCalls2010] 
		 WHERE 
			 [SourceUserUri]=@SipAccount AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [ToGateway] IS NOT NULL 
		 UNION ALL 

		 SELECT * FROM [PhoneCalls2013] 
		 WHERE 
			 [SourceUserUri]=@SipAccount AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [ToGateway] IS NOT NULL 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 [SourceUserUri], 
		 [ToGateway], 
		 YEAR(ResponseTime), 
		 MONTH(ResponseTime) 
	 ORDER BY [ToGateway] ASC, YEAR( ResponseTime) ASC, MONTH(ResponseTime) ASC 
 
) ' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_GatewaySummary_PerSiteDepartment]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_GatewaySummary_PerSiteDepartment]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_GatewaySummary_PerSiteDepartment] 
( 
	 @OfficeName  nvarchar(450), 
	 @DepartmentName nvarchar(450) 
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT TOP 100 PERCENT
		 [ToGateway] AS [ToGateway], 
		 YEAR(ResponseTime) AS [Year], 
		 MONTH(ResponseTime) AS [Month], 
		 SUM([Duration]) AS [CallsDuration], 
		 COUNT([SessionIdTime]) AS [CallsCount], 
		 SUM([marker_CallCost]) AS [CallsCost] 
	 FROM 
	 (
		 SELECT * FROM [PhoneCalls2010] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount]  
WHERE 
		 [AD_Department]=@DepartmentName AND 
		 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
		 [Exclude]=0 AND 
		 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) AND 
		 [ToGateway] IN 
		 ( 
			 SELECT [Gateway] 
			 FROM [GatewaysDetails] 
				 LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] 
				 LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] 
			 WHERE [SiteName]=@OfficeName 
		 ) 
		 UNION ALL 

		 SELECT * FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount]  
WHERE 
		 [AD_Department]=@DepartmentName AND 
		 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
		 [Exclude]=0 AND 
		 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) AND 
		 [ToGateway] IN 
		 ( 
			 SELECT [Gateway] 
			 FROM [GatewaysDetails] 
				 LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] 
				 LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] 
			 WHERE [SiteName]=@OfficeName 
		 ) 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 [ToGateway], 
		 YEAR(ResponseTime), 
		 MONTH(ResponseTime) 
	 ORDER BY [ToGateway] ASC, YEAR( ResponseTime) ASC, MONTH(ResponseTime) ASC 
 
) ' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_GatewaySummary_PerSite]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_GatewaySummary_PerSite]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_GatewaySummary_PerSite] 
( 
	 @OfficeName	nvarchar(450)
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT TOP 100 PERCENT
		 [ToGateway] AS [ToGateway], 
		 YEAR(ResponseTime) AS [Year], 
		 MONTH(ResponseTime) AS [Month], 
		 SUM([Duration]) AS [CallsDuration], 
		 COUNT([SessionIdTime]) AS [CallsCount], 
		 SUM([marker_CallCost]) AS [CallsCost] 
	 FROM 
	 (
		 SELECT * FROM [PhoneCalls2010] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount]  
WHERE 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
		 [Exclude]=0 AND 
		 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) AND 
		 [ToGateway] IN 
		 ( 
			 SELECT [Gateway] 
			 FROM [GatewaysDetails] 
				 LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] 
				 LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] 
			 WHERE [SiteName]=@OfficeName 
		 ) 
		 UNION ALL 

		 SELECT * FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount]  
WHERE 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
		 [Exclude]=0 AND 
		 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) AND 
		 [ToGateway] IN 
		 ( 
			 SELECT [Gateway] 
			 FROM [GatewaysDetails] 
				 LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] 
				 LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] 
			 WHERE [SiteName]=@OfficeName 
		 ) 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 [ToGateway], 
		 YEAR(ResponseTime), 
		 MONTH(ResponseTime) 
	 ORDER BY [ToGateway] ASC, YEAR( ResponseTime) ASC, MONTH(ResponseTime) ASC 
 
) ' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_CallsSummary_ForUsers_PerSite_PDF]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_CallsSummary_ForUsers_PerSite_PDF]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_CallsSummary_ForUsers_PerSite_PDF] 
( 
	 @OfficeName  nvarchar(450), 
	 @FromDate datetime, 
 	 @ToDate dateTime 
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT TOP 100 PERCENT
		 [AD_UserID] AS [AD_UserID], 
		 [AD_DisplayName] AS [AD_DisplayName], 
		 [AD_Department] AS [AD_Department], 
		 [SourceUserUri] AS [SourceUserUri], 
		 SUM(CASE WHEN [ui_CallType] = ''Business'' THEN [marker_CallCost] END) AS [BusinessCallsCost], 
		 SUM(CASE WHEN [ui_CallType] = ''Personal'' THEN [marker_CallCost] END) AS [PersonalCallsCost], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN [marker_CallCost] END) AS [UnmarkedCallsCost] 
	 FROM 
	 (
		 SELECT * FROM [PhoneCalls2010] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
WHERE 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
		 [Exclude]=0 AND 
		 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) AND 
		 ([SessionIdTime] BETWEEN @FromDate AND @ToDate) AND 
		 [ToGateway] IN 
		 ( 
			 SELECT [Gateway] 
			 FROM [GatewaysDetails] 
				 LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] 
				 LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] 
			 WHERE [SiteName]=@OfficeName 
		 ) 
		 UNION ALL

		 SELECT * FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
WHERE 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
		 [Exclude]=0 AND 
		 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) AND 
		 ([SessionIdTime] BETWEEN @FromDate AND @ToDate) AND 
		 [ToGateway] IN 
		 ( 
			 SELECT [Gateway] 
			 FROM [GatewaysDetails] 
				 LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] 
				 LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] 
			 WHERE [SiteName]=@OfficeName 
		 ) 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 [SourceUserUri], 
		 [AD_UserID], 
		 [AD_DisplayName], 
		 [AD_Department] 
	 ORDER BY SourceUserUri ASC 
 
) ' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_CallsSummary_ForUsers_PerSite]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_CallsSummary_ForUsers_PerSite]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_CallsSummary_ForUsers_PerSite] 
( 
	 @OfficeName	nvarchar(450)
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT TOP 100 PERCENT
		 [AD_UserID] AS [AD_UserID], 
		 [AD_DisplayName] AS [AD_DisplayName], 
		 [AD_Department] AS [AD_Department], 
		 [SourceUserUri] AS [SourceUserUri], 
		 YEAR(ResponseTime) AS [Year], 
		 MONTH(ResponseTime) AS [Month], 
		 (CAST(CAST(YEAR(ResponseTime) AS varchar) + ''/'' + CAST(MONTH(ResponseTime) AS varchar) + ''/'' +CAST(1 AS VARCHAR) AS DATETIME)) AS Date, 
		 SUM(CASE WHEN [ui_CallType] = ''Business'' THEN [Duration] END) AS [BusinessCallsDuration], 
		 SUM(CASE WHEN [ui_CallType] = ''Business'' THEN 1 END) AS [BusinessCallsCount], 
		 SUM(CASE WHEN [ui_CallType] = ''Business'' THEN [marker_CallCost] END) AS [BusinessCallsCost], 
		 SUM(CASE WHEN [ui_CallType] = ''Personal'' THEN [Duration] END) AS [PersonalCallsDuration], 
		 SUM(CASE WHEN [ui_CallType] = ''Personal'' THEN 1 END) AS [PersonalCallsCount], 
		 SUM(CASE WHEN [ui_CallType] = ''Personal'' THEN [marker_CallCost] END) AS [PersonalCallsCost], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN [Duration] END) AS [UnmarkedCallsDuration], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN 1 END) AS [UnmarkedCallsCount], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN [marker_CallCost] END) AS [UnmarkedCallsCost] 
	 FROM 
	 (
		 SELECT * FROM [PhoneCalls2010] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
WHERE 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
		 [Exclude]=0 AND 
		 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) AND 
		 [ToGateway] IN 
		 ( 
			 SELECT [Gateway] 
			 FROM [GatewaysDetails] 
				 LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] 
				 LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] 
			 WHERE [SiteName]=@OfficeName 
		 ) 
		 UNION ALL

		 SELECT * FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
WHERE 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
		 [Exclude]=0 AND 
		 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) AND 
		 [ToGateway] IN 
		 ( 
			 SELECT [Gateway] 
			 FROM [GatewaysDetails] 
				 LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] 
				 LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] 
			 WHERE [SiteName]=@OfficeName 
		 ) 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 [SourceUserUri], 
		 [AD_UserID], 
		 [AD_DisplayName], 
		 [AD_Department], 
		 YEAR(ResponseTime), 
		 MONTH(ResponseTime) 
	 ORDER BY YEAR( ResponseTime) ASC, MONTH(ResponseTime) ASC 
 
) ' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[Get_ChargeableCalls_ForSite]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Get_ChargeableCalls_ForSite]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[Get_ChargeableCalls_ForSite] 
( 
	 @OfficeName	nvarchar(450)
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT *,''PhoneCalls2010'' AS PhoneCallsTableName 
	 FROM [PhoneCalls2010] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
	 WHERE 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
		 [Exclude]=0 AND 
		 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) AND 
		 [ToGateway] IN 
		 ( 
			 SELECT [Gateway] 
			 FROM [GatewaysDetails] 
				 LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] 
				 LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] 
			 WHERE [SiteName]=@OfficeName 
		 ) 
	 UNION ALL 
 	 SELECT *,''PhoneCalls2013'' AS PhoneCallsTableName 
	 FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
	 WHERE 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
		 [Exclude]=0 AND 
		 ([ac_DisputeStatus]=''Rejected'' OR [ac_DisputeStatus] IS NULL ) AND 
		 [ToGateway] IN 
		 ( 
			 SELECT [Gateway] 
			 FROM [GatewaysDetails] 
				 LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] 
				 LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] 
			 WHERE [SiteName]=@OfficeName 
		 ) 
	  
) ' 
END
GO
/****** Object:  View [dbo].[View_SitesGateways]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[View_SitesGateways]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[View_SitesGateways]
AS
SELECT        dbo.GatewaysDetails.GatewayID, dbo.Gateways.Gateway, dbo.Sites.CountryCode, dbo.Sites.SiteName, dbo.GatewaysDetails.SiteID
FROM            dbo.Gateways RIGHT OUTER JOIN
                         dbo.GatewaysDetails ON dbo.Gateways.GatewayId = dbo.GatewaysDetails.GatewayID LEFT OUTER JOIN
                         dbo.Sites ON dbo.GatewaysDetails.SiteID = dbo.Sites.SiteID
'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'View_SitesGateways', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[12] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Gateways"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 133
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "GatewaysDetails"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 135
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Sites"
            Begin Extent = 
               Top = 4
               Left = 527
               Bottom = 133
               Right = 697
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1890
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_SitesGateways'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'View_SitesGateways', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_SitesGateways'
GO
/****** Object:  View [dbo].[vw_User_Role_Details]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_User_Role_Details]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[vw_User_Role_Details]
AS
SELECT        dbo.UsersRoles.UsersRolesID, dbo.UsersRoles.SipAccount, dbo.UsersRoles.RoleID, dbo.Roles.RoleName, dbo.Roles.RoleDescription, dbo.UsersRoles.SiteID, 
                         dbo.Sites.CountryCode, dbo.Sites.SiteName, dbo.UsersRoles.PoolID, dbo.Pools.PoolFQDN, dbo.UsersRoles.GatewayID, dbo.Gateways.Gateway, 
                         dbo.UsersRoles.Notes
FROM            dbo.UsersRoles LEFT OUTER JOIN
                         dbo.Gateways ON dbo.UsersRoles.GatewayID = dbo.Gateways.GatewayId LEFT OUTER JOIN
                         dbo.Sites ON dbo.UsersRoles.SiteID = dbo.Sites.SiteID LEFT OUTER JOIN
                         dbo.Pools ON dbo.UsersRoles.PoolID = dbo.Pools.PoolID LEFT OUTER JOIN
                         dbo.Roles ON dbo.UsersRoles.RoleID = dbo.Roles.RoleID
'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'vw_User_Role_Details', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "UsersRoles"
            Begin Extent = 
               Top = 0
               Left = 1
               Bottom = 217
               Right = 171
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Roles"
            Begin Extent = 
               Top = 141
               Left = 268
               Bottom = 253
               Right = 440
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Sites"
            Begin Extent = 
               Top = 114
               Left = 624
               Bottom = 226
               Right = 794
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Pools"
            Begin Extent = 
               Top = 0
               Left = 246
               Bottom = 101
               Right = 416
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Gateways"
            Begin Extent = 
               Top = 6
               Left = 786
               Bottom = 101
               Right = 956
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 14
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_User_Role_Details'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane2' , N'SCHEMA',N'dbo', N'VIEW',N'vw_User_Role_Details', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_User_Role_Details'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'vw_User_Role_Details', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_User_Role_Details'
GO
/****** Object:  View [dbo].[vw_Gateways_Full]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_Gateways_Full]'))
EXEC dbo.sp_executesql @statement = N'
CREATE VIEW [dbo].[vw_Gateways_Full]
AS
SELECT        dbo.Gateways.GatewayId, dbo.Gateways.Gateway, dbo.GatewaysDetails.SiteID, dbo.GatewaysDetails.PoolID, dbo.GatewaysDetails.Description, 
                         dbo.GatewaysRates.GatewaysRatesID, dbo.GatewaysRates.RatesTableName, dbo.GatewaysRates.StartingDate, dbo.GatewaysRates.EndingDate, 
                         dbo.GatewaysRates.ProviderName, dbo.GatewaysRates.CurrencyCode
FROM            dbo.GatewaysDetails RIGHT OUTER JOIN
                         dbo.GatewaysRates ON dbo.GatewaysDetails.GatewayID = dbo.GatewaysRates.GatewayID RIGHT OUTER JOIN
                         dbo.Gateways ON dbo.GatewaysDetails.GatewayID = dbo.Gateways.GatewayId

'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'vw_Gateways_Full', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[5] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "GatewaysDetails"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 135
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Gateways"
            Begin Extent = 
               Top = 147
               Left = 307
               Bottom = 242
               Right = 477
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "GatewaysRates"
            Begin Extent = 
               Top = 21
               Left = 716
               Bottom = 248
               Right = 894
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 12
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Gateways_Full'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'vw_Gateways_Full', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Gateways_Full'
GO
/****** Object:  UserDefinedFunction [dbo].[fnc_GetCallCost_DONOTUSE]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnc_GetCallCost_DONOTUSE]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		W. Hadidi
-- Modified :   S. Abu gahida
-- Create date: 14/3/2013
-- Description:	Get phone call cost  
-- =============================================
CREATE FUNCTION [dbo].[fnc_GetCallCost_DONOTUSE] 
(
	-- Add the parameters for the function here
	@ResponseTime datetime,
	@ToGateway nvarchar(256),
	@CallTo bigint,
	@Duration numeric(8,2)
)
RETURNS money
AS
BEGIN
	-- Declare the return variable here
	DECLARE @sql nvarchar(4000),
			@ratesTblName nvarchar(450),
			@costPerMin	money,
			@costPerMinOUT money,
			@ParmDefinition nvarchar(500),
			@results money

	-- Add the T-SQL statements to compute the return value here
	-- Gets rates table name
	Select @ratesTblName=RatesTableName 
	From [tBill].[dbo].[vw_Gateways_Full] 
	WHERE Gateway=@ToGateway AND @ResponseTime >= StartingDate AND @ResponseTime <= ISNULL(EndingDate,SYSDATETIME())
	

	-- Rates for DXB
	IF @ratesTblName = ''Rates_dxb-gw1.ae.ccg.local_2013_04_02''
			BEGIN
				Select @costPerMinOUT=rate 
				FROM dbo.[Rates_dxb-gw1.ae.ccg.local_2013_04_02]
				WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo)
				SET @results = @costPerMinOUT * CEILING(ISNULL(@Duration,0))
			END
	ELSE
			BEGIN
				-- Rates for MOA
				IF @ratesTblName = ''Rates_10.1.1.3_2013_04_02''
						BEGIN
							Select @costPerMinOUT=rate 
							FROM dbo.[Rates_10.1.1.3_2013_04_02]
							WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo)
							SET @results = @costPerMinOUT * CEILING(ISNULL(CEILING(@Duration/60),0))
						END
				ELSE
					BEGIN
						-- Rates for MOA
						IF @ratesTblName = ''Rates_10.1.1.4_2013_04_02''
								BEGIN
									Select @costPerMinOUT=rate 
									FROM dbo.[Rates_10.1.1.4_2013_04_02] 
									WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo)
									SET @results = @costPerMinOUT * CEILING(ISNULL(CEILING(@Duration/60),0))
								END
						ELSE
							BEGIN
								-- Rates for MOA
								IF @ratesTblName = ''Rates_10.1.1.5_2013_04_02''
									BEGIN
										Select @costPerMinOUT=rate 
										FROM dbo.[Rates_10.1.1.5_2013_04_02] 
										WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo)
										SET @results = @costPerMinOUT * CEILING(ISNULL(CEILING(@Duration/60),0))
									END
								ELSE
									BEGIN
										-- Rates for JHAP
										IF @ratesTblName = ''Rates_10.12.60.254_2013_04_02''
											BEGIN
												Select @costPerMinOUT=rate 
												FROM dbo.[Rates_10.12.60.254_2013_04_02] 
												WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo)
												SET @results = @costPerMinOUT * CEILING(ISNULL(CEILING(@Duration/60),0))
											END
										ELSE
											BEGIN
												-- Zero UK calls
												IF @ToGateway = ''10.9.0.42''
													BEGIN
														SET @results = 0
													END
												ELSE
													SET @results = null
											END
									END
							END
					END
			END
			-- Return the result of the function
				return @results
END
' 
END
GO
/****** Object:  UserDefinedFunction [dbo].[fnc_GetCallCost]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fnc_GetCallCost]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'
-- =============================================
-- Author:		W. Hadidi
-- Create date: 20/May/2013
-- Description:	Get phone call cost  
-- =============================================
CREATE FUNCTION [dbo].[fnc_GetCallCost] 
(
	-- Add the parameters for the function here
	@ResponseTime datetime,
	@ToGateway nvarchar(256),
	@CallTo bigint,
	@Duration numeric(8,2)
)
RETURNS money
AS
BEGIN
	-- Declare the return variable here
	DECLARE @sql nvarchar(4000),
			@caseResult int,
			@ratesTblName nvarchar(450),
			@costPerMin	money,
			@costPerMinOUT money,
			@ParmDefinition nvarchar(500),
			@results money

	-- Add the T-SQL statements to compute the return value here
	-- Gets rates table name
	Select distinct @ratesTblName=RatesTableName 
	From [tBill].[dbo].[vw_Gateways_Full] 
	WHERE Gateway=@ToGateway AND @ResponseTime >= StartingDate AND @ResponseTime <= ISNULL(EndingDate,SYSDATETIME())
	
	
	
	-- Cases 
	
	SET @caseResult =
			Case @ratesTblName
				WHEN N''Rates_dxb-gw1.ae.ccg.local_2013_04_02'' THEN 1
				WHEN N''Rates_10.1.0.12_2013_04_02'' THEN 2
				WHEN N''Rates_10.1.1.4_2013_04_02'' THEN 3
				WHEN N''Rates_10.1.1.5_2013_04_02'' THEN 4
				WHEN N''Rates_10.12.60.254_2013_04_02'' THEN 5
				WHEN N''Rates_10.1.1.3_2013_04_02'' THEN 6
				WHEN N''Rates_10.12.10.12_2013_04_02'' THEN 7
				WHEN N''Rates_10.12.0.12_2013_04_02'' THEN 8
				WHEN N''Rates_10.1.1.57_2013_04_02'' THEN 9
				WHEN N''Rates_192.168.10.1_2013_04_02'' THEN 10
				WHEN N''Rates_10.81.0.3_2013_04_02'' THEN 11
				WHEN N''Rates_10.1.5.51_2013_04_02'' THEN 12
				WHEN N''Rates_gr-moa-ccm-02.moa.gr.ccc_2013_04_02'' THEN 13
				WHEN N''Rates_10.12.60.253_2013_04_02'' THEN 14
				WHEN N''Rates_lync.skype-lync.akadns.net_2013_04_02'' THEN 15 
				ELSE 99
			END
				
	IF @caseResult = 1   
		BEGIN Select @costPerMinOUT=rate FROM dbo.[Rates_dxb-gw1.ae.ccg.local_2013_04_02] WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo) END 
	IF @caseResult = 2   
		BEGIN Select @costPerMinOUT=rate FROM dbo.[Rates_10.1.0.12_2013_04_02] WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo) END 
	IF @caseResult = 3   
		BEGIN Select @costPerMinOUT=rate FROM dbo.[Rates_10.1.1.4_2013_04_02] WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo) END 
	IF @caseResult = 4
		BEGIN Select @costPerMinOUT=rate FROM dbo.[Rates_10.1.1.5_2013_04_02] WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo) END 
	IF @caseResult = 5
		BEGIN Select @costPerMinOUT=rate FROM dbo.[Rates_10.12.60.254_2013_04_02] WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo) END 		
	IF @caseResult = 6
		BEGIN Select @costPerMinOUT=rate FROM dbo.[Rates_10.1.1.3_2013_04_02] WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo) END 
	IF @caseResult = 7
		BEGIN Select @costPerMinOUT=rate FROM dbo.[Rates_10.12.10.12_2013_04_02] WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo) END 
	IF @caseResult = 8
		BEGIN Select @costPerMinOUT=rate FROM dbo.[Rates_10.12.0.12_2013_04_02] WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo) END 
	IF @caseResult = 9
		BEGIN Select @costPerMinOUT=rate FROM dbo.[Rates_10.1.1.57_2013_04_02] WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo) END 
	IF @caseResult = 10
		BEGIN Select @costPerMinOUT=rate FROM dbo.[Rates_192.168.10.1_2013_04_02] WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo) END 
	IF @caseResult = 11
		BEGIN Select @costPerMinOUT=rate FROM dbo.[Rates_10.81.0.3_2013_04_02] WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo) END 
	IF @caseResult = 12
		BEGIN Select @costPerMinOUT=rate FROM dbo.[Rates_10.1.5.51_2013_04_02] WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo) END
	IF @caseResult = 13
		BEGIN Select @costPerMinOUT=rate FROM dbo.[Rates_gr-moa-ccm-02.moa.gr.ccc_2013_04_02] WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo) END
	IF @caseResult = 14
		BEGIN Select @costPerMinOUT=rate FROM dbo.[Rates_10.12.60.253_2013_04_02] WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo) END
	IF @caseResult = 15
		BEGIN Select @costPerMinOUT=rate FROM dbo.[Rates_lync.skype-lync.akadns.net_2013_04_02] WHERE country_code_dialing_prefix = Convert(nvarchar(MAX), @CallTo) END
	IF @caseResult = 99
		BEGIN SET @costPerMinOUT = NULL END
	
	SET @results = @costPerMinOUT * CEILING(ISNULL(CEILING(@Duration/60),0))
	
	-- RASO EXCEPTIONS
	IF   @ToGateway = ''dxb-gw1.ae.ccg.local'' and @Duration <= 15
	BEGIN
		-- SET @results = @costPerMinOUT * CEILING(ISNULL(CEILING((@Duration-15)/60),0))
		SET @results = 0
	END

	IF @Duration <= 0
	BEGIN
	SET @results =0
	END

	-- Zero UK calls
	IF @ToGateway = ''10.9.0.42''
	BEGIN
	SET @results = 0
	END

	-- Return the result of the function
	return @results
END	' 
END
GO
/****** Object:  DdlTrigger [tr_MStran_alterschemaonly]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE parent_class_desc = 'DATABASE' AND name = N'tr_MStran_alterschemaonly')
EXECUTE dbo.sp_executesql N'
create trigger [tr_MStran_alterschemaonly] on database for ALTER_FUNCTION, ALTER_PROCEDURE as 

							set ANSI_NULLS ON
							set ANSI_PADDING ON
							set ANSI_WARNINGS ON
							set ARITHABORT ON
							set CONCAT_NULL_YIELDS_NULL ON
							set NUMERIC_ROUNDABORT OFF
							set QUOTED_IDENTIFIER ON

							declare @EventData xml
							set @EventData=EventData()    
							exec sys.sp_MStran_ddlrepl @EventData, 3'
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
DISABLE TRIGGER [tr_MStran_alterschemaonly] ON DATABASE
GO
/****** Object:  DdlTrigger [tr_MStran_altertable]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE parent_class_desc = 'DATABASE' AND name = N'tr_MStran_altertable')
EXECUTE dbo.sp_executesql N'
create trigger [tr_MStran_altertable] on database for ALTER_TABLE as 

							set ANSI_NULLS ON
							set ANSI_PADDING ON
							set ANSI_WARNINGS ON
							set ARITHABORT ON
							set CONCAT_NULL_YIELDS_NULL ON
							set NUMERIC_ROUNDABORT OFF
							set QUOTED_IDENTIFIER ON

							declare @EventData xml
							set @EventData=EventData()    
							exec sys.sp_MStran_ddlrepl @EventData, 1'
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
DISABLE TRIGGER [tr_MStran_altertable] ON DATABASE
GO
/****** Object:  DdlTrigger [tr_MStran_altertrigger]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE parent_class_desc = 'DATABASE' AND name = N'tr_MStran_altertrigger')
EXECUTE dbo.sp_executesql N'
create trigger [tr_MStran_altertrigger] on database for ALTER_TRIGGER as 

							set ANSI_NULLS ON
							set ANSI_PADDING ON
							set ANSI_WARNINGS ON
							set ARITHABORT ON
							set CONCAT_NULL_YIELDS_NULL ON
							set NUMERIC_ROUNDABORT OFF
							set QUOTED_IDENTIFIER ON

							declare @EventData xml
							set @EventData=EventData()    
							exec sys.sp_MStran_ddlrepl @EventData, 4'
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
DISABLE TRIGGER [tr_MStran_altertrigger] ON DATABASE
GO
/****** Object:  DdlTrigger [tr_MStran_alterview]    Script Date: 11/14/2013 13:35:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE parent_class_desc = 'DATABASE' AND name = N'tr_MStran_alterview')
EXECUTE dbo.sp_executesql N'
create trigger [tr_MStran_alterview] on database for ALTER_VIEW as 

							set ANSI_NULLS ON
							set ANSI_PADDING ON
							set ANSI_WARNINGS ON
							set ARITHABORT ON
							set CONCAT_NULL_YIELDS_NULL ON
							set NUMERIC_ROUNDABORT OFF
							set QUOTED_IDENTIFIER ON

							declare @EventData xml
							set @EventData=EventData()    
							exec sys.sp_MStran_ddlrepl @EventData, 2'
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
DISABLE TRIGGER [tr_MStran_alterview] ON DATABASE
GO
/****** Object:  Default [DF__PhoneCall2013__ac_Is__05A3D694]    Script Date: 11/14/2013 13:35:06 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__PhoneCall2013__ac_Is__05A3D694]') AND parent_object_id = OBJECT_ID(N'[dbo].[PhoneCalls2013]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__PhoneCall2013__ac_Is__05A3D694]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PhoneCalls2013] ADD  CONSTRAINT [DF__PhoneCall2013__ac_Is__05A3D694]  DEFAULT (N'NO') FOR [ac_IsInvoiced]
END


End
GO
/****** Object:  Default [DF__PhoneCall__Exclu__035179CE]    Script Date: 11/14/2013 13:35:06 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__PhoneCall__Exclu__035179CE]') AND parent_object_id = OBJECT_ID(N'[dbo].[PhoneCalls2013]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__PhoneCall__Exclu__035179CE]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PhoneCalls2013] ADD  CONSTRAINT [DF__PhoneCall__Exclu__035179CE]  DEFAULT ((0)) FOR [Exclude]
END


End
GO
/****** Object:  Default [DF__PhoneCall2010__ac_Is__05A3D694]    Script Date: 11/14/2013 13:35:06 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__PhoneCall2010__ac_Is__05A3D694]') AND parent_object_id = OBJECT_ID(N'[dbo].[PhoneCalls2010]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__PhoneCall2010__ac_Is__05A3D694]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PhoneCalls2010] ADD  CONSTRAINT [DF__PhoneCall2010__ac_Is__05A3D694]  DEFAULT (N'NO') FOR [ac_IsInvoiced]
END


End
GO
/****** Object:  Default [DF__PhoneCall__Exclu__7BB05806]    Script Date: 11/14/2013 13:35:06 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__PhoneCall__Exclu__7BB05806]') AND parent_object_id = OBJECT_ID(N'[dbo].[PhoneCalls2010]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__PhoneCall__Exclu__7BB05806]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PhoneCalls2010] ADD  CONSTRAINT [DF__PhoneCall__Exclu__7BB05806]  DEFAULT ((0)) FOR [Exclude]
END


End
GO
/****** Object:  Default [DF__PhoneCall__ac_Is__05A3D694]    Script Date: 11/14/2013 13:35:06 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__PhoneCall__ac_Is__05A3D694]') AND parent_object_id = OBJECT_ID(N'[dbo].[PhoneCalls]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__PhoneCall__ac_Is__05A3D694]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PhoneCalls] ADD  CONSTRAINT [DF__PhoneCall__ac_Is__05A3D694]  DEFAULT (N'NO') FOR [ac_IsInvoiced]
END


End
GO
/****** Object:  Default [DF__PhoneCall__Exclu__54CB950F]    Script Date: 11/14/2013 13:35:06 ******/
IF Not EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'[dbo].[DF__PhoneCall__Exclu__54CB950F]') AND parent_object_id = OBJECT_ID(N'[dbo].[PhoneCalls]'))
Begin
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__PhoneCall__Exclu__54CB950F]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[PhoneCalls] ADD  CONSTRAINT [DF__PhoneCall__Exclu__54CB950F]  DEFAULT ((0)) FOR [Exclude]
END


End
GO
/****** Object:  ForeignKey [FK_GatewaysDetails_Pools]    Script Date: 11/14/2013 13:35:06 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewaysDetails_Pools]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewaysDetails]'))
ALTER TABLE [dbo].[GatewaysDetails]  WITH CHECK ADD  CONSTRAINT [FK_GatewaysDetails_Pools] FOREIGN KEY([PoolID])
REFERENCES [dbo].[Pools] ([PoolID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewaysDetails_Pools]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewaysDetails]'))
ALTER TABLE [dbo].[GatewaysDetails] CHECK CONSTRAINT [FK_GatewaysDetails_Pools]
GO
/****** Object:  ForeignKey [FK_GatewaysDetails_Sites]    Script Date: 11/14/2013 13:35:06 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewaysDetails_Sites]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewaysDetails]'))
ALTER TABLE [dbo].[GatewaysDetails]  WITH CHECK ADD  CONSTRAINT [FK_GatewaysDetails_Sites] FOREIGN KEY([SiteID])
REFERENCES [dbo].[Sites] ([SiteID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GatewaysDetails_Sites]') AND parent_object_id = OBJECT_ID(N'[dbo].[GatewaysDetails]'))
ALTER TABLE [dbo].[GatewaysDetails] CHECK CONSTRAINT [FK_GatewaysDetails_Sites]
GO
/****** Object:  ForeignKey [FK_dbo.DepartmentHeads_dbo.Sites_officeId]    Script Date: 11/14/2013 13:35:06 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.DepartmentHeads_dbo.Sites_officeId]') AND parent_object_id = OBJECT_ID(N'[dbo].[DepartmentHeads]'))
ALTER TABLE [dbo].[DepartmentHeads]  WITH CHECK ADD  CONSTRAINT [FK_dbo.DepartmentHeads_dbo.Sites_officeId] FOREIGN KEY([SiteID])
REFERENCES [dbo].[Sites] ([SiteID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_dbo.DepartmentHeads_dbo.Sites_officeId]') AND parent_object_id = OBJECT_ID(N'[dbo].[DepartmentHeads]'))
ALTER TABLE [dbo].[DepartmentHeads] CHECK CONSTRAINT [FK_dbo.DepartmentHeads_dbo.Sites_officeId]
GO
/****** Object:  ForeignKey [FK_RolesPerUsers_Sites]    Script Date: 11/14/2013 13:35:07 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RolesPerUsers_Sites]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersRoles]'))
ALTER TABLE [dbo].[UsersRoles]  WITH CHECK ADD  CONSTRAINT [FK_RolesPerUsers_Sites] FOREIGN KEY([SiteID])
REFERENCES [dbo].[Sites] ([SiteID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RolesPerUsers_Sites]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersRoles]'))
ALTER TABLE [dbo].[UsersRoles] CHECK CONSTRAINT [FK_RolesPerUsers_Sites]
GO
/****** Object:  ForeignKey [FK_UsersRoles_Pools]    Script Date: 11/14/2013 13:35:07 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersRoles_Pools]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersRoles]'))
ALTER TABLE [dbo].[UsersRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsersRoles_Pools] FOREIGN KEY([PoolID])
REFERENCES [dbo].[Pools] ([PoolID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersRoles_Pools]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersRoles]'))
ALTER TABLE [dbo].[UsersRoles] CHECK CONSTRAINT [FK_UsersRoles_Pools]
GO
/****** Object:  DdlTrigger [tr_MStran_alterschemaonly]    Script Date: 11/14/2013 13:35:07 ******/
Enable Trigger [tr_MStran_alterschemaonly] ON Database
GO
/****** Object:  DdlTrigger [tr_MStran_altertable]    Script Date: 11/14/2013 13:35:07 ******/
Enable Trigger [tr_MStran_altertable] ON Database
GO
/****** Object:  DdlTrigger [tr_MStran_altertrigger]    Script Date: 11/14/2013 13:35:07 ******/
Enable Trigger [tr_MStran_altertrigger] ON Database
GO
/****** Object:  DdlTrigger [tr_MStran_alterview]    Script Date: 11/14/2013 13:35:07 ******/
Enable Trigger [tr_MStran_alterview] ON Database
GO
