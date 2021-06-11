using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Users
{   
    /// <summary>
    /// Represents the **Register** Endpoint of the API for the User table.
    /// </summary>
    public class RegisterModel
    {   
        /// <summary>
        /// Represents the First Name of the user.
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Represents the Last Name of the user.
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Represents the Username of the user.
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Represents the Password field of the user.
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Represents the Gender of the user.
        /// </summary>
        [Required]
        public string Gender { get; set; }
        
        /// <summary>
        /// Represents the Preferred platform of the user.
        /// </summary>
        [Required]
        public string PreferredPlatform { get; set; }


    }
}