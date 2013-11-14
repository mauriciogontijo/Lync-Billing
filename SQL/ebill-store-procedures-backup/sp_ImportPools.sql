USE [tBill]
GO

/****** Object:  StoredProcedure [dbo].[sp_ImportPools]    Script Date: 11/14/2013 13:24:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		W. Hadidi
-- Create date: 27/June/2013
-- Description:	Import Pools from Live System
-- =============================================
CREATE PROCEDURE [dbo].[sp_ImportPools] 
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	DECLARE @LastPoolInDestination [int],
			@OPENQUERY nvarchar(4000), 
			@TSQL nvarchar(4000), 
			@sqlQuery nvarchar(4000),
			@LinkedServer nvarchar(4000)

	SET @LastPoolInDestination = (SELECT max([PoolID]) from [dbo].[Pools])
	SET @LastPoolInDestination = ISNULL(@LastPoolInDestination,-1)


	SET @LinkedServer = '[10.1.0.131]'
	SET @OPENQUERY = 'SELECT * FROM OPENQUERY('+ @LinkedServer + ','''
	SET @TSQL =	   'SELECT [PoolId], [PoolFQDN] 
					FROM [LcsCDR].[dbo].[Pools] 
					WHERE [PoolId] > ' + CONVERT(nvarchar(10),@LastPoolInDestination) +
					' ORDER BY [PoolId] '')'


--SET IDENTITY_INSERT [dbo].[Pools] ON 

INSERT INTO [dbo].[Pools]
           ([PoolID]
           ,[PoolFQDN])
EXEC (@OPENQUERY+@TSQL) 
END



GO


