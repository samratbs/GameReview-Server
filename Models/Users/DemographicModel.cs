namespace WebApi.Models.Users
{   
    /// <summary>
    /// Represents the **Get** Endpoint of the API for Male to Female audience ratio.
    /// </summary>
    public class DemographicModel
    {   
        /// <summary>
        /// Represents the people who are male in percentage.
        /// </summary>
        public float malePercentage { get; set; }

        /// <summary>
        /// Represents the people who are female in percentage.
        /// </summary>
        public float femalePercentage { get; set; } 

        /// <summary>
        /// Represents the people who are other in percentage.
        /// </summary>
        public float otherPercentage { get; set; } 

        /// <summary>
        /// Represents the total number of users registered.
        /// </summary>
        public float totalUsers { get; set; } 


    }
}