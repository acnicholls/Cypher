use cypher

select * from tblWords where fldWord_value in (116,61,111,80,77,104,91,81,78,104,89,88,93,56,66,55,35,79,108,101,81,82,42,127,124,40,48,47,90,87,45,88,51,114,116,118,112,63,58,64)
and fldWord_word like 'a%'


select * from tblMessageWords
