USE [tBill]
GO

/****** Object:  StoredProcedure [dbo].[ExcludeLocalCalls]    Script Date: 11/14/2013 13:20:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		S. Abu Ghaida
-- Create date: 26/7/2013
-- Description:	Exclude Local Greek Calls
-- =============================================
CREATE PROCEDURE [dbo].[ExcludeLocalCalls] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- UPDATE Greek Local Calls
	UPDATE PhoneCalls set Exclude=1 
	WHERE  
		(marker_CallType='National - Fixedline' OR marker_CallType='Local') and SourceUserUri like '%@ccc.gr'

END

GO


