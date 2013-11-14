USE [tBill]
GO

/****** Object:  StoredProcedure [dbo].[sp_ImportGateways]    Script Date: 11/14/2013 13:24:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		W. Hadidi
-- Create date: 01/May/2013
-- Description:	Import Phone Calls from Live System
-- =============================================
CREATE PROCEDURE [dbo].[sp_ImportGateways] 
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	DECLARE @LastGatewayInDestination [int],
			@OPENQUERY nvarchar(4000), 
			@TSQL nvarchar(4000), 
			@sqlQuery nvarchar(4000),
			@LinkedServer nvarchar(4000)

	SET @LastGatewayInDestination = (SELECT max([GatewayId]) from [dbo].[Gateways])
	SET @LastGatewayInDestination = ISNULL(@LastGatewayInDestination,-1)


	SET @LinkedServer = '[10.1.0.131]'
	SET @OPENQUERY = 'SELECT * FROM OPENQUERY('+ @LinkedServer + ','''
	SET @TSQL =	   'SELECT [GatewayId], [Gateway] 
					FROM [LcsCDR].[dbo].[Gateways] 
					WHERE [GatewayId] > ' + CONVERT(nvarchar(10),@LastGatewayInDestination) +
					' ORDER BY [GatewayId] '')'


SET IDENTITY_INSERT [dbo].[Gateways] ON 

INSERT INTO [dbo].[Gateways]
           ([GatewayId]
           ,[Gateway])
EXEC (@OPENQUERY+@TSQL) 
END


GO


