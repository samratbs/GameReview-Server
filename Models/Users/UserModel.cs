namespace WebApi.Models.Users
{   

    /// <summary>
    /// Represents the **Get** Endpoints of the API for the User table.
    /// </summary>
    public class UserModel
    {   
        /// <summary>
        /// Represents the primary key of the user table.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the First Name of the user table.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Represents the Last Name of the user table.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Represents the Username of the user table.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Represents the Gender of the user table.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Represents the Preferred platform of the user.
        /// </summary>
        public string PreferredPlatform { get; set; }
    }
}