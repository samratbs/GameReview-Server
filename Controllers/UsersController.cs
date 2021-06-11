using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;
using WebApi.Models.Users;
using WebApi.Models.Feedbacks;

namespace WebApi.Controllers
{   

    ///<summary>
    ///The class that acts as the entry point of the application for all the feedback api endpoints.
    ///</summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        ///<summary>
        ///Api endpoint responsible for login.
        ///</summary>
        ///<param name="login">The parameter used to receive data for logging in as **LoginModel**.</param>
        ///<returns>
        ///The corresponding result.
        /// </returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginModel login)
        {
            var user = _userService.Login(login.Username, login.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        ///<summary>
        ///Api endpoint responsible for registering a user.
        ///</summary>
        ///<param name="register">The parameter used to receive data for registration as **RegisterModel**.</param>
        ///<returns>
        ///The corresponding result.
        /// </returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterModel register)
        {
            // map model to entity
            var user = _mapper.Map<User>(register);

            try
            {
                // create a new user
                _userService.Register(user, register.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        ///<summary>
        ///Api endpoint responsible for fetching all the users.
        ///</summary>
        ///<returns>
        ///The corresponding result.
        /// </returns>
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }

        ///<summary>
        ///Api endpoint responsible for fetching a specific user.
        ///</summary>
        ///<param name="id">The user reference used to fetch the user data.</param>
        ///<returns>
        ///The corresponding result.
        /// </returns>
        [HttpGet("{id}")]
        public IActionResult GetUsersById(int id)
        {
            var user = _userService.GetUsersById(id);
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        ///<summary>
        ///Api endpoint responsible for updating a user data.
        ///</summary>
        ///<param name="id">The user reference used to determine the user to update.</param>
        ///<param name="model">The parameter used to receive data for updating the user data as **UpdateModel**.</param>
        ///<returns>
        ///The corresponding result.
        /// </returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]UpdateModel model)
        {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = id;

            try
            {
                // update the user using the post data
                _userService.Update(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        ///<summary>
        ///Api endpoint responsible for deleting a user data.
        ///</summary>
        ///<param name="id">The user reference used to determine the user to delete.</param>
        ///<returns>
        ///The corresponding result.
        /// </returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }

        ///<summary>
        ///Api endpoint responsible for fetching the calculated male to female ratio.
        ///</summary>
        ///<returns>
        ///The corresponding result.
        /// </returns>
        [HttpGet("ratio")]
        public IActionResult GetMaleFemaleRatio()
        {
            var ratio = _userService.GetMaleFemaleRatio();
            var model = _mapper.Map<DemographicModel>(ratio);
            return Ok(model);
        }

        ///<summary>
        ///Api endpoint responsible for fetching the preffered user platforms after calculation.
        ///</summary>
        ///<returns>
        ///The corresponding result.
        /// </returns>
        [HttpGet("platform")]
        public IActionResult GetPlatformRatio()
        {
            var preference = _userService.GetPlatform();
            var model = _mapper.Map<PlatformModel>(preference);
            return Ok(model);
        }
    }
}
