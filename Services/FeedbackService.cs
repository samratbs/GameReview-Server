using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Feedbacks;

namespace WebApi.Services
{   
    ///<summary>
    ///**FeedbackService** class is implemented using this **IFeedbackService** interface for abstraction
    ///This Service handles all the backend logic for the **Feedback** endpoints with the database.
    ///</summary>
    public interface IFeedbackService
    {

        /// <returns>
        /// An **IEnumerable** list of the Feedback data.
        /// </returns>
        IEnumerable<Feedback> GetAll();

        /// <param name="id"> The id of the user whose feedback is to be retrieved.</param>
        /// <returns>
        /// A **Feedback** class which contains values of the feedback that was retrieved from the database.
        /// It returns an exception if the feedback is not found in the database.
        /// </returns>
        Feedback GetByRef(int id);

        ///<param name="feedback">The Feedback Entity class which has the required values to be added to the database.</param>
        ///<returns>
        /// A **Feedback** class which contains values of the data that was just added into the database.
        /// It returns an exception if the user has already added a feedback.
        /// </returns>
        Feedback Create(Feedback feedback);

        /// <returns>
        /// A RatingModel class which has values for the average rating and positive/negative feedback percentages.
        /// </returns>
        RatingModel GetAverageRatings();
        
    }

    ///<summary>
    ///The class that is responsible for all the server side logic of the **Feedback** api endpoints.
    ///It ensures the usage of **DbContext** which maintains a session with the database allowing us to 
    ///query or save instances of the entities.
    ///</summary>
    public class FeedbackService : IFeedbackService
    {   
        private DataContext _context;

        /// <summary>
        /// Constructor used for initialization.
        /// </summary>
        /// <param name="context">The variable used to represent the DbContext which communicates with the database.</param>
        public FeedbackService(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a feedback and adds it into the database. It also checks if the user has already created a feedback
        /// or not. If they have, the feedback is not added into the database.
        /// </summary>
        ///<param name="feedback">The Feedback Entity class which has the required values to be added to the database.</param>
        ///<returns>
        /// A **Feedback** class which contains values of the data that was just added into the database.
        /// It returns an exception if the user has already added a feedback.
        /// </returns>
        public Feedback Create(Feedback feedback)
        {
            // validation
            if (_context.Feedbacks.Any(x => x.UserRef == feedback.UserRef))
                throw new AppException("Feedback is already taken");

            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();

            return feedback;
        }

        /// <summary>
        /// Fetches all the feedbacks provided by the user from the database.
        /// </summary>
        /// <returns>
        /// An **IEnumerable** list of the Feedback data.
        /// </returns>
        public IEnumerable<Feedback> GetAll()
        {
            return _context.Feedbacks;
        }

        /// <summary>
        /// Fetches a specific feedback based on the provided User id from the database.
        /// </summary>
        /// <param name="id"> The id of the user whose feedback is to be retrieved.</param>
        /// <returns>
        /// A **Feedback** class which contains values of the feedback that was retrieved from the database.
        /// It returns an exception if the feedback is not found in the database.
        /// </returns>
        public Feedback GetByRef(int id)
        {
            var singlefeedback = _context.Feedbacks.FirstOrDefault(x => x.UserRef == id);
            if (singlefeedback == null)
                throw new AppException("Feedback not added yet");
            return singlefeedback;
        }

        /// <summary>
        /// It takes all the ratings in the database and calculate its average. It also determines how positive or negative the average
        /// feedbacks were.
        /// </summary>
        /// <returns>
        /// A RatingModel class which has values for the average rating and positive/negative feedback percentages.
        /// </returns>
        public RatingModel GetAverageRatings() 
        {   
            float avg = 0;
            int n = 0;
            float positive = 0;
            float negative = 0;
            float positivePercentage = 0;
            float negativePercentage = 0;

            List<Rating> rates = _context.Feedbacks.Select(p=> new Rating {
                Ratings = p.Ratings,
                FeedbackType = p.Type,
            }).ToList();

            foreach(var rate in rates)
            {   
                avg = avg + rate.Ratings;     
                n=n+1;

                if (rate.FeedbackType == "Positive" || rate.FeedbackType == "Semi Positive")
                {
                    positive = positive + 1;
                }
                else if (rate.FeedbackType == "Negative")
                {
                    negative = negative + 1;
                }
                
            }

            float totalfeedback =  positive + negative;
            positivePercentage = (positive/totalfeedback)*100;
            negativePercentage = 100 - positivePercentage;
            avg = avg/n;
            
            var Average = new RatingModel {
                Rate = avg,
                positivePercentage = positivePercentage,
                negativePercentage = negativePercentage,
                totalFeedbacks = totalfeedback
                };
            return Average;

        }

    
    }

}