USE [tBill]
GO

/****** Object:  StoredProcedure [dbo].[sp_nullthemall]    Script Date: 11/14/2013 13:24:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		W. Hadidi
-- Create date: 18/Jul/2013 
-- Description:	Nullify all Marker, UI and AC
--				related data
-- =============================================
CREATE PROCEDURE [dbo].[sp_nullthemall]
	-- Add the parameters for the stored procedure here
	@paranoid	nvarchar(30)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @clause nvarchar(64)
	SET @clause = 'Yes I am Sure'


    -- Insert statements for procedure here
	IF @paranoid = @clause
		BEGIN
			UPDATE PhoneCalls
			SET  [marker_CallFrom] = null,	[marker_CallTo] = null,		[marker_CallToCountry] = null, 
				 [marker_CallCost] = null,	[marker_CallTypeID] = null, [marker_CallType] = null,
			     [marker_TimeStamp] = null,	[ui_MarkedOn] = null,		[ui_UpdatedByUser] = null,
			     [ui_CallType] = null,		[ac_DisputeStatus] = null,	[ac_DisputeResolvedOn] = null,
			     [ac_IsInvoiced] = null,	[ac_InvoiceDate] = null
			-- Where clause here
		END
	ELSE
		PRINT 'You need to pass the following clause: ' + @clause
END

GO


