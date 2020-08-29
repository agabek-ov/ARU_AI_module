namespace TheGameOfLife
{
    public class Ladybird : Insect
    {
        //Counter for how many timesteps passed, since ladybird ate greenfly
        private int lastGreenfly = 0;

        /// <summary>
        /// Method for checking, if ladybird is starving
        /// </summary>
        /// <returns>returns true if ladybird is starving</returns>
        public bool Starve()
        {
            lastGreenfly++;
            if (lastGreenfly == 3) return true;

            return false;
        }

        /// <summary>
        /// Method for restarting the counter for lastGreenfly
        /// </summary>
        public void AteGreenfly() => lastGreenfly = 0;
        
        /// <summary>
        /// Method for checking if ladybird has to breed
        /// </summary>
        /// <returns>returns true if ladybird is breeding</returns>
        public bool IsBreeding()
        {
            //if ladybird had its eighth timestep since last breeding, it breeds
            if (timestep % 8 == 0) return true;

            return false;
        }
    }
}
