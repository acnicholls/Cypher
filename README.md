# Cypher
String encoding/decoding application

so this project started, because I saw a post on Reddit about an MI5 level code.
https://www.reddit.com/r/codes/comments/1bshvw/an_uncrackable_code_given_to_me_by_a_wwii_veteran/
it got me to thinking, exactly how to hide an english message in a string of numbers.

this method is to use a natural language processor to detect the percentage possibility that 
a given string of numbers, can be turned into words that make a sentence.

first a test sentence is provided to check the encode/decode procedure.
if the test sentence  can be decoded in this manner, then in all likelyhood the 
given message can as well.  

parameters may be adjusted later, like the calculation of the word value, which currently calculates each word 
in the given dictionary by giving each letter a numerical value from 1 to 26 and adding the sum of he words letters.
this calculation should be possible to shift, in case the code's designer used different numerical values for different letters.
there are very many permutations of this.  it would be a shame to miss that.  but for the test, we will use A-Z equals 1-26. 
if it fails, build a database table of all the possible combinations of 1-26 vs A-Z and run those permutations against the 
permutions of the words permutations.  this will not be simply done on a POC machine.  the permutations list of words for
the test message in the reddit post was 10 GB.  

the resulting values are matched  with the values in the given message.  each possible match is recorded for each 'word' value
in the message.  Next a calculation is done to see how many possible permutations there are to make each 
word value for each word be in a 'message format' with all the other words for which the word values matched.

this permutation number is then saved, and the 'message formats' are saved in the database.  
these messages are then processed by the NLP and given a percentage value for the likelihood that at least
one sentence is contained in the message.

These messages are then checked manually, to determine the effectiveness of the test.

acnicholls
