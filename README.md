# brainFSharpuck
A brainfuck interpretter in f#

This is a brainfuck interpreter made in f#. To compile a program, just give the path in the console argument of a file that respect this format
(This is the same input as the hackerrank challenge)
Input 
First line will contain two space separated integers, n m, which represent number of characters in input to BrainF__k program and number of lines in the program, respectively. Next line contains n+1 characters which represents the input for the BrainF__k program. This line ends with character '$' which represent the end of input. Please ignore this in input. Then follows m lines which is the BrainF__k program.


- After 100000 operations, the program is killed. I wish I could detect infinite loops, but I can't solve the halting problem
