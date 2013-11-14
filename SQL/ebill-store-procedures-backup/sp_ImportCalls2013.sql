USE [tBill]
GO

/****** Object:  StoredProcedure [dbo].[sp_ImportCalls2013]    Script Date: 11/14/2013 13:23:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		W. Hadidi, S. Abu Ghaida
-- Create date: 25/April/2013
-- Description:	Import Phone Calls from Live System
-- =============================================
CREATE PROCEDURE [dbo].[sp_ImportCalls2013] 
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	DECLARE @LastRecordDateInDestination [datetime],
			@FirstRecordDateInSource [datetime],
			@OPENQUERY nvarchar(4000), 
			@TSQL nvarchar(4000), 
			@sqlQuery nvarchar(4000),
			@LinkedServer nvarchar(4000)

	SET @LastRecordDateInDestination = (SELECT max([SessionIdTime]) from [dbo].[PhoneCalls2013])

	SET @FirstRecordDateInSource = (SELECT * FROM OPENQUERY([10.1.1.170\LYNC2013MON01], 'SELECT MIN([SessionIdTime]) FROM [LcsCDR].[dbo].[VoipDetails]'))

	SET @LastRecordDateInDestination = ISNULL(@LastRecordDateInDestination,@FirstRecordDateInSource)


	SET @LinkedServer = '[10.1.1.170\LYNC2013MON01]'
	SET @OPENQUERY = 'SELECT * FROM OPENQUERY('+ @LinkedServer + ','''
	SET @TSQL = 'SELECT        
							 VoipDetails.SessionIdTime, VoipDetails.SessionIdSeq, Users_1.UserUri AS SourceUserUri, Users_2.UserUri AS DestinationUserUri, Phones.PhoneUri AS SourceNumberUri, 
							 Phones_1.PhoneUri AS DestinationNumberUri, MediationServers.MediationServer AS FromMediationServer, 
							 MediationServers_1.MediationServer AS ToMediationServer, Gateways.Gateway AS FromGateway, Gateways_1.Gateway AS ToGateway, 
							 EdgeServers.EdgeServer AS SourceUserEdgeServer, EdgeServers_1.EdgeServer AS DestinationUserEdgeServer, Servers.ServerFQDN, Pools.PoolFQDN, 
							 SessionDetails.ResponseTime, SessionDetails.SessionEndTime, CONVERT(decimal(8, 0), DATEDIFF(second, SessionDetails.ResponseTime, 
							 SessionDetails.SessionEndTime)) AS Duration
					FROM     [LcsCDR].[dbo].[SessionDetails] LEFT OUTER JOIN
							 [LcsCDR].[dbo].[Servers] ON SessionDetails.ServerId = Servers.ServerId LEFT OUTER JOIN
							 [LcsCDR].[dbo].[Pools] ON SessionDetails.PoolId = Pools.PoolId LEFT OUTER JOIN
							 [LcsCDR].[dbo].[SIPResponseMetaData] ON SessionDetails.ResponseCode = SIPResponseMetaData.ResponseCode LEFT OUTER JOIN
							 [LcsCDR].[dbo].[EdgeServers] AS EdgeServers_1 ON SessionDetails.User2EdgeServerId = EdgeServers_1.EdgeServerId LEFT OUTER JOIN
							 [LcsCDR].[dbo].[EdgeServers] ON SessionDetails.User1EdgeServerId = EdgeServers.EdgeServerId LEFT OUTER JOIN
							 [LcsCDR].[dbo].[Users] AS Users_2 ON SessionDetails.User2Id = Users_2.UserId LEFT OUTER JOIN
							 [LcsCDR].[dbo].[Users] AS Users_1 ON SessionDetails.User1Id = Users_1.UserId RIGHT OUTER JOIN
							 [LcsCDR].[dbo].[VoipDetails] LEFT OUTER JOIN
							 [LcsCDR].[dbo].[Gateways] AS Gateways_1 ON VoipDetails.ToGatewayId = Gateways_1.GatewayId LEFT OUTER JOIN
							 [LcsCDR].[dbo].[Gateways] ON VoipDetails.FromGatewayId = Gateways.GatewayId LEFT OUTER JOIN
							 [LcsCDR].[dbo].[MediationServers] AS MediationServers_1 ON VoipDetails.ToMediationServerId = MediationServers_1.MediationServerId LEFT OUTER JOIN
							 [LcsCDR].[dbo].[MediationServers] ON VoipDetails.FromMediationServerId = MediationServers.MediationServerId LEFT OUTER JOIN
							 [LcsCDR].[dbo].[Phones] AS Phones_1 ON VoipDetails.ConnectedNumberId = Phones_1.PhoneId LEFT OUTER JOIN
							 [LcsCDR].[dbo].[Phones] ON VoipDetails.FromNumberId = Phones.PhoneId ON SessionDetails.SessionIdTime = VoipDetails.SessionIdTime AND 
							 SessionDetails.SessionIdSeq = VoipDetails.SessionIdSeq
				WHERE		Users_1.UserUri IS NOT NULL AND SessionDetails.ResponseCode = 200 AND SessionDetails.MediaTypes = 16 
							AND VoipDetails.SessionIdTime < dateadd(day,datediff(day,1,GETDATE()),0)
							AND VoipDetails.SessionIdTime > ''''' + CONVERT(nvarchar(23),@LastRecordDateInDestination,121) + ''''''')'

INSERT INTO [dbo].[PhoneCalls2013]
           ([SessionIdTime]
           ,[SessionIdSeq]
		   ,[SourceUserUri]
           ,[DestinationUserUri]
           ,[SourceNumberUri]
           ,[DestinationNumberUri]
           ,[FromMediationServer]
           ,[ToMediationServer]
           ,[FromGateway]
           ,[ToGateway]
		   ,[SourceUserEdgeServer]
           ,[DestinationUserEdgeServer]
           ,[ServerFQDN]
           ,[PoolFQDN]
           ,[ResponseTime]
           ,[SessionEndTime]
           ,[Duration])
EXEC (@OPENQUERY+@TSQL) 
END


GO


