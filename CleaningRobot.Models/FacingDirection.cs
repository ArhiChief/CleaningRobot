namespace CleaningRobot.Models
{
    /// <summary>
    /// Possible facing direction of robot
    /// </summary>
    public enum FacingDirection
    {
        /// <summary>
        /// North
        /// </summary>
        N,
        /// <summary>
        /// East
        /// </summary>
        E,
        /// <summary>
        /// South
        /// </summary>
        S,
        /// <summary>
        /// West
        /// </summary>
        W
    }

    /// <summary>
    /// Extension methods to help working with facing direction of the robot
    /// </summary>
    public static class FacingDirectionExtensions
    {
        /// <summary>
        /// Turn facing direction left
        /// </summary>
        /// <param name="dir">Current direction</param>
        /// <returns>Result after turning left</returns>
        public static FacingDirection TurnLeft(this FacingDirection dir)
        {
            switch (dir)
            {
                case FacingDirection.N:
                    return FacingDirection.W;

                case FacingDirection.W:
                    return FacingDirection.S;

                case FacingDirection.S:
                    return FacingDirection.E;

                case FacingDirection.E:
                    return FacingDirection.N;
            }
            throw new System.ArgumentException("Invalid value", nameof(dir));
        }

        /// <summary>
        /// Turn facing direction right
        /// </summary>
        /// <param name="dir">Current direction</param>
        /// <returns>Result after turning right</returns>
        public static FacingDirection TurnRight(this FacingDirection dir)
        {
            switch (dir)
            {
                case FacingDirection.N:
                    return FacingDirection.E;

                case FacingDirection.E:
                    return FacingDirection.S;

                case FacingDirection.S:
                    return FacingDirection.W;
                    
                case FacingDirection.W:
                    return FacingDirection.N;
            }
            throw new System.ArgumentException("Invalid value", nameof(dir));
        }
    }
}