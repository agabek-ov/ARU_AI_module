using System;
using System.IO;
using System.Text;

namespace TheGameOfLife
{
    public class Statistics
    {
        private static StringBuilder statsFile = new StringBuilder();
        private static int greenfliesNum, ladybridsNum, ladybirdsCount, greenfliesCount, timestep;
        private static string fullPath;

        public static void SetUpInitialValues(int _greenfliesNum, int _ladybirdsNum)
        {
            greenfliesNum = _greenfliesNum;
            ladybridsNum = _ladybirdsNum;
        }

        public static void ClearStatistics()
        {
            greenfliesCount = 0;
            ladybirdsCount = 0;
        }

        public static void LadybirdsCount() => ladybirdsCount++;

        public static void GreenfliesCount() => greenfliesCount++;

        public static int GetLadybirdsCount() => ladybirdsCount;

        public static int GetGreenfliesCount() => greenfliesCount;

        public static int GetTimestepsCount() => timestep;

        public static int GetTotalInsects() => ladybirdsCount + greenfliesCount;

        public static void DisplayStatistics()
        {
            //Displaying the stats
            Console.WriteLine("\n\nLadybirds: " + ladybirdsCount);
            Console.WriteLine("Greenflies: " + greenfliesCount);
            Console.WriteLine("Total Insects: " + (ladybirdsCount + greenfliesCount));
        }

        public static void CreateStringBuilder()
        {
            string filename = "TheGoLStats.csv";
            fullPath = Path.GetFullPath(filename);
            statsFile.AppendLine(string.Format("0,{0},{1}", greenfliesNum, ladybridsNum));
        }

        public static void SaveFile() => File.WriteAllText(fullPath, statsFile.ToString());
        
        /// <summary>
        /// Method for recording the statistics in the String Builder
        /// </summary>
        public static void RecordStatistics()
        {
            timestep++;
            var first = timestep.ToString(); //assign timesteps to the first variable
            var second = greenfliesCount.ToString(); //assign greenflies to the second variable
            var third = ladybirdsCount.ToString(); //assign ladybirds to the third variable

            var newLine = string.Format("{0},{1},{2}", first, second, third);
            statsFile.AppendLine(newLine);

            ClearStatistics();
        }
    }
}
