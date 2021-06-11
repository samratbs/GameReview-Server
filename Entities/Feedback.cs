namespace WebApi.Entities
{   

    /// <summary>
    /// Represents the Feedback table of the database.
    /// </summary>
    public class Feedback
    {   
        /// <summary>
        /// Represents the Primary key of the Feedback Table.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the Comment Feedback provided by the User.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Represents the Feedback Type provided by the User.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Represents the Ratings provided by the User.
        /// </summary>
        public int Ratings { get; set; }

        /// <summary>
        /// Represents the Foreign Key Reference to the User table.
        /// </summary>
        public int UserRef { get; set; }
    }
}