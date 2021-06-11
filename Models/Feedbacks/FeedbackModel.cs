namespace WebApi.Models.Feedbacks
{
    /// <summary>
    /// Represents the **Get** Endpoints of the API for Feedbacks.
    /// </summary>
    public class FeedbackModel
    {   
        /// <summary>
        /// Represents the id of the feedback.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the Comment provided by the user.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Represents the Feedback Type provided by the user.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Represents the Ratings provided by the user.
        /// </summary>
        public int Ratings { get; set; }

        /// <summary>
        /// Represents the Foreign Key Reference of the feedback to the user.
        /// </summary>
        public int UserRef { get; set; }
    }
}