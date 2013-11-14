USE [tBill]
GO

/****** Object:  StoredProcedure [dbo].[sp_CallsMarker]    Script Date: 11/14/2013 13:22:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- ==============================================
-- Author:		W. Hadidi
-- Create date: 12/Mar/2013
-- Description:	Phone Calls Marker
-- ==============================================
-- CallTypeID = 60 > RASO calls before 1/May/2013
-- CallTypeID = 61 > JHAP calls before 1/May/2013
--
-- ==============================================
CREATE PROCEDURE [dbo].[sp_CallsMarker] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

-- Remove billing
--UPDATE [dbo].[PhoneCalls]
--	SET [marker_CallTypeID] = null, marker_CallCost = null, marker_CallFrom = null, marker_CallTo = null, marker_CallToCountry = null, marker_CallType = null

-- Mark Incoming phone calls
UPDATE [dbo].[PhoneCalls]
   SET [marker_CallTypeID] = 15, [marker_CallType] = 'Incoming phone call - free of charge'
 WHERE [SourceUserUri] is null

 -- Mark Toll-free numbers
UPDATE [dbo].[PhoneCalls]
   SET [marker_CallTypeID] = 12, [marker_CallType] = 'Toll-free numbers'
 WHERE [marker_CallTypeID] is null and [DestinationNumberUri] like '+800%'

-- Mark RASO-To-RASO Calls
-- DXB RASO +97143045000–999
UPDATE [dbo].[PhoneCalls]
   SET [marker_CallTypeID] = 2, [marker_CallType] = 'RASO-To-RASO - free of charge'
 WHERE [marker_CallTypeID] is null AND [SourceNumberUri] like '+97143045%'  AND [DestinationNumberUri] like '+97143045%'

-- Mark MOA-To-MOA Calls
-- MOA +302106182000–999
UPDATE [dbo].[PhoneCalls]
   SET [marker_CallTypeID] = 2, [marker_CallType] = 'MOA-To-MOA - free of charge'
 WHERE [marker_CallTypeID] is null AND [SourceNumberUri] like '+302106182%'  AND [DestinationNumberUri] like '+302106182%'

 -- Mark Khoubar Area-To-Khoubar Calls
-- KHB +96638062100-299   
UPDATE [dbo].[PhoneCalls]
   SET [marker_CallTypeID] = 2, [marker_CallType] = 'KHB-To-KHB - free of charge'
 WHERE [marker_CallTypeID] is null AND [SourceNumberUri] like '+96638062[100-299]%'  AND [DestinationNumberUri] like '+96638062[100-299]%'

-- Mark JHAP-To-JHAP Calls
-- JHAP +96633674350, +96633674351, +96633674352, +96633674353, +96633674357
UPDATE [dbo].[PhoneCalls]
   SET [marker_CallTypeID] = 2, [marker_CallType] = 'JHAP-To-JHAP - free of charge'
 WHERE [marker_CallTypeID] is null AND [SourceNumberUri] like '+9663367435[01237]%'  AND [DestinationNumberUri] like '+9663367435[01237]%'

-- Mark LON-To-LON Calls
-- UK LON +442072454440 to +442072454499 and +442072451340 to +442072451359
UPDATE [dbo].[PhoneCalls]
   SET [marker_CallTypeID] = 2, [marker_CallType] = 'LON-To-LON - free of charge'
 WHERE [marker_CallTypeID] is null AND ([SourceNumberUri] like '+4420724544[40-99]%' OR [SourceNumberUri] like '+4420724513[40-59]%')  AND ([DestinationNumberUri] like '+4420724544[40-99]%' OR [DestinationNumberUri] like '+4420724513[40-59]%')

-- Mark Calling to self - looks like calling voicemail by calling self number
UPDATE [dbo].[PhoneCalls]
   SET [marker_CallTypeID] = 16, [marker_CallType] = 'Call to self - free of charge'
 WHERE [marker_CallTypeID] is null AND ([SourceUserUri] = [DestinationUserUri] or  [SourceNumberUri] = [DestinationNumberUri])

-- Mark Lync to Lync Call
UPDATE [dbo].[PhoneCalls]
   SET [marker_CallTypeID] = 14, [marker_CallType] = 'Lync To Lync - regardless of site/pool - free of charge'
 WHERE [marker_CallTypeID] is null AND [DestinationUserUri] is not null AND (FromMediationServer is null and ToMediationServer is null and FromGateway is null and ToGateway is null) 

-- Mark CCC Vodafone Business Subscribers
UPDATE [dbo].[PhoneCalls]
   SET [marker_CallTypeID] = 11, [marker_CallType] = 'External PSTN - CCC Vodafone VPN - free of charge'
 WHERE [marker_CallTypeID] is null AND [PoolFQDN] in ('gr-moa-lync-pool.ccg.resource', 'moa-branch-sba1.ccg.resource') AND [DestinationNumberUri] in (Select '+30'+[Number]  from [dbo].[VodafoneBusinessSubscribers])

-- Mark Inter-site calls
UPDATE [dbo].[PhoneCalls]
   SET [marker_CallTypeID] = 4, [marker_CallType] = 'Inter-site Call - free of charge'
 WHERE [marker_CallTypeID] is null and [DestinationNumberUri] like '+0[35789]%' 

  -- Mark non-numeric numbers for future investigation
 UPDATE [dbo].[PhoneCalls]
   SET [marker_CallTypeID] = 17, [marker_CallType] = 'Bill Later - Investigate Destination'
 WHERE [marker_CallTypeID] is null and ISNUMERIC([DestinationNumberUri]) <> 1 

  -- Mark non-numeric numbers for future investigation
 UPDATE [dbo].[PhoneCalls]
   SET [marker_CallTypeID] = 17, [marker_CallType] = 'Bill Later - Investigate Source '
 WHERE [marker_CallTypeID] is null and ISNUMERIC([SourceNumberUri]) <> 1 


 -- Re-iterate type 17 for extra fine tuning
 -- The two above queries can be modified to eliminate the below
 -- but for the time being we will do it on two stages
 UPDATE [dbo].[PhoneCalls] 
	SET [marker_CallTypeID] = 1, [marker_CallType] = 'Source investigated - Chargeable'
	WHERE [marker_CallTypeID] = 17 and [marker_CallType] = 'Bill Later - Investigate Source' and SourceNumberUri like '+[1-9]%'

 UPDATE [dbo].[PhoneCalls] 
	SET [marker_CallTypeID] = 1, [marker_CallType] = 'Destination investigated - Chargeable'
	WHERE [marker_CallTypeID] = 17 and [marker_CallType] = 'Bill Later - Investigate Destination' and DestinationNumberUri like '+[1-9]%'


 -- Catch all - This should be always the last statement in this SP
UPDATE [dbo].[PhoneCalls]
   SET [marker_CallTypeID] = 1, [marker_CallType] = 'Chargeable'
 WHERE [marker_CallTypeID] is null
END
GO


