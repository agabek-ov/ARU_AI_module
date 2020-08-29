using System;
using System.Threading;
using System.Linq;

namespace TheGameOfLife
{
    public class GameLauncher
    {
        private static int refreshRate = 100,//Change to console readline OR create menu with different types of displaying
                           gridLength = 20, gridWidth = 20,
                           ladybridsNum = 5, greenfliesNum = 100;
        private static Insect[,] grid = new Insect[gridLength, gridWidth];
        private static Random random = new Random();

        /// <summary>
        /// Method for launching the application
        /// </summary>
        public static void Launch()
        {
            Initialise();
            Display(false);
            RunSimulation();
        }

        /// <summary>
        /// Method for initialiseing the grid
        /// </summary>
        private static void Initialise()
        {
            CreateInsect(typeof(Ladybird), ladybridsNum);
            CreateInsect(typeof(Greenfly), greenfliesNum);
        }

        /// <summary>
        /// Method for creating insects of a given number and type on the grid
        /// </summary>
        /// <param name="insectType"></param> type of insect to be created
        /// <param name="insectNum"></param> number of insects to be created
        private static void CreateInsect(Type insectType, int insectNum)
        {
            for (int i = 0; i < insectNum; i++)
            {
                //Generates random x and y positions
                int row = random.Next(0, gridLength), col = random.Next(0, gridWidth);

                //Checks if the cell is available
                //if true, creates an insect of given type
                if (grid[row, col] is null) grid[row, col] = (Insect)Activator.CreateInstance(insectType);
                //Else if cell is busy, it iterates back, and searches for another cell
                else i--;
            }
        }

        /// <summary>
        /// Method for displaying the grid with all the insects on it
        /// </summary>
        private static void Display(bool isFinalState)
        {
            //Printing top border
            PrintBorder();

            //Loops through the grid and prints rows with greenfly(o), ladybird(x) and empty cell
            LoopThroughGrid(PrintCell);

            Statistics.DisplayStatistics();

            Console.WriteLine(
            (isFinalState)
                ? $"\nProgram has finished at timestep: {Statistics.GetTimestepsCount()}"
                : "\nPress ESCAPE key to stop\n");
        }

        /// <summary>
        /// Method used in Display method for printing the border
        /// </summary>
        private static void PrintBorder()
        {
            for (int i = 0; i < gridWidth; i++)
                Console.Write("-----");
            Console.Write("-\n");
        }

        /// <summary>
        /// Method for printing each cell in the grid
        /// </summary>
        /// <param name="row"></param> current row
        /// <param name="col"></param> current column
        private static void PrintCell(int row, int col)
        {
            //Printing leftmost border
            if (col == 0) Console.Write("|");

            if (grid[row, col] != null)
            {
                if (grid[row, col] is Ladybird)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("{0,3}", "x");
                    Statistics.LadybirdsCount();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("{0,3}", "o");
                    Statistics.GreenfliesCount();
                }
                Console.ResetColor();
                Console.Write(" |");
            }
            else
                Console.Write("{0,3} |", " ");

            //Printing rightmost and bottom borders
            if (col == gridWidth - 1)
            {
                Console.WriteLine();
                //Printing bottom border
                PrintBorder();
            }
        }

        /// <summary>
        /// Method for running the simulation
        /// </summary>
        private static void RunSimulation()
        {
            Statistics.CreateStringBuilder();
            Statistics.SetUpInitialValues(greenfliesNum, ladybridsNum);
            bool isFinalState = GridIsFullOrEmpty();

            do
            {
                while (!(Console.KeyAvailable || isFinalState))
                {
                    MoveInsects();
                    isFinalState = GridIsFullOrEmpty();
                    Statistics.RecordStatistics();
                    Display(isFinalState);

                    //Slowing down the displaying part according to refreshRate (seconds per refresh)
                    Thread.Sleep(refreshRate);
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape && !isFinalState);

            Statistics.SaveFile();
        }

        /// <summary>
        /// Method for checking if grid reached its final state
        /// Either full of greenflies,
        /// or empty.
        /// </summary>
        /// <returns>returns true if final state reached</returns>
        private static bool GridIsFullOrEmpty()
        {
            if (Statistics.GetTotalInsects() == 0) return true;
            if (Statistics.GetGreenfliesCount() == gridLength * gridWidth) return true;
            return false;
        }

        /// <summary>
        /// Method for moving all the insects in the grid
        /// </summary>
        private static void MoveInsects()
        {
            //Loops through all the insects and sets the variable hasMoved to initial value
            grid.Cast<Insect>().Where(i => i != null).ToList().ForEach(insect => insect.NewTimestep());

            //Sends methods for moving each insect type as Action to loop through the grid
            LoopThroughGrid(MoveLadybird);
            LoopThroughGrid(MoveGreenfly);
        }

        /// <summary>
        /// Method for moving a ladybird according to the rules
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private static void MoveLadybird(int row, int col)
        {
            //Checks if the current cell is empty or not type of Ladybird, skips if true
            if (grid[row, col] is null || grid[row, col]?.GetType() != typeof(Ladybird)) return;

            //Checks if the insect has already moved or just spawned
            //if true, skips that cell
            if (grid[row, col].HasMoved()) return;

            //Checks for any adjacent greenflies and chooses a random one
            int[] newXY = GetAdjacentPositionOfType(typeof(Greenfly), row, col);

            //If values are same as before, no adjacent greenflies were found
            if (!(newXY[0] == row && newXY[1] == col))
            {
                //eats it and takes its place
                grid[newXY[0], newXY[1]] = grid[row, col];
                ((Ladybird)grid[newXY[0], newXY[1]]).AteGreenfly();
                grid[row, col] = null;
            }
            //If no greenflies were found, moves to random adjacent position
            else
            {
                newXY = MoveToRandomAdjacentPosition(row, col);
            }

            //Record insect's movement
            grid[newXY[0], newXY[1]].Move();

            //Checks if ladybird is starving
            //if true, it dies
            if (((Ladybird)grid[newXY[0], newXY[1]]).Starve())
            {
                grid[newXY[0], newXY[1]] = null;
                return;
            }

            //if next move causes breeding
            if (((Ladybird)grid[newXY[0], newXY[1]]).IsBreeding()) BreedInsect(newXY[0], newXY[1]);
        }

        /// <summary>
        /// Method for moving a greenfly according to the rules
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private static void MoveGreenfly(int row, int col)
        {
            //Checks if the current cell is empty or not type of Greenfly, skips if true
            if (grid[row, col] is null || grid[row, col]?.GetType() != typeof(Greenfly)) return;

            //Checks if the insect has already moved or just spawned
            //if true, skips that cell
            if (grid[row, col].HasMoved()) return;

            int[] newXY = MoveToRandomAdjacentPosition(ref row, col);

            //Record insect's movement
            grid[newXY[0], newXY[1]].Move();

            //if next move causes breeding
            if (((Greenfly)grid[newXY[0], newXY[1]]).IsBreeding()) BreedInsect(newXY[0], newXY[1]);
        }

        /// <summary>
        /// Method for moving to random adjacent cell
        /// if the cell is empty, occupies it and returns its position
        /// if not, stays in the initial cell and returns initial position
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>returns</returns>
        private static int[] MoveToRandomAdjacentPosition(int row, int col)
        {
            int[] newXY = { row, col };
            //Gets random direction to move on
            int[] offsetXY = GetRandomDirection();

            //Possible x and y positions to check
            int possibleX = row + offsetXY[0], possibleY = col + offsetXY[1];

            //Checks if new position is within the range of the grid
            //if true, checks the insect in the new cell
            if (!(0 > possibleX || possibleX >= gridLength || 0 > possibleY || possibleY >= gridWidth))
            {
                //if the new cell is available
                //Current greenfly occupies that cell
                if (grid[possibleX, possibleY] is null)
                {
                    //if true, moves the greenfly to the new cell
                    grid[possibleX, possibleY] = grid[row, col];
                    //and removes it from the previous
                    grid[row, col] = null;
                    newXY[0] = possibleX;
                    newXY[1] = possibleY;
                }
            }
            return newXY;
        }

        /// <summary>
        /// Method for breeding an insect of a given type
        /// on adjacent empty cell
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        private static void BreedInsect(int row, int col)
        {
            //Checks for any adjacent empty cells and chooses a random one
            int[] adjEmptyPos = GetAdjacentPositionOfType(null, row, col);

            //If values are same as before, no adjacent empty cells were found
            if (adjEmptyPos[0] == row && adjEmptyPos[1] == col) return;

            //Spawns an insect of the same type at the adjacent empty cell
            grid[adjEmptyPos[0], adjEmptyPos[1]] = (Insect)Activator.CreateInstance(grid[row,col].GetType());
        }

        /// <summary>
        /// Method for finding adjacent cells of a chosen type,
        /// and returning position of randomly chosen one.
        /// default return value is initial position
        /// </summary>
        /// <param name="type"></param> Type of cell to be searched for
        /// <param name="row"></param> X position of a ladybird
        /// <param name="col"></param> Y position of a ladybird
        /// <returns>array with two elements, x and y values of a new position</returns>
        private static int[] GetAdjacentPositionOfType(Type type, int row, int col)
        {
            //Setting default return value
            int[] newXY = { row, col };
            //Allocating memory for eight possible adjacent positions with certain type
            int[,] adjacentPos = new int[8, 2];
            int[] offset = { -1, 0, 1 };
            int positionsCounter = 0;

            //Loops through all possible directions (e.g. West, North-West, etc.)
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    //if i = 1 and j = 1, both x and y offsets are zero, and this iteration is skipped
                    if (i == 1 && j == 1) continue;

                    //applying the offset and checking the adjacent cells
                    int possibleX = row + offset[i], possibleY = col + offset[j];

                    //Checks if new position is not within the range of the grid, skips it if true
                    if (0 > possibleX || possibleX >= gridLength || 0 > possibleY || possibleY >= gridWidth) continue;

                    //Check if the cell is the same type as the searched type
                    if (grid[possibleX, possibleY]?.GetType() == type)
                    {
                        //Stores the position of the cell to the overall array
                        adjacentPos[positionsCounter, 0] = possibleX;
                        adjacentPos[positionsCounter, 1] = possibleY;
                        positionsCounter++;
                    }
                }
            }
            //if adjacent cells of the certain type were found, randomly chooses one
            //and stores its position before returning the value
            if (positionsCounter > 0)
            {
                int positionIndex = random.Next(0, positionsCounter);
                newXY[0] = adjacentPos[positionIndex, 0];
                newXY[1] = adjacentPos[positionIndex, 1];
            }
            return newXY;
        }

        /// <summary>
        /// Method for looping through the grid of insects
        /// </summary>
        /// <param name="method"></param> A method to be executed in the loop
        private static void LoopThroughGrid(Action<int, int> method)
        {
            for (int row = 0; row < gridLength; row++)
            {
                for (int col = 0; col < gridWidth; col++)
                {
                    method(row, col);
                }
            }
        }

        /// <summary>
        /// Method for generating random direction to move on
        /// </summary>
        /// <returns>
        /// Method returns an array with two cells
        /// each resposbile for the movement on axises
        /// returning values are between [-1,+1], for both first(x) and second(y) cells
        /// 
        /// All possible directions:
        ///(-1,-1)  (0,-1)  ( 1,-1)
        ///(-1, 0)          ( 1, 0)
        ///(-1, 1)  (0, 1)  ( 1, 1)
        /// </returns>
        private static int[] GetRandomDirection()
        {
            //Boolean variable for checking if random direction is found and valid
            bool positionChanged;

            //Initilises array with two cells, one for x-axis movement, other for y-axis movement
            int[] offsetXY = new int[2];

            //array with possible offsets
            int[] offset = { -1, 0, 1 };

            do
            {
                //Gets two random integers in the range from 0 to 3(not inclusive)
                int randomX = random.Next(0, 3);
                int randomY = random.Next(0, 3);

                //storing the offset in the returning value
                offsetXY[0] = offset[randomX];
                offsetXY[1] = offset[randomY];

                //Checks if both random X and random Y are zeros
                //if true, runs the loop again
                positionChanged = randomX == 1 && randomY == 1;

            } while (positionChanged);
            return offsetXY;
        }
    }
}