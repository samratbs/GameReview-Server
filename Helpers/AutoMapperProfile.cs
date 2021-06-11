using AutoMapper;
using WebApi.Entities;
using WebApi.Models.Users;
using WebApi.Models.Feedbacks;

namespace WebApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
            CreateMap<CreateModel, Feedback>();
            CreateMap<Feedback, FeedbackModel>();
        }
    }
}