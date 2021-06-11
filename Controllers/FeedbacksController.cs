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
using WebApi.Models.Feedbacks;

namespace WebApi.Controllers
{
    
    ///<summary>
    ///The class that acts as the entry point of the application for all the feedback api endpoints.
    ///</summary>
    [ApiController]
    [Route("feedbacks")]
    public class FeedbacksController: ControllerBase

    {
        private IFeedbackService _feedbackService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public FeedbacksController(
            IFeedbackService feedbackService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _feedbackService = feedbackService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        ///<summary>
        ///Api endpoint responsible for creating a feedback.
        ///</summary>
        ///<param name="model">The parameter used to receive data for creating the feedback as **CreateModel** </param>
        ///<returns>
        ///The corresponding result.
        /// </returns>
        [HttpPost("create")]
        public IActionResult CreateFeedback([FromBody]CreateModel model)
        {
            // map model to entity
            var feedback = _mapper.Map<Feedback>(model);

            try
            {
                // create feedbacks
                _feedbackService.Create(feedback);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        ///<summary>
        ///Api endpoint responsible for fetching all feedbacks.
        ///</summary>
        ///<returns>
        ///The corresponding result.
        /// </returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var feedbacks = _feedbackService.GetAll();
            var model = _mapper.Map<IList<FeedbackModel>>(feedbacks);
            return Ok(model);
        }

        ///<summary>
        ///Api endpoint responsible for fetching a specific feedback.
        ///</summary>
        ///<param name="id">The user reference used to fetch the feedback in **int**.</param>
        ///<returns>
        ///The corresponding result.
        /// </returns>
        [HttpGet("{id}")]
        public IActionResult GetByRef(int id)
        {
            var user = _feedbackService.GetByRef(id);
            var model = _mapper.Map<FeedbackModel>(user);
            return Ok(model);
        }

        ///<summary>
        ///Api endpoint responsible for fetching the average calculated ratings.
        ///</summary>
        ///<returns>
        ///The corresponding result.
        /// </returns>
        [HttpGet("avg")]
        public IActionResult GetAverageRatings() 
        {
            var feedbacks = _feedbackService.GetAverageRatings();
            var model = _mapper.Map<RatingModel>(feedbacks);
            return Ok(model);
        }

    }

}