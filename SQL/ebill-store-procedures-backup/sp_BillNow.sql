USE [tBill]
GO

/****** Object:  StoredProcedure [dbo].[sp_BillNow]    Script Date: 11/14/2013 13:21:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		W. Hadidi
-- Create date: 13/Mar/2013
-- Description:	Mark And Bill VOIP Table
-- =============================================
CREATE PROCEDURE [dbo].[sp_BillNow]
AS
BEGIN
	-- Declare the return variable here
		DECLARE @callFrom bigint,
				@callTo bigint,
				@callType4Billing nvarchar(450)

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--EXEC dbo.sp_CallsMarker_2013

	UPDATE [dbo].[PhoneCalls]
		SET [marker_CallFrom] = dbo.fnc_phone2DialingPrefix([SourceNumberUri])
			,[marker_CallTo] = dbo.fnc_phone2DialingPrefix([DestinationNumberUri])
			,[marker_CallType] = dbo.fnc_GetCallType(dbo.fnc_phone2DialingPrefix([SourceNumberUri]),dbo.fnc_phone2DialingPrefix([DestinationNumberUri]))
		WHERE [marker_CallTypeID] = 1
		AND [SourceNumberUri] like '+[0-9]%' AND [DestinationNumberUri] like '+[0-9]%'
		AND [marker_CallFrom] IS NULL AND [marker_CallTo] IS NULL 
	UPDATE [dbo].[PhoneCalls]
		SET [marker_CallToCountry] = dbo.[fnc_DialingPrefix2Country]([marker_CallTo])
		WHERE [marker_CallTypeID] = 1
		AND [marker_CallToCountry] IS NULL
	UPDATE [dbo].[PhoneCalls]
		SET [marker_CallCost] = dbo.[fnc_GetCallCost]([ResponseTime],[ToGateway],[marker_CallTo],[Duration]),
			[marker_TimeStamp] = SYSDATETIME()
		WHERE [marker_CallTypeID] = 1
		AND [marker_CallCost] IS NULL
END



GO


