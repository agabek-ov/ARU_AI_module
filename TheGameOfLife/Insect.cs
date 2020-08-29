namespace TheGameOfLife
{
    public class Insect
    {
        //TODO Switch to props, fix stack overflow issue
        //Number of timesteps done by an insect
        protected int timestep = 0;
        //Boolean for if insect has moved in the timestep or not
        protected bool hasMoved;

        public Insect()
        {
            //When a new insect is spawned/created, it has already moved
            hasMoved = true;
        }

        /// <summary>
        /// Method for recording the movement of the insect
        /// </summary>
        public void Move()
        {
            timestep++;
            hasMoved = true;
        }

        /// <summary>
        /// Method for renewing the moved boolean, as new timestep started
        /// </summary>
        public void NewTimestep() => hasMoved = false;

        /// <summary>
        /// Method for checking if
        /// the insect has already moved
        /// in the particular timestep
        /// </summary>
        /// <returns></returns>
        public bool HasMoved() => hasMoved;
    }
}
