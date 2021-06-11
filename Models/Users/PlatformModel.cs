namespace WebApi.Models.Users
{   

    /// <summary>
    /// Represents the **Get** user preferred platforms Endpoint of the API for Feedbacks.
    /// </summary>
    public class PlatformModel
    {   
        /// <summary>
        /// Represents the people who prefers PS4 in percentage.
        /// </summary>
        public float ps4Percentage { get; set; } 

        /// <summary>
        /// Represents the people who prefers PC in percentage.
        /// </summary>
        public float pcPercentage { get; set; } 

        /// <summary>
        /// Represents the people who prefers XBOX in percentage.
        /// </summary>
        public float xboxPercentage { get; set; } 

        /// <summary>
        /// Represents the people who prefers Swtich in percentage.
        /// </summary>
        public float switchPercentage { get; set; } 

        /// <summary>
        /// Represents the people who prefers Mobile devices in percentage.
        /// </summary>
        public float mobilePercentage { get; set; } 


    }
}