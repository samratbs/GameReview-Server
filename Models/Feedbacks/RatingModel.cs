namespace WebApi.Models.Feedbacks
{   
    /// <summary>
    /// Represents the **Get Average Ratings** Endpoint of the API for Feedbacks.
    /// </summary>
    public class RatingModel
    {   
        /// <summary>
        /// Represents the calculated average rating.
        /// </summary>
        public float Rate { get; set; }

        /// <summary>
        /// Represents the calculated positive type ratings.
        /// </summary>
        public float positivePercentage { get; set; }

        /// <summary>
        /// Represents the calculated negative type ratings.
        /// </summary>
        public float negativePercentage { get; set; }

        /// <summary>
        /// Represents the total number of feedbacks received.
        /// </summary>
        public float totalFeedbacks { get; set; }
       
    }
}