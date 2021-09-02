# 8PuzzleSolver
C# Project capable of solving 8-15 puzzles

## How to Use:

When run, the program asks the user to enter a puzzle size.  To simulate the 8 puzzle, the user must enter 9 for the size.<br/>
The program then prompts the user to enter the puzzle in the starting / incorrect position with an e in the empty spot.  <br/>
E.g. 1 3 4 5 2 8 e 7 6
<br/>
The user is then asked to enter the configuration that they want the puzzle to finish in.
<br/>
E.g. 1 2 3 4 5 6 7 8 e
<br/>
<br/>
## Solutions
Once the user enters the desired target configuration, the puzzle will be solved using various algorithms (i.e. Dijkstra's, A*, IDA*) and will not only display the steps to the solution, but also the time taken to solve the puzzle and the number of moves checked.
