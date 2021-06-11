namespace WebApi.Entities
{   

    /// <summary>
    /// Represents the User table of the database.
    /// </summary>
    public class User
    {   
        /// <summary>
        /// Represents the Primary Key of the User Table.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the First Name of the User.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Represents the Last Name of the User.
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// Represents the Username of the User.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Represents the Gender of the User.
        /// </summary>
        public string Gender { get; set; }
        
        /// <summary>
        /// Represents the Platform User Prefers.
        /// </summary>
        public string PreferredPlatform { get; set; }

        /// <summary>
        /// Represents the PasswordHash of the User Password for Security.
        /// </summary>
        public byte[] PasswordHash { get; set; }

        /// <summary>
        /// Represents the PasswordSalt of the User Password for Security.
        /// </summary>
        public byte[] PasswordSalt { get; set; }

    }
}