using System;

namespace TheGameOfLife
{
    public class Display
    {
        /// <summary>
        /// Default settings for the application
        /// </summary>
        private static int refreshRate = 500,
                           gridHeight = 20, gridWidth = 20,
                           ladybridsNum = 5, greenfliesNum = 100;
        private static bool autoMode = false;
        private static string autoModeStr = (autoMode) ? "ON" : "OFF";

        /// <summary>
        /// Method for displaying user interface
        /// </summary>
        /// <returns>returns whether to start application or not</returns>
        public static bool DisplayUserInterface()
        {
            bool choiceIsCorrect;
            bool startApplication;

            do
            {
                DisplayMenu();

                int choice = GetIntFromUser(1, 5);

                choiceIsCorrect = false;
                startApplication = true;

                switch (choice)
                {
                    case 1:
                        choiceIsCorrect = true;
                        break;
                    case 2:
                        ChangeAppSettings();
                        break;
                    case 3:
                        DisplayHelp();
                        break;
                    case 4:
                        DisplayAbout();
                        break;
                    case 5:
                        startApplication = false;
                        choiceIsCorrect = true;
                        Console.WriteLine("Application has been closed");
                        break;
                }
            } while (!choiceIsCorrect);

            return startApplication;
        }

        /// <summary>
        /// Method displaying options to change app settings
        /// and changes them depending on what user wanted
        /// </summary>
        private static void ChangeAppSettings()
        {
            bool choiceIsCorrect;
            int min, max;
            do
            {
                AdjustInsectsForGrid();
                DisplayCurrentAppSettings();

                min = 1;
                max = 7;

                int choice = GetIntFromUser(min, max);

                Console.Clear();

                choiceIsCorrect = false;

                switch (choice)
                {
                    case 1:
                        min = 1;
                        max = 20;
                        Console.WriteLine($"Grid Height: {gridHeight}. ");
                        Console.Write($"Enter Grid Height (between {min} and {max}): ");

                        gridHeight = GetIntFromUser(min, max);
                        break;
                    case 2:
                        min = 1;
                        max = 40;
                        Console.WriteLine($"Grid Width: {gridWidth}. ");
                        Console.Write($"Enter Grid Width (between {min} and {max}): ");

                        gridWidth = GetIntFromUser(min, max);
                        break;
                    case 3:
                        min = 0;
                        max = gridHeight * gridWidth - greenfliesNum;
                        Console.WriteLine($"Ladybirds number: {ladybridsNum}. ");
                        Console.Write($"Enter ladybirds number (between {min} and {max}): ");

                        ladybridsNum = GetIntFromUser(min, max);
                        break;
                    case 4:
                        min = 0;
                        max = gridHeight * gridWidth - ladybridsNum;
                        Console.WriteLine($"Greenflies number: {greenfliesNum}. ");
                        Console.Write($"Enter greenflies number (between {min} and {max}): ");

                        greenfliesNum = GetIntFromUser(min, max);
                        break;
                    case 5:
                        min = 100;
                        max = 2000;
                        Console.WriteLine($"Refresh rate: {refreshRate} milliseconds. ");
                        Console.Write($"Enter refresh rate (between {min} and {max}): ");

                        refreshRate = GetIntFromUser(min, max);
                        break;
                    case 6:
                        min = 0;
                        max = 1;
                        Console.WriteLine($"Automatic mode: {autoModeStr}");
                        Console.Write($"Enter mode ({min}-[OFF], {max}-[ON]): ");

                        autoMode = (GetIntFromUser(min, max) > 0);
                        autoModeStr = (autoMode) ? "ON" : "OFF";
                        break;
                    case 7:
                        choiceIsCorrect = true;
                        break;
                }
            } while (!choiceIsCorrect);
        }

        /// <summary>
        /// Method displaying main menu
        /// </summary>
        private static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("         The Game of Life\n\n" +
                              "Press a number of an option below and 'Enter':\n" +
                              "1) Run the application\n" +
                              "2) Change settings\n" +
                              "3) Help\n" +
                              "4) About\n" +
                              "5) Exit\n");
        }

        /// <summary>
        /// Method displaying current settings of the app
        /// </summary>
        private static void DisplayCurrentAppSettings()
        {
            Console.Clear();
            Console.WriteLine("Press a number of a setting to change and 'Enter'\n" +
                              "Current settings:\n" +
                              $"1) Grid Height: {gridHeight}\n" +
                              $"2) Grid Width: {gridWidth}\n" +
                              $"3) Ladybirds Number: {ladybridsNum}\n" +
                              $"4) Greenflies Number: {greenfliesNum}\n" +
                              $"5) Refresh rate: {refreshRate}\n" +
                              $"6) Automatic mode: {autoModeStr}\n" +
                              "7) Go back to menu");
        }

        /// <summary>
        /// Method displaying help details for the application
        /// </summary>
        private static void DisplayHelp()
        {
            Console.Clear();
            Console.WriteLine("         Help\n" +
                              "Grid Height and Width - dimensions of a grid to be created\n" +
                              "Ladybirds number      - number of ladybirds to be spawned at the beginning\n" +
                              "Greenflies number     - number of greenflies to be spawned at the beginning\n" +
                              "Refresh rate          - number of milliseconds between each refreshment of the screen\n" +
                              "Auto mode             - automatic mode for application to work without user interaction\n" +
                              "\nPress 'Enter' to return back to menu");
            Console.ReadLine();
            Console.Clear();
        }

        /// <summary>
        /// Method displaying about the application
        /// </summary>
        private static void DisplayAbout()
        {
            Console.Clear();
            Console.WriteLine("         About\n" +
                              "In  1970  the  British  mathematician  John  Conway  formulated  \n" +
                              "‘The Game  of  Life’ which models the  evolution  of  cells  in  a closed\n" +
                              "2D  grid  when  those  cells  are  governed  by  certain rules. The\n" +
                              "game is relevant for computer simulations of the real world in fields as\n" +
                              "diverse as biology and economics because patterns may emerge that can be used\n" +
                              "to predict the future (e.g. population  dynamics,  the  state  of  the  stock  market).\n" +
                              "\nPress 'Enter' to return back to menu");
            Console.ReadLine();
            Console.Clear();
        }

        /// <summary>
        /// Method for getting an integer input from the user
        /// </summary>
        /// <param name="min"></param> minimum expected number
        /// <param name="max"></param> maximum expected number
        /// <returns>return an integer of specified range</returns>
        private static int GetIntFromUser(int min, int max)
        {
            int input = 0;

            while (true)
            {
                try
                {

                    input = Convert.ToInt32(Console.ReadLine());
                    if (input >= min && input <= max) break;
                }
                catch
                {
                    Console.WriteLine($"Please enter a number between {min} and {max}: ");
                    continue;
                }
                Console.WriteLine($"Please enter a number between {min} and {max}: ");
            }
            return input;
        }

        /// <summary>
        /// Method for for adjusting the population of insects
        /// depending on the grid size if needed
        /// </summary>
        private static void AdjustInsectsForGrid()
        {
            if (ladybridsNum + greenfliesNum > gridHeight * gridWidth)
            {
                //Ratio of ladybirds per all cells is from assignment specs(5/400 = 1/80)
                ladybridsNum = (int)(gridHeight * gridWidth * (1.0 / 80.0));
                //Ratio of greenflies per all cells is from assignment specs(100/400 = 1/25) 
                greenfliesNum = (int)(gridHeight * gridWidth * (1.0 / 4.0));
            }
        }

        /// <summary>
        /// Method for returning all the settings
        /// </summary>
        /// <returns>returns all the settings</returns>
        public static (bool autoMode, int _refreshRate, int _gridHeight, int _gridWidth, int _ladybirdsNum, int _greenfliesNum) GetSettingsFromUser()
        {
            return (autoMode, refreshRate, gridHeight, gridWidth, ladybridsNum, greenfliesNum);
        }
    }
}
