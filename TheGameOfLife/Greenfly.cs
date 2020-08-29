namespace TheGameOfLife
{
    public class Greenfly : Insect
    {
        /// <summary>
        /// Method for checking if greenfly has to breed
        /// </summary>
        /// <returns>returns true if greenfly is breeding</returns>
        public bool IsBreeding()
        {
            //if greenfly had its third timestep since last breeding, it breeds
            if (timestep % 3 == 0) return true;

            return false;
        }
    }
}
