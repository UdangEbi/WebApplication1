using AutoMapper;
using WebApplication1.DTO; // ganti sesuai namespace project kamu
using WebApplication1.Models; // ganti sesuai namespace

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Course, CourseDTO>();
        CreateMap<Category, CategoryDTO>();
        CreateMap<Instructor, InstructorDTO>();
        CreateMap<CourseAddDTO, Course>();
    }
}
