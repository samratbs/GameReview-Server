namespace WebApi.Helpers
{   
    /// <summary>
    /// A Class used to Query specific fields from the database.
    /// </summary>
    public class Rating
    {   

        /// <summary>
        /// Used to represent and bring in all the Rating field from the database.
        /// </summary>
        public int Ratings { get; set; }

        /// <summary>
        /// Used to represent and bring in all the Feedback type field from the database.
        /// </summary>
        public string FeedbackType { get; set; }

    }
}