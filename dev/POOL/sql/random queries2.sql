use cypher

--select * from tblLog where fldLog_date > '2013-04-13 18:30' and fldLog_caller ='DetectSentence' order by fldLog_date

--select * from tblpermutations2 order by PermID

--drop table tblpermutations2

-- 1,206,365 permutations

select min(PermID) from tblpermutations2 where box2 is null 
  
 select * from tblpermutations2 where Box1='small' and box2='message' order by PermID
