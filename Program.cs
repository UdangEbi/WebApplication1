using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using WebApplication1;
using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.Models;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//add ef core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Dependency Injection
builder.Services.AddScoped<ICategory, CategoryEF>();
builder.Services.AddScoped<ICourse, CourseEF>();
builder.Services.AddScoped<IInstructor, InstructorEF>();
builder.Services.AddScoped<IAspUser, AspUserEF>();
// builder.Services.AddSingleton<IInstructor, InstructorADO>();
// builder.Services.AddSingleton<ICourse, CourseADO>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

app.UseHttpsRedirection();
app.MapGet("api/v1/cekpassword/{password}", (string password) =>
{
    var pass = WebApplication1.Helpers.HashHelper.HashPassword(password);
    return Results.Ok($"Password : {password} Hash: {pass}");
});

app.MapPost("api/v1/register", (IAspUser aspUserData, AspUser user) =>
{
    var newUser = aspUserData.RegisterUser(user);
    return Results.Created($"/api/v1/users/{newUser.Username}", newUser);
});

app.MapPost("api/v1/login", (IAspUser aspUserData, AspUser loginData) =>
{
    bool isValid = aspUserData.Login(loginData.Username, loginData.Password);
    if (!isValid)
    {
        return Results.Unauthorized();
    }

    return Results.Ok(new
    {
        message = "Login successful",
        username = loginData.Username
    });
});


// app.MapGet("api/v1/helloservices", (string id)=> 
// {
//     return $"Hello ASP Web API : id ={id}!";
// });  

// app.MapGet("api/v1/helloservices/{name}", (string name)=> $"Hello {name}!");

// app.MapGet("api/v1/luas-segitiga/", (double alas, double tinggi)=> 
// {
//     double luas = 0.5 * alas * tinggi;
//     return $"Luas segitiga dengan alas = {alas} dan tinggi = {tinggi} adalah {luas}";
// });

// app.MapGet("api/v1/categories", (ICategory categoryData)=>
// {
//     var categories = categoryData.GetCategories();
//     return categories;
// });

// app.MapGet("api/v1/categories/{id}", (ICategory categoryData, int id) =>
// {
//     var category = categoryData.GetCategoryById(id);
//     return category;
// });

// app.MapPost("api/v1/categories", (ICategory categoryData, Category category) =>
// {
//     var newCategory = categoryData.AddCategory(category);
//     return newCategory;
// });

// app.MapPut("api/v1/categories", (ICategory categoryData, Category category) =>
// {
//     var updatedCategory = categoryData.UpdateCategory(category);
//     return updatedCategory;
// });
// app.MapDelete("api/v1/categories/{id}", (ICategory categoryData, int id) =>
// {
//     categoryData.DeleteCategory(id);
//     return Results.NoContent();
// });

// app.MapGet("api/v1/instructors", (IInstructor instructorData)=>
// {
//     var instructors = instructorData.GetInstructor();
//     return instructors;
// });

// app.MapGet("api/v1/instructors/{id}", (IInstructor instructorData, int id) =>
// {
//     var instructor = instructorData.GetInstructorById(id);
//     return instructor;
// });

// app.MapPost("api/v1/instructors", (IInstructor instructorData, Instructor instructor) =>
// {
//     var newInstructor = instructorData.AddInstructor(instructor);
//     return newInstructor;
// });

// app.MapPut("api/v1/instructors", (IInstructor instructorData, Instructor instructor) =>
// {
//     var updatedInstructor = instructorData.UpdateInstructor(instructor);
//     return updatedInstructor;
// });
// app.MapDelete("api/v1/instructors/{id}", (IInstructor instructorData, int id) =>
// {
//     instructorData.DeleteInstructor(id);
//     return Results.NoContent();
// });

// app.MapGet("api/v1/courses", (ICourse courseData, IMapper mapper)=>
// {
//     var courses = courseData.GetAllCourses();
//     var courseDTOs = mapper.Map<IEnumerable<CourseDTO>>(courses);
//     return Results.Ok(courseDTOs);
// });

// app.MapGet("api/v1/courses/{id}", (ICourse courseData, IMapper mapper, int id) =>
// {
//     var course = courseData.GetCourseByIdCourse(id);
//     if (course == null) return Results.NotFound();

//     var courseDTO = mapper.Map<CourseDTO>(course);
//     return Results.Ok(courseDTO);
// });

// app.MapPost("api/v1/courses", (ICourse courseData, IMapper mapper, CourseAddDTO courseAddDTO) =>
// {
//     var course = mapper.Map<Course>(courseAddDTO);
//     var added = courseData.AddCourse(course);
//     var courseDTO = mapper.Map<CourseDTO>(added);
//     return Results.Ok(courseDTO); 
// });

// app.MapPut("api/v1/courses", (ICourse courseData, IMapper mapper, Course course) =>
// {
//     var existingCourse = courseData.GetCourseByIdCourse(course.CourseId);
//     if (existingCourse == null)
//         return Results.NotFound();

//     existingCourse.CourseName = course.CourseName;
//     existingCourse.CourseDescription = course.CourseDescription;
//     existingCourse.Duration = course.Duration;
//     existingCourse.CategoryId = course.CategoryId;
//     existingCourse.InstructorId = course.InstructorId;

//     var updated = courseData.UpdateCourse(existingCourse);
//     var courseDTO = mapper.Map<CourseDTO>(updated);
//     return Results.Ok(courseDTO);
// });
// app.MapDelete("api/v1/courses/{id}", (ICourse courseData, int id) =>
// {
//     try
//     {
//         courseData.DeleteCourse(id);
//         return Results.NoContent();
//     }
//     catch (Exception ex)
//     {
//         return Results.Problem(ex.Message);
//     }
// });

app.Run();

