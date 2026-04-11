using AutoMapper;
using StructureCodeSolution.Application.Usecases.V1.Queries.Courses.Abstracts;
using StructureCodeSolution.Domain.Aggregates.Courses;

namespace StructureCodeSolution.Application.Usecases.V1.Queries.Courses.Mappings
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
            // Course -> CourseResponse
            CreateMap<Course, Response.CourseResponse>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency))
                .ForMember(dest => dest.StarRating, opt => opt.MapFrom(src => src.Rating.StarRating))
                .ForMember(dest => dest.NumberOfRatings, opt => opt.MapFrom(src => src.Rating.NumberOfRatings))
                .ForMember(dest => dest.NumberOfStudents, opt => opt.MapFrom(src => src.Statistics.NumberOfStudents))
                .ForMember(dest => dest.NumberOfComments, opt => opt.MapFrom(src => src.Statistics.NumberOfComments))
                .ForMember(dest => dest.NumberOfLessons, opt => opt.MapFrom(src => src.Statistics.NumberOfLessons))
                .ForMember(dest => dest.NumberOfHours, opt => opt.MapFrom(src => src.Statistics.TotalHours))
                .ForMember(dest => dest.Videos, opt => opt.MapFrom(src => src.Videos.OrderBy(v => v.Order)));

            // Video -> VideoResponse
            CreateMap<Video, Response.VideoResponse>();
        }
    }
}
