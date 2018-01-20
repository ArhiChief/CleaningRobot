namespace CleaningRobot.Models
{
    /// <summary>
    /// Map cell types
    /// </summary>
    public enum MapCell
    {
        /// <summary>
        /// A cleanable space of 1 by 1 that can be occupied and cleaned (S)
        /// </summary>
        S,
        /// <summary>
        /// A column of 1 by 1 which can’t be occupied or cleaned (C)
        /// </summary>
        C,
        /// <summary>
        /// A wall (represented by an empty cell or by being outside the matrix) (null)
        /// </summary>
        Null
    }
}