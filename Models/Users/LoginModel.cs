using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Users
{   
    /// <summary>
    /// Represents the **Login** Endpoint of the API.
    /// </summary>
    public class LoginModel
    {   
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
    }
}