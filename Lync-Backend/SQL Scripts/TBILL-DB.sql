USE [master]
GO
/****** Object:  Database [tBill]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE DATABASE [tBill] ON  PRIMARY 
( NAME = N'tBill', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\tBill.mdf' , SIZE = 9008128KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'tBill_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\tBill_log.ldf' , SIZE = 12016128KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 COLLATE Latin1_General_BIN
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
/****** Object:  User [mailsvc]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE USER [mailsvc] FOR LOGIN [mailsvc] WITH DEFAULT_SCHEMA=[dbo]
GO

/****** Object:  UserDefinedTableType [dbo].[RatesTableType]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE TYPE [dbo].[RatesTableType] AS TABLE(
	[country_code_dialing_prefix] [bigint] NOT NULL,
	[rate] [decimal](18, 4) NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[country_code_dialing_prefix] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  Table [dbo].[ActiveDirectoryUsers]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActiveDirectoryUsers](
	[AD_UserID] [int] NOT NULL,
	[SipAccount] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[AD_DisplayName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[AD_PhysicalDeliveryOfficeName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AD_Department] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AD_TelephoneNumber] [nvarchar](50) COLLATE Latin1_General_BIN NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[UpdatedByAD] [bit] NULL,
 CONSTRAINT [PK_ActiveDirectoryUsers] PRIMARY KEY CLUSTERED 
(
	[SipAccount] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Announcements]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Announcements](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Announcement] [text] COLLATE Latin1_General_CI_AI NOT NULL,
	[Role] [nvarchar](50) COLLATE Latin1_General_CI_AI NULL,
	[AnnouncementDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Announcements] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CallMarkerStatus]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CallMarkerStatus](
	[markerId] [int] IDENTITY(1,1) NOT NULL,
	[phoneCallsTable] [nvarchar](100) COLLATE Latin1_General_BIN NOT NULL,
	[type] [nvarchar](100) COLLATE Latin1_General_BIN NOT NULL,
	[timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_CallMarkerStatus] PRIMARY KEY CLUSTERED 
(
	[markerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CallTypes]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CallTypes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CallType] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_CallTypes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Countries]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[CountryName] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CountryCodeISO2] [nvarchar](2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CountryCodeISO3] [nvarchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CurrencyName] [nvarchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[CurrencyISOName] [nvarchar](19) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[CountryCodeISO3] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DIDs]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DIDs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DID] [nvarchar](100) COLLATE Latin1_General_BIN NOT NULL,
	[Description] [nvarchar](250) COLLATE Latin1_General_BIN NULL,
 CONSTRAINT [PK_dids] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DelegateTypes]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DelegateTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DelegeeType] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Description] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_DelegateTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Departments]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentName] [nvarchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Description] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Gateways]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Gateways](
	[GatewayId] [int] IDENTITY(1,1) NOT NULL,
	[Gateway] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_Gateways] PRIMARY KEY CLUSTERED 
(
	[GatewayId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GatewaysDetails]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GatewaysDetails](
	[GatewayID] [int] NOT NULL,
	[SiteID] [int] NOT NULL,
	[PoolID] [int] NOT NULL,
	[Description] [nvarchar](256) COLLATE Latin1_General_CI_AI NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GatewaysRates]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MailStatistics]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MailStatistics](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[EmailAddress] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[RecievedCount] [bigint] NULL,
	[RecievedSize] [bigint] NULL,
	[SentCount] [bigint] NULL,
	[SentSize] [bigint] NULL,
	[TimeStamp] [datetime] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MailTemplates]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MailTemplates](
	[TemplateID] [int] IDENTITY(1,1) NOT NULL,
	[TemplateSubject] [text] COLLATE Latin1_General_BIN NULL,
	[TemplateBody] [text] COLLATE Latin1_General_BIN NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MonitoringServersInfo]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NumberingPlan]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Persistence]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Persistence](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Module] [nvarchar](200) COLLATE Latin1_General_BIN NOT NULL,
	[Module_Key] [nvarchar](250) COLLATE Latin1_General_BIN NOT NULL,
	[Module_Value] [nvarchar](250) COLLATE Latin1_General_BIN NOT NULL,
 CONSTRAINT [PK_Defentions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PhoneBook]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhoneBook](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SipAccount] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[DestinationNumber] [nvarchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Type] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Name] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[DestinationCountry] [nvarchar](3) COLLATE Latin1_General_CI_AI NULL,
 CONSTRAINT [PK_PhoneBook] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PhoneCalls2010]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
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
	[ReferredBy] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ChargingParty] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
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
	[ui_AssignedByUser] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ui_AssignedOn] [datetime] NULL,
	[ui_CallType] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ac_DisputeStatus] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ac_DisputeResolvedOn] [datetime] NULL,
	[ac_IsInvoiced] [nvarchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ac_InvoiceDate] [datetime] NULL,
	[Exclude] [bit] NULL,
	[CalleeURI] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ui_AssignedToUser] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_PhoneCalls2010] PRIMARY KEY CLUSTERED 
(
	[SessionIdTime] ASC,
	[SessionIdSeq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PhoneCalls2013]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
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
	[ReferredBy] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ChargingParty] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
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
	[ui_AssignedByUser] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ui_AssignedOn] [datetime] NULL,
	[ui_CallType] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ac_DisputeStatus] [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ac_DisputeResolvedOn] [datetime] NULL,
	[ac_IsInvoiced] [nvarchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ac_InvoiceDate] [datetime] NULL,
	[Exclude] [bit] NULL,
	[CalleeURI] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ui_AssignedToUser] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_PhoneCalls2013] PRIMARY KEY CLUSTERED 
(
	[SessionIdTime] ASC,
	[SessionIdSeq] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PhoneCallsExceptions]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PhoneCallsExceptions](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserUri] [nvarchar](450) COLLATE Latin1_General_CI_AI NULL,
	[Number] [varchar](100) COLLATE Latin1_General_BIN NULL,
	[Description] [nvarchar](450) COLLATE Latin1_General_CI_AI NULL,
 CONSTRAINT [PK_Vodafone Business Subscribers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Pools]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pools](
	[PoolID] [int] IDENTITY(1,1) NOT NULL,
	[PoolFQDN] [nvarchar](256) COLLATE Latin1_General_BIN NOT NULL,
 CONSTRAINT [PK_Pools] PRIMARY KEY CLUSTERED 
(
	[PoolID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Rates_x.x.x.x_2013_04_02]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rates_x.x.x.x_2013_04_02](
	[country_code_dialing_prefix] [bigint] NOT NULL,
	[rate] [decimal](18, 4) NOT NULL,
 CONSTRAINT [PK_Rates_10.1.0.12_2013_04_02] PRIMARY KEY NONCLUSTERED 
(
	[country_code_dialing_prefix] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Roles]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleName] [nvarchar](100) COLLATE Latin1_General_CI_AI NOT NULL,
	[RoleDescription] [nvarchar](100) COLLATE Latin1_General_CI_AI NULL,
	[RoleID] [int] NOT NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Roles_Delegates]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles_Delegates](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SipAccount] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SiteID] [int] NULL,
	[DepartmentID] [int] NULL,
	[DelegeeType] [int] NOT NULL,
	[Delegee] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Description] [nvarchar](450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_Delegates] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Roles_DepartmentsHeads]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles_DepartmentsHeads](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentID] [int] NOT NULL,
	[SipAccount] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_DepartmentsHeads] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Roles_System]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles_System](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SipAccount] [nvarchar](256) COLLATE Latin1_General_CI_AI NOT NULL,
	[RoleID] [int] NOT NULL,
	[SiteID] [int] NULL,
	[Description] [nvarchar](256) COLLATE Latin1_General_CI_AI NULL,
 CONSTRAINT [PK_RolesPerUsers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sites]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sites](
	[SiteID] [int] IDENTITY(1,1) NOT NULL,
	[CountryCode] [nvarchar](3) COLLATE Latin1_General_CI_AI NULL,
	[SiteName] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Description] [nvarchar](256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_Sites] PRIMARY KEY CLUSTERED 
(
	[SiteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sites_Departments]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sites_Departments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SiteID] [int] NOT NULL,
	[DepartmentID] [int] NOT NULL,
 CONSTRAINT [PK_Sites_Departments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  UserDefinedFunction [dbo].[Get_CallsSummary_ForSiteDepartment]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_CallsSummary_ForSiteDepartment] 
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
		 (CAST(CAST(YEAR(ResponseTime) AS varchar) + '/' + CAST(MONTH(ResponseTime) AS varchar) + '/' +CAST(1 AS VARCHAR) AS DATETIME)) AS Date, 
		 SUM(CASE WHEN [ui_CallType] = 'Business' THEN [Duration] END) AS [BusinessCallsDuration], 
		 COUNT(CASE WHEN [ui_CallType] = 'Business' THEN 1 END) AS [BusinessCallsCount], 
		 SUM(CASE WHEN [ui_CallType] = 'Business' THEN [marker_CallCost] END) AS [BusinessCallsCost], 
		 SUM(CASE WHEN [ui_CallType] = 'Personal' THEN [Duration] END) AS [PersonalCallsDuration], 
		 COUNT(CASE WHEN [ui_CallType] = 'Personal' THEN 1 END) AS [PersonalCallsCount], 
		 SUM(CASE WHEN [ui_CallType] = 'Personal' THEN [marker_CallCost] END) AS [PersonalCallsCost], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN [Duration] END) AS [UnmarkedCallsDuration], 
		 COUNT(CASE WHEN [ui_CallType] IS NULL THEN 1 END) AS [UnmarkedCallsCount], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN [marker_CallCost] END) AS [UnmarkedCallsCost] 
	 FROM 
	 (
		 SELECT * FROM [PhoneCalls2010] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [AD_Department]=@DepartmentName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 [ToGateway] IS NOT NULL AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) 
 
		 UNION ALL

		 SELECT * FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [AD_Department]=@DepartmentName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 [ToGateway] IS NOT NULL AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) 
 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 YEAR(ResponseTime), 
		 MONTH(ResponseTime) 
	 ORDER BY YEAR( ResponseTime) ASC, MONTH(ResponseTime) ASC 
 
) 
GO
/****** Object:  UserDefinedFunction [dbo].[Get_CallsSummary_ForUser]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_CallsSummary_ForUser] 
( 
	 @SipAccount  nvarchar(450), 
	 @FromDate datetime, 
 	 @ToDate dateTime 
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT TOP 100 PERCENT
		 YEAR(ResponseTime) AS [Year], 
		 MONTH(ResponseTime) AS [Month], 
		 (CAST(CAST(YEAR(ResponseTime) AS varchar) + '/' + CAST(MONTH(ResponseTime) AS varchar) + '/' +CAST(1 AS VARCHAR) AS DATETIME)) AS Date, 
		 [ChargingParty] AS [ChargingParty], 
		 SUM(CASE WHEN [ui_CallType] = 'Business' THEN [Duration] END) AS [BusinessCallsDuration], 
		 COUNT(CASE WHEN [ui_CallType] = 'Business' THEN 1 END) AS [BusinessCallsCount], 
		 SUM(CASE WHEN [ui_CallType] = 'Business' THEN [marker_CallCost] END) AS [BusinessCallsCost], 
		 SUM(CASE WHEN [ui_CallType] = 'Personal' THEN [Duration] END) AS [PersonalCallsDuration], 
		 COUNT(CASE WHEN [ui_CallType] = 'Personal' THEN 1 END) AS [PersonalCallsCount], 
		 SUM(CASE WHEN [ui_CallType] = 'Personal' THEN [marker_CallCost] END) AS [PersonalCallsCost], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN [Duration] END) AS [UnmarkedCallsDuration], 
		 COUNT (CASE WHEN [ui_CallType] IS NULL THEN 1 END) AS [UnmarkedCallsCount], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN [marker_CallCost] END) AS [UnmarkedCallsCost] 
	 FROM 
	 (
		 SELECT * FROM [PhoneCalls2010] 
		 WHERE 
			 [ChargingParty]=@SipAccount AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 ([SessionIdTime] BETWEEN @FromDate AND @ToDate) AND 
			 [Exclude]=0 AND 
			 [ToGateway] IS NOT NULL AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) 
 
		 UNION ALL 

		 SELECT * FROM [PhoneCalls2013] 
		 WHERE 
			 [ChargingParty]=@SipAccount AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 ([SessionIdTime] BETWEEN @FromDate AND @ToDate) AND 
			 [Exclude]=0 AND 
			 [ToGateway] IS NOT NULL AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) 
 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 YEAR(ResponseTime), 
		 MONTH(ResponseTime), 
		 [ChargingParty] 
	 ORDER BY [ChargingParty] ASC 
 
) 
GO
/****** Object:  UserDefinedFunction [dbo].[Get_CallsSummary_ForUsers_PerGateway]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_CallsSummary_ForUsers_PerGateway] 
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
		 (CAST(CAST(YEAR(ResponseTime) AS varchar) + '/' + CAST(MONTH(ResponseTime) AS varchar) + '/' +CAST(1 AS VARCHAR) AS DATETIME)) AS Date, 
		 SUM(CASE WHEN [ui_CallType] = 'Business' THEN [Duration] END) AS [BusinessCallsDuration], 
		 SUM(CASE WHEN [ui_CallType] = 'Business' THEN 1 END) AS [BusinessCallsCount], 
		 SUM(CASE WHEN [ui_CallType] = 'Business' THEN [marker_CallCost] END) AS [BusinessCallsCost], 
		 SUM(CASE WHEN [ui_CallType] = 'Personal' THEN [Duration] END) AS [PersonalCallsDuration], 
		 SUM(CASE WHEN [ui_CallType] = 'Personal' THEN 1 END) AS [PersonalCallsCount], 
		 SUM(CASE WHEN [ui_CallType] = 'Personal' THEN [marker_CallCost] END) AS [PersonalCallsCost], 
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
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) 
 
		 UNION ALL

		 SELECT * FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 			 [ToGateway]=@Gateway AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) 
 
		  
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
 
) 
GO
/****** Object:  UserDefinedFunction [dbo].[Get_CallsSummary_ForUsers_PerSite]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_CallsSummary_ForUsers_PerSite] 
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
		 YEAR(ResponseTime) AS [Year], 
		 MONTH(ResponseTime) AS [Month], 
		 (CAST(CAST(YEAR(ResponseTime) AS varchar) + '/' + CAST(MONTH(ResponseTime) AS varchar) + '/' +CAST(1 AS VARCHAR) AS DATETIME)) AS Date, 
		 [AD_UserID] AS [AD_UserID], 
		 [AD_DisplayName] AS [AD_DisplayName], 
		 [AD_Department] AS [AD_Department], 
		 [ChargingParty] AS [ChargingParty], 
		 [ac_IsInvoiced] AS [ac_IsInvoiced], 
		 SUM(CASE WHEN [ui_CallType] = 'Business' THEN [Duration] END) AS [BusinessCallsDuration], 
		 COUNT(CASE WHEN [ui_CallType] = 'Business' THEN 1 END) AS [BusinessCallsCount], 
		 SUM(CASE WHEN [ui_CallType] = 'Business' THEN [marker_CallCost] END) AS [BusinessCallsCost], 
		 SUM(CASE WHEN [ui_CallType] = 'Personal' THEN [Duration] END) AS [PersonalCallsDuration], 
		 COUNT(CASE WHEN [ui_CallType] = 'Personal' THEN 1 END) AS [PersonalCallsCount], 
		 SUM(CASE WHEN [ui_CallType] = 'Personal' THEN [marker_CallCost] END) AS [PersonalCallsCost], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN [Duration] END) AS [UnmarkedCallsDuration], 
		 COUNT(CASE WHEN [ui_CallType] IS NULL THEN 1 END) AS [UnmarkedCallsCount], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN [marker_CallCost] END) AS [UnmarkedCallsCost] 
	 FROM 
	 (
		 SELECT * FROM [PhoneCalls2010] 
			 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([SessionIdTime] BETWEEN @FromDate AND @ToDate) AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
			 [ToGateway] IS NOT NULL AND 
			 [ToGateway] IN 
			 ( 
				 SELECT [Gateway] 
				 FROM [GatewaysDetails] 
					 LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] 
					 LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] 
				 WHERE 
 					 [SiteName]=@OfficeName 
			 ) 
		 UNION ALL

		 SELECT * FROM [PhoneCalls2013] 
			 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([SessionIdTime] BETWEEN @FromDate AND @ToDate) AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
			 [ToGateway] IS NOT NULL AND 
			 [ToGateway] IN 
			 ( 
				 SELECT [Gateway] 
				 FROM [GatewaysDetails] 
					 LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] 
					 LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] 
				 WHERE 
 					 [SiteName]=@OfficeName 
			 ) 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 YEAR(ResponseTime), 
		 MONTH(ResponseTime), 
		 [ChargingParty], 
		 [AD_UserID], 
		 [AD_DisplayName], 
		 [AD_Department], 
		 [ac_IsInvoiced] 
	 ORDER BY [ChargingParty] ASC 
 
) 
GO
/****** Object:  UserDefinedFunction [dbo].[Get_CallsSummary_ForUsers_PerSite_PDF]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_CallsSummary_ForUsers_PerSite_PDF] 
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
		 [ChargingParty] AS [ChargingParty], 
		 [ac_IsInvoiced] AS [ac_IsInvoiced], 
		 SUM(CASE WHEN [ui_CallType] = 'Business' THEN [marker_CallCost] END) AS [BusinessCallsCost], 
		 SUM(CASE WHEN [ui_CallType] = 'Personal' THEN [marker_CallCost] END) AS [PersonalCallsCost], 
		 SUM(CASE WHEN [ui_CallType] IS NULL THEN [marker_CallCost] END) AS [UnmarkedCallsCost] 
	 FROM 
	 (
		 SELECT * FROM [PhoneCalls2010] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] 
WHERE 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
		 [Exclude]=0 AND 
		 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
		 ([SessionIdTime] BETWEEN @FromDate AND @ToDate) AND 
		 [ToGateway] IS NOT NULL AND 
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
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] 
WHERE 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
		 [Exclude]=0 AND 
		 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
		 ([SessionIdTime] BETWEEN @FromDate AND @ToDate) AND 
		 [ToGateway] IS NOT NULL AND 
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
		 [ChargingParty], 
		 [AD_UserID], 
		 [AD_DisplayName], 
		 [AD_Department], 
		 [ac_IsInvoiced] 
	 ORDER BY ChargingParty ASC 
 
) 
GO
/****** Object:  UserDefinedFunction [dbo].[Get_ChargeableCalls_ForGateway]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_ChargeableCalls_ForGateway] 
( 
	 @Gateway	nvarchar(450)
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT *,'PhoneCalls2010' AS PhoneCallsTableName 
	 FROM [PhoneCalls2010] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
	 WHERE 
		 [ToGateway]=@Gateway AND 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) 
 
	 UNION ALL 
 	 SELECT *,'PhoneCalls2013' AS PhoneCallsTableName 
	 FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[SourceUserUri] =   [ActiveDirectoryUsers].[SipAccount] 
	 WHERE 
		 [ToGateway]=@Gateway AND 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) 
 
	  
) 
GO
/****** Object:  UserDefinedFunction [dbo].[Get_ChargeableCalls_ForSite]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_ChargeableCalls_ForSite] 
( 
	 @OfficeName	nvarchar(450)
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT *,'PhoneCalls2010' AS PhoneCallsTableName 
	 FROM [PhoneCalls2010] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] 
	 WHERE 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
		 [Exclude]=0 AND 
		 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
		 [ToGateway] IS NOT NULL AND 
		 [ToGateway] IN 
		 ( 
			 SELECT [Gateway] 
			 FROM [GatewaysDetails] 
				 LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] 
				 LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] 
			 WHERE [SiteName]=@OfficeName 
		 ) 
	 UNION ALL 
 	 SELECT *,'PhoneCalls2013' AS PhoneCallsTableName 
	 FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] 
	 WHERE 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
		 [Exclude]=0 AND 
		 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
		 [ToGateway] IS NOT NULL AND 
		 [ToGateway] IN 
		 ( 
			 SELECT [Gateway] 
			 FROM [GatewaysDetails] 
				 LEFT JOIN [Gateways] ON [Gateways].[GatewayId] = [GatewaysDetails].[GatewayID] 
				 LEFT JOIN [Sites] ON [Sites].[SiteID] = [GatewaysDetails].[SiteID] 
			 WHERE [SiteName]=@OfficeName 
		 ) 
	  
) 
GO
/****** Object:  UserDefinedFunction [dbo].[Get_ChargeableCalls_ForUser]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_ChargeableCalls_ForUser] 
( 
	 @SipAccount	nvarchar(450)
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT *,'PhoneCalls2010' AS PhoneCallsTableName
	 FROM [PhoneCalls2010] 
	 WHERE ([ChargingParty]=@SipAccount  OR [ui_AssignedToUser]=@SipAccount ) AND [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 [ToGateway] IS NOT NULL AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) 
 
	 UNION ALL 
 	 SELECT *,'PhoneCalls2013' AS PhoneCallsTableName
	 FROM [PhoneCalls2013] 
	 WHERE ([ChargingParty]=@SipAccount  OR [ui_AssignedToUser]=@SipAccount ) AND [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [Exclude]=0 AND 
			 [ToGateway] IS NOT NULL AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) 
 
	  
) 
GO
/****** Object:  UserDefinedFunction [dbo].[Get_DestinationCountries_ForSite]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_DestinationCountries_ForSite] 
( 
	 @OfficeName  nvarchar(450), 
	 @FromDate datetime, 
 	 @ToDate dateTime, 
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
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 
			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
			 [SessionIdTime] BETWEEN @FromDate AND @ToDate  
 
		 UNION ALL

		 SELECT * FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [NumberingPlan]  ON [PhoneCalls2013].[marker_CallTo] =   [NumberingPlan].[Dialing_prefix] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 
			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
			 [SessionIdTime] BETWEEN @FromDate AND @ToDate  
 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 [marker_CallToCountry], 
		 [Country_Name] 
	 ORDER BY CallsCount DESC 
) 
GO
/****** Object:  UserDefinedFunction [dbo].[Get_DestinationCountries_ForSiteDepartment]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_DestinationCountries_ForSiteDepartment] 
( 
	 @OfficeName  nvarchar(450), 
	 @DepartmentName nvarchar(450), 
	 @FromDate datetime, 
 	 @ToDate dateTime, 
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
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 
			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [AD_Department]=@DepartmentName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
			 [SessionIdTime] BETWEEN @FromDate AND @ToDate  
 
		 UNION ALL

		 SELECT * FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [NumberingPlan]  ON [PhoneCalls2013].[marker_CallTo] =   [NumberingPlan].[Dialing_prefix] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 
			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [AD_Department]=@DepartmentName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
			 [SessionIdTime] BETWEEN @FromDate AND @ToDate  
 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 [marker_CallToCountry], 
		 [Country_Name] 
	 ORDER BY CallsCount DESC 
) 
GO
/****** Object:  UserDefinedFunction [dbo].[Get_DestinationCountries_ForUser]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_DestinationCountries_ForUser] 
( 
	 @SipAccount  nvarchar(450), 
	 @FromDate datetime, 
 	 @ToDate dateTime, 
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
			 [ChargingParty]=@SipAccount AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
			 [SessionIdTime] BETWEEN @FromDate AND @ToDate  
 
		 UNION ALL 

		 SELECT * FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [NumberingPlan]  ON [PhoneCalls2013].[marker_CallTo] =   [NumberingPlan].[Dialing_prefix] 
		 WHERE 
			 [ChargingParty]=@SipAccount AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
			 [SessionIdTime] BETWEEN @FromDate AND @ToDate  
 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 [marker_CallToCountry], 
		 [Country_Name] 
	 ORDER BY CallsCount DESC 
) 
GO
/****** Object:  UserDefinedFunction [dbo].[Get_DestinationsNumbers_ForSite]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_DestinationsNumbers_ForSite] 
( 
	 @OfficeName  nvarchar(450), 
	 @FromDate datetime, 
 	 @ToDate dateTime, 
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
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 
			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
			 [SessionIdTime] BETWEEN @FromDate AND @ToDate  
 
		 UNION ALL

		 SELECT * FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 
			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
			 [SessionIdTime] BETWEEN @FromDate AND @ToDate  
 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 ISNULL([DestinationNumberUri], [DestinationUserUri]), 
		 [marker_CallToCountry] 
	 ORDER BY CallsCount DESC 
) 
GO
/****** Object:  UserDefinedFunction [dbo].[Get_DestinationsNumbers_ForSiteDepartment]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_DestinationsNumbers_ForSiteDepartment] 
( 
	 @OfficeName  nvarchar(450), 
	 @DepartmentName nvarchar(450), 
	 @FromDate datetime, 
 	 @ToDate dateTime, 
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
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 
			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [AD_Department]=@DepartmentName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
			 [SessionIdTime] BETWEEN @FromDate AND @ToDate  
 
		 UNION ALL

		 SELECT * FROM [PhoneCalls2013] 
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount] 
		 WHERE 
			 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
			 [AD_Department]=@DepartmentName AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
			 [SessionIdTime] BETWEEN @FromDate AND @ToDate  
 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 ISNULL([DestinationNumberUri], [DestinationUserUri]), 
		 [marker_CallToCountry] 
	 ORDER BY CallsCount DESC 
) 
GO
/****** Object:  UserDefinedFunction [dbo].[Get_DestinationsNumbers_ForUser]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_DestinationsNumbers_ForUser] 
( 
	 @SipAccount  nvarchar(450), 
	 @FromDate datetime, 
 	 @ToDate dateTime, 
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
			 [ChargingParty]=@SipAccount AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
			 [SessionIdTime] BETWEEN @FromDate AND @ToDate  
 
		 UNION ALL 

		 SELECT * FROM [PhoneCalls2013] 
		 WHERE 
			 [ChargingParty]=@SipAccount AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
			 [SessionIdTime] BETWEEN @FromDate AND @ToDate  
 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 ISNULL([DestinationNumberUri], [DestinationUserUri]), 
		 [marker_CallToCountry] 
	 ORDER BY CallsCount DESC 
) 
GO
/****** Object:  UserDefinedFunction [dbo].[Get_GatewaySummary_ForAll_Sites]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_GatewaySummary_ForAll_Sites] 
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
 
) 
GO
/****** Object:  UserDefinedFunction [dbo].[Get_GatewaySummary_PerSite]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_GatewaySummary_PerSite] 
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
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount]  
WHERE 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
		 [Exclude]=0 AND 
		 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
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
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount]  
WHERE 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
		 [Exclude]=0 AND 
		 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
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
 
) 
GO
/****** Object:  UserDefinedFunction [dbo].[Get_GatewaySummary_PerSiteDepartment]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_GatewaySummary_PerSiteDepartment] 
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
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2010].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount]  
WHERE 
		 [AD_Department]=@DepartmentName AND 
		 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
		 [Exclude]=0 AND 
		 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
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
		 LEFT OUTER JOIN [ActiveDirectoryUsers]  ON [PhoneCalls2013].[ChargingParty] =   [ActiveDirectoryUsers].[SipAccount]  
WHERE 
		 [AD_Department]=@DepartmentName AND 
		 [AD_PhysicalDeliveryOfficeName]=@OfficeName AND 
		 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
		 [Exclude]=0 AND 
		 ([ac_DisputeStatus]='Rejected' OR [ac_DisputeStatus] IS NULL ) AND 
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
 
) 
GO
/****** Object:  UserDefinedFunction [dbo].[Get_GatewaySummary_PerUser]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_GatewaySummary_PerUser] 
( 
	 @SipAccount	nvarchar(450)
) 
RETURNS TABLE 
AS 
RETURN 
(
	 SELECT TOP 100 PERCENT
		 [ChargingParty] AS [ChargingParty], 
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
			 [ChargingParty]=@SipAccount AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [ToGateway] IS NOT NULL 
		 UNION ALL 

		 SELECT * FROM [PhoneCalls2013] 
		 WHERE 
			 [ChargingParty]=@SipAccount AND 
			 [marker_CallTypeID] in (1,2,3,4,5,21,22) AND 
			 [ToGateway] IS NOT NULL 
		  
	) AS [UserPhoneCallsStats] 
	 GROUP BY 
		 [ChargingParty], 
		 [ToGateway], 
		 YEAR(ResponseTime), 
		 MONTH(ResponseTime) 
	 ORDER BY [ToGateway] ASC, YEAR( ResponseTime) ASC, MONTH(ResponseTime) ASC 
 
) 
GO
/****** Object:  UserDefinedFunction [dbo].[Get_MailStatistics_ForUsers_PerSiteDepartment]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_MailStatistics_ForUsers_PerSiteDepartment] 
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
 
) 
GO
/****** Object:  UserDefinedFunction [dbo].[Get_MailStatistics_PerSiteDepartment]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Get_MailStatistics_PerSiteDepartment] 
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
  
) 
GO
/****** Object:  UserDefinedFunction [dbo].[func_Rates_Per_Site]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
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
		max(CASE WHEN Type_Of_Service='fixedline' then rate END) Fixedline,
		max(CASE WHEN Type_Of_Service='gsm' then rate END) GSM

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
			numberingplan.Type_Of_Service='gsm' or
			numberingplan.Type_Of_Service='fixedline'
	) src

	GROUP BY Country_Name,Two_Digits_country_code,Three_Digits_Country_Code
)

GO
/****** Object:  View [dbo].[View_SitesGateways]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_SitesGateways]
AS
SELECT        dbo.GatewaysDetails.GatewayID, dbo.Gateways.Gateway, dbo.Sites.CountryCode, dbo.Sites.SiteName, dbo.GatewaysDetails.SiteID
FROM            dbo.Gateways RIGHT OUTER JOIN
                         dbo.GatewaysDetails ON dbo.Gateways.GatewayId = dbo.GatewaysDetails.GatewayID LEFT OUTER JOIN
                         dbo.Sites ON dbo.GatewaysDetails.SiteID = dbo.Sites.SiteID

GO
/****** Object:  View [dbo].[vw_Countries]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Countries]
AS
SELECT DISTINCT Country_Name AS Country, Three_Digits_Country_Code, Two_Digits_country_code
FROM            dbo.NumberingPlan

GO
/****** Object:  View [dbo].[vw_Gateways_Full]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_Gateways_Full]
AS
SELECT        dbo.Gateways.GatewayId, dbo.Gateways.Gateway, dbo.GatewaysDetails.SiteID, dbo.GatewaysDetails.PoolID, dbo.GatewaysDetails.Description, 
                         dbo.GatewaysRates.GatewaysRatesID, dbo.GatewaysRates.RatesTableName, dbo.GatewaysRates.StartingDate, dbo.GatewaysRates.EndingDate, 
                         dbo.GatewaysRates.ProviderName, dbo.GatewaysRates.CurrencyCode
FROM            dbo.GatewaysDetails RIGHT OUTER JOIN
                         dbo.GatewaysRates ON dbo.GatewaysDetails.GatewayID = dbo.GatewaysRates.GatewayID RIGHT OUTER JOIN
                         dbo.Gateways ON dbo.GatewaysDetails.GatewayID = dbo.Gateways.GatewayId


GO
/****** Object:  View [dbo].[vw_User_Role_Details]    Script Date: 1/24/2014 1:15:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_User_Role_Details]
AS
SELECT        dbo.UsersRoles.UsersRolesID, dbo.UsersRoles.SipAccount, dbo.UsersRoles.RoleID, dbo.Roles.RoleName, dbo.Roles.RoleDescription, dbo.UsersRoles.SiteID, 
                         dbo.Sites.CountryCode, dbo.Sites.SiteName, dbo.UsersRoles.PoolID, dbo.Pools.PoolFQDN, dbo.UsersRoles.GatewayID, dbo.Gateways.Gateway, 
                         dbo.UsersRoles.Notes
FROM            dbo.UsersRoles LEFT OUTER JOIN
                         dbo.Gateways ON dbo.UsersRoles.GatewayID = dbo.Gateways.GatewayId LEFT OUTER JOIN
                         dbo.Sites ON dbo.UsersRoles.SiteID = dbo.Sites.SiteID LEFT OUTER JOIN
                         dbo.Pools ON dbo.UsersRoles.PoolID = dbo.Pools.PoolID LEFT OUTER JOIN
                         dbo.Roles ON dbo.UsersRoles.RoleID = dbo.Roles.RoleID

GO
/****** Object:  Index [ClusteredIndex-20130523-100923]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE UNIQUE CLUSTERED INDEX [ClusteredIndex-20130523-100923] ON [dbo].[Rates_10.1.0.12_2013_04_02]
(
	[country_code_dialing_prefix] ASC,
	[rate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ClusteredIndex-20130523-101050]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE UNIQUE CLUSTERED INDEX [ClusteredIndex-20130523-101050] ON [dbo].[Rates_10.1.1.3_2013_04_02]
(
	[country_code_dialing_prefix] ASC,
	[rate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ClusteredIndex-20130523-101106]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE UNIQUE CLUSTERED INDEX [ClusteredIndex-20130523-101106] ON [dbo].[Rates_10.1.1.4_2013_04_02]
(
	[country_code_dialing_prefix] ASC,
	[rate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [ClusteredIndex-20130523-101124]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE UNIQUE CLUSTERED INDEX [ClusteredIndex-20130523-101124] ON [dbo].[Rates_10.1.1.5_2013_04_02]
(
	[country_code_dialing_prefix] ASC,
	[rate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Dialing_prefix-20130523-083555]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE NONCLUSTERED INDEX [Dialing_prefix-20130523-083555] ON [dbo].[NumberingPlan]
(
	[Dialing_prefix] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-20130523-083624]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20130523-083624] ON [dbo].[NumberingPlan]
(
	[Dialing_prefix] ASC,
	[Three_Digits_Country_Code] ASC,
	[Type_Of_Service] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [Three_Digits_Country_CodeIndex-20130523-083647]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE NONCLUSTERED INDEX [Three_Digits_Country_CodeIndex-20130523-083647] ON [dbo].[NumberingPlan]
(
	[Three_Digits_Country_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-20131115-105030]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131115-105030] ON [dbo].[PhoneCalls2010]
(
	[ChargingParty] ASC
)
INCLUDE ( 	[SessionIdTime],
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
	[ReferredBy],
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
	[ui_AssignedByUser],
	[ui_AssignedOn],
	[ui_CallType],
	[ac_DisputeStatus],
	[ac_DisputeResolvedOn],
	[ac_IsInvoiced],
	[ac_InvoiceDate],
	[Exclude]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-20131115-105100]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131115-105100] ON [dbo].[PhoneCalls2010]
(
	[marker_CallTypeID] ASC
)
INCLUDE ( 	[SessionIdTime],
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
	[ReferredBy],
	[ChargingParty],
	[Duration],
	[marker_CallFrom],
	[marker_CallTo],
	[marker_CallToCountry],
	[marker_CallCost],
	[marker_CallType],
	[marker_TimeStamp],
	[ui_MarkedOn],
	[ui_UpdatedByUser],
	[ui_AssignedByUser],
	[ui_AssignedOn],
	[ui_CallType],
	[ac_DisputeStatus],
	[ac_DisputeResolvedOn],
	[ac_IsInvoiced],
	[ac_InvoiceDate],
	[Exclude]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-20131115-105116]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131115-105116] ON [dbo].[PhoneCalls2010]
(
	[ToGateway] ASC
)
INCLUDE ( 	[SessionIdTime],
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
	[ReferredBy],
	[ChargingParty],
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
	[ui_AssignedByUser],
	[ui_AssignedOn],
	[ui_CallType],
	[ac_DisputeStatus],
	[ac_DisputeResolvedOn],
	[ac_IsInvoiced],
	[ac_InvoiceDate],
	[Exclude]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-20131115-105440]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131115-105440] ON [dbo].[PhoneCalls2010]
(
	[SourceUserUri] ASC
)
INCLUDE ( 	[SessionIdTime],
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
	[ReferredBy],
	[ChargingParty],
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
	[ui_AssignedByUser],
	[ui_AssignedOn],
	[ui_CallType],
	[ac_DisputeStatus],
	[ac_DisputeResolvedOn],
	[ac_IsInvoiced],
	[ac_InvoiceDate],
	[Exclude]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-20131130-131722]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131130-131722] ON [dbo].[PhoneCalls2010]
(
	[ui_AssignedToUser] ASC
)
INCLUDE ( 	[SessionIdTime],
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
	[ReferredBy],
	[ChargingParty],
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
	[ui_AssignedByUser],
	[ui_AssignedOn],
	[ui_CallType],
	[ac_DisputeStatus],
	[ac_DisputeResolvedOn],
	[ac_IsInvoiced],
	[ac_InvoiceDate],
	[Exclude],
	[CalleeURI]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-20140108-104019]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20140108-104019] ON [dbo].[PhoneCalls2010]
(
	[ui_CallType] ASC
)
INCLUDE ( 	[SessionIdTime],
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
	[ReferredBy],
	[ChargingParty],
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
	[ui_AssignedByUser],
	[ui_AssignedOn],
	[ac_DisputeStatus],
	[ac_DisputeResolvedOn],
	[ac_IsInvoiced],
	[ac_InvoiceDate],
	[Exclude],
	[CalleeURI],
	[ui_AssignedToUser]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-20140108-104227]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20140108-104227] ON [dbo].[PhoneCalls2010]
(
	[ac_IsInvoiced] ASC
)
INCLUDE ( 	[SessionIdTime],
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
	[ReferredBy],
	[ChargingParty],
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
	[ui_AssignedByUser],
	[ui_AssignedOn],
	[ui_CallType],
	[ac_DisputeStatus],
	[ac_DisputeResolvedOn],
	[ac_InvoiceDate],
	[Exclude],
	[CalleeURI],
	[ui_AssignedToUser]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-20131115-104328]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131115-104328] ON [dbo].[PhoneCalls2013]
(
	[SourceUserUri] ASC
)
INCLUDE ( 	[SessionIdTime],
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
	[ReferredBy],
	[ChargingParty],
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
	[ui_AssignedByUser],
	[ui_AssignedOn],
	[ui_CallType],
	[ac_DisputeStatus],
	[ac_DisputeResolvedOn],
	[ac_IsInvoiced],
	[ac_InvoiceDate],
	[Exclude]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-20131115-104351]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131115-104351] ON [dbo].[PhoneCalls2013]
(
	[ChargingParty] ASC
)
INCLUDE ( 	[SessionIdTime],
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
	[ReferredBy],
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
	[ui_AssignedByUser],
	[ui_AssignedOn],
	[ui_CallType],
	[ac_DisputeStatus],
	[ac_DisputeResolvedOn],
	[ac_IsInvoiced],
	[ac_InvoiceDate],
	[Exclude]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-20131115-104412]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131115-104412] ON [dbo].[PhoneCalls2013]
(
	[marker_CallTypeID] ASC
)
INCLUDE ( 	[SessionIdTime],
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
	[ReferredBy],
	[ChargingParty],
	[Duration],
	[marker_CallFrom],
	[marker_CallTo],
	[marker_CallToCountry],
	[marker_CallCost],
	[marker_CallType],
	[marker_TimeStamp],
	[ui_MarkedOn],
	[ui_UpdatedByUser],
	[ui_AssignedByUser],
	[ui_AssignedOn],
	[ui_CallType],
	[ac_DisputeStatus],
	[ac_DisputeResolvedOn],
	[ac_IsInvoiced],
	[ac_InvoiceDate],
	[Exclude]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-20131115-104434]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131115-104434] ON [dbo].[PhoneCalls2013]
(
	[ToGateway] ASC
)
INCLUDE ( 	[SessionIdTime],
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
	[ReferredBy],
	[ChargingParty],
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
	[ui_AssignedByUser],
	[ui_AssignedOn],
	[ui_CallType],
	[ac_DisputeStatus],
	[ac_DisputeResolvedOn],
	[ac_IsInvoiced],
	[ac_InvoiceDate],
	[Exclude]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-20131130-131811]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131130-131811] ON [dbo].[PhoneCalls2013]
(
	[ui_AssignedToUser] ASC
)
INCLUDE ( 	[SessionIdTime],
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
	[ReferredBy],
	[ChargingParty],
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
	[ui_AssignedByUser],
	[ui_AssignedOn],
	[ui_CallType],
	[ac_DisputeStatus],
	[ac_DisputeResolvedOn],
	[ac_IsInvoiced],
	[ac_InvoiceDate],
	[Exclude],
	[CalleeURI]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-20140108-104047]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20140108-104047] ON [dbo].[PhoneCalls2013]
(
	[ui_CallType] ASC
)
INCLUDE ( 	[SessionIdTime],
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
	[ReferredBy],
	[ChargingParty],
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
	[ui_AssignedByUser],
	[ui_AssignedOn],
	[ac_DisputeStatus],
	[ac_DisputeResolvedOn],
	[ac_IsInvoiced],
	[ac_InvoiceDate],
	[Exclude],
	[CalleeURI],
	[ui_AssignedToUser]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-20140108-104243]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20140108-104243] ON [dbo].[PhoneCalls2013]
(
	[ac_IsInvoiced] ASC
)
INCLUDE ( 	[SessionIdTime],
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
	[ReferredBy],
	[ChargingParty],
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
	[ui_AssignedByUser],
	[ui_AssignedOn],
	[ui_CallType],
	[ac_DisputeStatus],
	[ac_DisputeResolvedOn],
	[ac_InvoiceDate],
	[Exclude],
	[CalleeURI],
	[ui_AssignedToUser]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-20131018-092614]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131018-092614] ON [dbo].[PhoneCallsExceptions]
(
	[UserUri] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [NonClusteredIndex-20131018-092636]    Script Date: 1/24/2014 1:15:45 PM ******/
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20131018-092636] ON [dbo].[PhoneCallsExceptions]
(
	[Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ActiveDirectoryUsers] ADD  CONSTRAINT [DF_ActiveDirectoryUsers_UpdatedByAD]  DEFAULT ((1)) FOR [UpdatedByAD]
GO
ALTER TABLE [dbo].[PhoneCalls2010] ADD  CONSTRAINT [DF__PhoneCall2010__ac_Is__05A3D694]  DEFAULT (N'NO') FOR [ac_IsInvoiced]
GO
ALTER TABLE [dbo].[PhoneCalls2010] ADD  DEFAULT ((0)) FOR [Exclude]
GO
ALTER TABLE [dbo].[PhoneCalls2013] ADD  CONSTRAINT [DF__PhoneCall2013__ac_Is__05A3D694]  DEFAULT (N'NO') FOR [ac_IsInvoiced]
GO
ALTER TABLE [dbo].[PhoneCalls2013] ADD  DEFAULT ((0)) FOR [Exclude]
GO
ALTER TABLE [dbo].[GatewaysDetails]  WITH CHECK ADD  CONSTRAINT [FK_GatewaysDetails_Pools] FOREIGN KEY([PoolID])
REFERENCES [dbo].[Pools] ([PoolID])
GO
ALTER TABLE [dbo].[GatewaysDetails] CHECK CONSTRAINT [FK_GatewaysDetails_Pools]
GO
ALTER TABLE [dbo].[GatewaysDetails]  WITH CHECK ADD  CONSTRAINT [FK_GatewaysDetails_Sites] FOREIGN KEY([SiteID])
REFERENCES [dbo].[Sites] ([SiteID])
GO
ALTER TABLE [dbo].[GatewaysDetails] CHECK CONSTRAINT [FK_GatewaysDetails_Sites]
GO
ALTER TABLE [dbo].[GatewaysRates]  WITH CHECK ADD  CONSTRAINT [FK_GatewaysRates_Gateways] FOREIGN KEY([GatewayID])
REFERENCES [dbo].[Gateways] ([GatewayId])
GO
ALTER TABLE [dbo].[GatewaysRates] CHECK CONSTRAINT [FK_GatewaysRates_Gateways]
GO
ALTER TABLE [dbo].[Roles_Delegates]  WITH CHECK ADD  CONSTRAINT [FK_Delegates_DelegateTypes1] FOREIGN KEY([DelegeeType])
REFERENCES [dbo].[DelegateTypes] ([ID])
GO
ALTER TABLE [dbo].[Roles_Delegates] CHECK CONSTRAINT [FK_Delegates_DelegateTypes1]
GO
ALTER TABLE [dbo].[Roles_Delegates]  WITH CHECK ADD  CONSTRAINT [FK_Delegates_Sites] FOREIGN KEY([SiteID])
REFERENCES [dbo].[Sites] ([SiteID])
GO
ALTER TABLE [dbo].[Roles_Delegates] CHECK CONSTRAINT [FK_Delegates_Sites]
GO
ALTER TABLE [dbo].[Roles_DepartmentsHeads]  WITH CHECK ADD  CONSTRAINT [FK_DepartmentsHeads_DepartmentsHeads] FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Departments] ([ID])
GO
ALTER TABLE [dbo].[Roles_DepartmentsHeads] CHECK CONSTRAINT [FK_DepartmentsHeads_DepartmentsHeads]
GO
ALTER TABLE [dbo].[Roles_System]  WITH CHECK ADD  CONSTRAINT [FK_RolesPerUsers_Sites] FOREIGN KEY([SiteID])
REFERENCES [dbo].[Sites] ([SiteID])
GO
ALTER TABLE [dbo].[Roles_System] CHECK CONSTRAINT [FK_RolesPerUsers_Sites]
GO
ALTER TABLE [dbo].[Sites_Departments]  WITH CHECK ADD  CONSTRAINT [FK_Sites_Departments_Departments] FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Departments] ([ID])
GO
ALTER TABLE [dbo].[Sites_Departments] CHECK CONSTRAINT [FK_Sites_Departments_Departments]
GO
ALTER TABLE [dbo].[Sites_Departments]  WITH CHECK ADD  CONSTRAINT [FK_Sites_Departments_Sites] FOREIGN KEY([SiteID])
REFERENCES [dbo].[Sites] ([SiteID])
GO
ALTER TABLE [dbo].[Sites_Departments] CHECK CONSTRAINT [FK_Sites_Departments_Sites]
GO
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
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_SitesGateways'
GO
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
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Countries'
GO
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
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_Gateways_Full'
GO
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
   End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_User_Role_Details'
GO
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
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vw_User_Role_Details'
GO
USE [master]
GO
ALTER DATABASE [tBill] SET  READ_WRITE 
GO
