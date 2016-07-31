-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		AC Nicholls
-- Create date: 2013-04-09
-- Description:	Selects a list of words relating to a messageID, can be filtered
--  by the word's corresponding value
-- =============================================
CREATE PROCEDURE cypher_selectMessageWords
@filter bit, @value int, @messageID int
AS
BEGIN
declare @strSQL varchar(max)
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	set @strSQL = 'SELECT w.fldWord_id, w.fldWord_word, w.fldWord_value from 
	tblMessageWords mw join tblWords w on mw.fldMessageWord_wordID = w.fldWord_id
	where fldMessageWord_messageID=@messageID'
	if @filter=1
	begin
		set @strSQL = @strSQL + ' and w.fldWord_value=@value'
	end
	
	exec sp_sqlexec @strSQL
END
GO
