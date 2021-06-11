using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Users;
using WebApi.Models.Feedbacks;


namespace WebApi.Services
{   

    ///<summary>
    ///**UserService** class is implemented using this **IUserService** interface for abstraction
    ///This Service handles all the backend logic for the **User** endpoints with the database.
    ///</summary>
    public interface IUserService
    {   
        ///<param name="username">The Username of the user during Login.</param>
        ///<param name="password">The Password of the user during Login.</param>
        ///<returns>
        /// A **User** class which contains the logged in user information and a bearer token for authorization.
        /// It returns a null value if the user is not found or if the username and password does not match.
        /// </returns>
        User Login(string username, string password);

        /// <returns>
        /// An **IEnumerable** list of the User data.
        /// </returns>
        IEnumerable<User> GetAllUsers();

        /// <param name="id"> The id of the user whose information is to be retrieved.</param>
        /// <returns>
        /// A **User** class that contains the user information retrieved from the database.
        /// It returns an exception if the user is not found in the database.
        /// </returns>
        User GetUsersById(int id);

        ///<param name="user">The User information of the user that needs to register in **String**.</param>
        ///<param name="password">The Password of the user that needs to register in **String**.</param>
        ///<returns>
        /// A **User** class which contains the registered user information.
        /// It returns an exception if the username already exists in the database or if the password is invalid or null.
        /// </returns>
        User Register(User user, string password);

        ///<param name="user">The new user information of the user that is being updated.</param>
        ///<param name="password">The new password of the user that is being updated.</param>
        ///<returns>
        /// A **User** class which contains the new user information.
        /// It returns an exception if the user data is null or if the user who is updating doesn't exist in the database.
        /// It also throws exception if the new username is already taken by someone else. 
        /// </returns>
        void Update(User user, string password = null);

        ///<param name="id">The id of the user to delete.</param>
        ///<returns>
        /// void
        /// </returns>
        void Delete(int id);

        /// <returns>
        /// A **DemographicModel** class which contains the values for the results generated by the method.
        /// </returns>
        DemographicModel GetMaleFemaleRatio();

        /// <returns>
        /// A **PlatformModel** class which contains the values for the results generated by the method.
        /// </returns>
        PlatformModel GetPlatform();
    }

    ///<summary>
    ///The class that is responsible for all the server side logic of the **User** api endpoints.
    ///It ensures the usage of **DbContext** which maintains a session with the database allowing us to 
    ///query or save instances of the entities.
    ///</summary>
    public class UserService : IUserService
    {
        private DataContext _context;

        /// <summary>
        /// Constructor used for initialization.
        /// </summary>
        /// <param name="context">The variable used to represent the DbContext which communicates with the database.</param>
        public UserService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method helps for a user to Login using their credentials. For this,
        /// it performs various validations such as checking if the username and password sent is null or whether
        /// it exists in the datbase. It also calls the **VerifyPasswordHash** method and checks with the entered password.
        /// </summary>
        ///<param name="username">The Username of the user during Login.</param>
        ///<param name="password">The Password of the user during Login.</param>
        ///<returns>
        /// A **User** class which contains the logged in user information and a bearer token for authorization.
        /// It returns a null value if the user is not found or if the username and password does not match.
        /// </returns>
        public User Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.Username == username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        /// <summary>
        /// Fetches all the users from the database.
        /// </summary>
        /// <returns>
        /// An **IEnumerable** list of the User data.
        /// </returns>
        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users;
        }

        /// <summary>
        /// Fetches a specific user information from the database using the provided User id.
        /// </summary>
        /// <param name="id"> The id of the user whose information is to be retrieved.</param>
        /// <returns>
        /// A **User** class that contains the user information retrieved from the database.
        /// It returns an exception if the user is not found in the database.
        /// </returns>
        public User GetUsersById(int id)
        {
            return _context.Users.Find(id);
        }

        /// <summary>
        /// This method creates a user and adds it into the database. For this,
        /// it performs various validations such as checking if the username already exists in the database and if the password is valid.
        /// It also checks whether the data retrieved from the user is null or not. Likewise, it also encrypts the password using CreatePasswordHash
        /// method for additional security.
        /// </summary>
        ///<param name="user">The User information of the user that needs to register.</param>
        ///<param name="password">The Password of the user that needs to register in.</param>
        ///<returns>
        /// A **User** class which contains the registered user information.
        /// It returns an exception if the username already exists in the database or if the password is invalid or null.
        /// </returns>
        public User Register(User user, string password)
        {
            // check if the user has send a valid password
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            // checks whether the username exists in the database
            if (_context.Users.Any(x => x.Username == user.Username))
                throw new AppException("Username \"" + user.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        /// <summary>
        /// This method updates the information of a specific user and adds it into the database. 
        /// </summary>
        ///<param name="user">The new user information of the user that is being updated.</param>
        ///<param name="password">The new password of the user that is being updated.</param>
        ///<returns>
        /// A **User** class which contains the new user information.
        /// It returns an exception if the user data is null or if the user who is updating doesn't exist in the database.
        /// It also throws exception if the new username is already taken by someone else. 
        /// </returns>
        public void Update(User userValues, string password = null)
        {
            var user = _context.Users.Find(userValues.Id);

            if (user == null)
                throw new AppException("User not found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userValues.Username) && userValues.Username != user.Username)
            {
                // throw error if the new username is already taken
                if (_context.Users.Any(x => x.Username == userValues.Username))
                    throw new AppException("Username " + userValues.Username + " is already taken");

                user.Username = userValues.Username;
            }

            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userValues.FirstName))
                user.FirstName = userValues.FirstName;

            if (!string.IsNullOrWhiteSpace(userValues.LastName))
                user.LastName = userValues.LastName;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            //update if Gender is provided
            if (!string.IsNullOrWhiteSpace(userValues.Gender))
                user.Gender = userValues.Gender;

            //update if Preferred is provided
            if (!string.IsNullOrWhiteSpace(userValues.PreferredPlatform))
                user.PreferredPlatform = userValues.PreferredPlatform;


            _context.Users.Update(user);
            _context.SaveChanges();
        }

        /// <summary>
        /// This method deletes the information of a specific user from the database. 
        /// </summary>
        ///<param name="id">The id of the user to delete.</param>
        ///<returns>
        /// void
        /// </returns>
        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// One of the private helper methods which creates a password hash.
        /// </summary>
        ///<param name="passwordHash">A reference type variable passed in the form of **Byte** which is the result hash generated.</param>
        ///<param name="passwordSalt">A reference type variable passed in the form of **Byte** for the salt of the password.</param>
        ///<param name="password">The new password of the user that is being encrypted.</param>
        ///<returns>
        ///void
        /// </returns>
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// One of the private helper methods which verifies a password hash.
        /// </summary>
        ///<param name="passwordHash">A reference type variable passed in the form of **Byte** which is the result hash generated.</param>
        ///<param name="passwordSalt">A reference type variable passed in the form of **Byte** which is the salt of the password.</param>
        ///<param name="password">The password of the user that is being decrypted.</param>
        ///<returns>
        ///void
        /// </returns>
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// It analyses the data from the database and calculates the Male to Female Ratio.
        /// </summary>
        /// <returns>
        /// A **DemographicModel** class which contains the values for the results generated by the method.
        /// </returns>
        public DemographicModel GetMaleFemaleRatio() 
        {   
            float m = 0;
            float n = 0;
            float o = 0;
            float malePercentage;
            float femalePercentage;
            float otherPercentage;

            List<GenderRatio> genders = _context.Users.Select(p=> new GenderRatio {
                Genders = p.Gender,
            }).ToList();
            
            foreach(var gender in genders)
            {   
                if (gender.Genders == "Male")
                {
                    m = m + 1;
                }
                else if(gender.Genders == "Female")
                {
                    n = n + 1;
                }
                else if(gender.Genders == "Other")
                {
                    o = o + 1;
                }
            }
            
            float total = m + n + o;
            malePercentage = (m/total)*100;
            femalePercentage = (n/total)*100;
            otherPercentage = (o/total)*100;

            var demographic = new DemographicModel {
                malePercentage = malePercentage,
                femalePercentage = femalePercentage,
                otherPercentage = otherPercentage,
                totalUsers = total
                };

            return demographic;

        }

        /// <summary>
        /// It analyses the data from the database and calculates the platform preferences of the user.
        /// </summary>
        /// <returns>
        /// A **PlatformModel** class which contains the values for the results generated by the method.
        /// </returns>
        public PlatformModel GetPlatform()
        {   
            float ps4Count = 0;
            float xboxCount = 0;
            float switchCount = 0;
            float pcCount = 0;
            float mobileCount = 0;

            float ps4Percentage;
            float xboxPercentage;
            float switchPercentage;
            float pcPercentage;
            float mobilePercentage;

            List<PlatformPreference> platforms = _context.Users.Select(p=> new PlatformPreference {
                Platform = p.PreferredPlatform,
            }).ToList();

            foreach(var platform in platforms)
            {
                if (platform.Platform == "PlayStation")
                {
                    ps4Count = ps4Count + 1;
                }
                else if (platform.Platform == "XBOX")
                {
                    xboxCount = xboxCount + 1;
                }
                else if (platform.Platform == "PC")
                {
                    pcCount = pcCount + 1;
                }
                else if (platform.Platform == "Switch")
                {
                    switchCount = switchCount + 1;
                }
                else if (platform.Platform == "Mobile")
                {
                    mobileCount = mobileCount + 1;
                }
            }

            float total = ps4Count + xboxCount + pcCount + switchCount + mobileCount;
            ps4Percentage = (ps4Count/total)*100;
            xboxPercentage = (xboxCount/total)*100;
            pcPercentage = (pcCount/total)*100;
            switchPercentage = (switchCount/total)*100;
            mobilePercentage = (mobileCount/total)*100;

            var preferredplatform = new PlatformModel {
                ps4Percentage = ps4Percentage,
                xboxPercentage = xboxPercentage,
                pcPercentage = pcPercentage,
                switchPercentage = switchPercentage,
                mobilePercentage = mobilePercentage
                };

            return preferredplatform;

        }
        


    
    }
}