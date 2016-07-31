use cypher

select w.fldWord_word, w.fldWord_value from tblMessageWords mw join tblWords w on mw.fldMessageWord_wordID=w.fldWord_id where w.fldWord_value =101


