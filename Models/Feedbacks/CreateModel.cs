using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Feedbacks
{   
    /// <summary>
    /// Represents the **Create** Endpoint of the API for Feedbacks.
    /// </summary>
    public class CreateModel
    {   
        /// <summary>
        /// Represents the Comment provided by the user.
        /// </summary>
        [Required]
        public string Comment { get; set; }

        /// <summary>
        /// Represents the Feedback Type provided by the user.
        /// </summary>
        [Required]
        public string Type { get; set; }

        /// <summary>
        /// Represents the Ratings provided by the user.
        /// </summary>
        [Required]
        public int Ratings { get; set; }

        /// <summary>
        /// Represents the Foreign Key Reference of the feedback to the user.
        /// </summary>
        [Required]
        public int UserRef { get; set; }
    }
}