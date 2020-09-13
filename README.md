# ARU_AI_module

This console application was made by me during completing Artificial Intelligence module at ARU (Achieved mark: 87%).

Mainly it was made in Visual Studio in C#. However, in order to analyse average lifespan of each insect a MATLAB script
was written which could be found in bin/Debug folder.

Assignment description:
In 1970 the British mathematician John Conway formulated ‘The Game of Life’ which models the evolution of
cells in a closed 2D grid when those cells are governed by certain rules. The game is relevant for computer
simulations of the real world in fields as diverse as biology and economicsbecause patterns may emerge that 
can be used to predict the future (egpopulation  dynamics,  the  state  of  the  stock  market).  This assignment 
requires  you  to  design and implement a program that models a version of the Game ofLife as described below. Note
thatonce the game is started, no user-interaction is required other than to press a key to markthe end of one time-period
and the start of the next, or to exit the game. Each key press changes  the  state  of  the  game  and  thus  represents 
a  time-step  (actual real units  of  time  are irrelevant).The  Game  of  Life  variation  you  are  to  model  involves
creating  a  simple  2D console-based predator-prey  simulation  between  greenflies  (prey)  and  ladybirds  (predator). 
These  insects live in  a  world  composed  of  a  20x20  grid  of  cells. The  grid  is  enclosed  so  an  insect  is  not 
allowed to move off the edges of the world (nor incidentally,can either insectfly). Only one insect can occupy a cell at a time.

Each insect performs some action everytime step.The greenflies behave according to the following model: 

Move.  Every  time  step,  randomly  try  to  move  up,  down,  left  or  right.  If  the neighbouring cell 
in the selected direction is occupied or would move the greenfly off the grid, then the greenfly stays in the current cell.

Breed. If a greenfly survives for three time steps, then at the end of the time step (that is, after moving) the greenfly will breed.
This is simulated by creating a new greenfly in  an  adjacent  (up,  down,  left  or  right)  cell  that  is  empty.  If  there  are  
no  empty  cells available,  then  no  breeding  occurs. Once  an  offspring  is  produced  a  greenfly  cannot produce an offspring 
until three more time steps have elapsed.

The ladybirds behave according to the following model:

Move. Every  time  step,  if  there  is  an  adjacent  greenfly  (up,  down,  left  or  right),  then the ladybird will move to that cell 
and eat the greenfly. Otherwise, the ladybird moves according  to  the  same  rules  as  the  greenfly.  Note  that  a  ladybird  cannot 
eat  other ladybirds.

Breed. If a ladybirdsurvives for eighttime steps, then at the end of the time step it will spawn off another ladybird in the same manner as
the greenfly.

Starve.  If  a  ladybird  has  not  eaten  a  greenfly  within  the  last  three  time  steps,  then  at the  end  of  the  third  time  
step  it  will  starve  and  die.  The  ladybird  should  then  be removed from the grid of cells.During one turn, all the ladybirds should 
move before the greenflies do.
