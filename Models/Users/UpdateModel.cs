namespace WebApi.Models.Users
{   
    /// <summary>
    /// Represents the **Update** Endpoint of the API for the User table.
    /// </summary>
    public class UpdateModel
    {   
        /// <summary>
        /// Represents the First Name of the user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Represents the Last Name of the user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Represents the Username of the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Represents the Password field of the user.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Represents the Gender of the user.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Represents the Preferred platform of the user.
        /// </summary>
        public string PreferredPlatform { get; set; }

    }
}