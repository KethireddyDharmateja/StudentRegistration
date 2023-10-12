using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRegistration.Data;
using StudentRegistration.Models;

namespace StudentRegistration.Controllers
{
    public class CourseController : Controller
    {
        private readonly StudentDbContext studentDbContext;

        public CourseController(StudentDbContext studentDbContext)
        {
            this.studentDbContext = studentDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var courses = await studentDbContext.Courses.ToListAsync();
            return View(courses);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCourseViewModel addCourseViewModel)
        {
            var course = new Course()
            {
                Id = Guid.NewGuid(),
                CourseName = addCourseViewModel.CourseName,
                CourseCode = addCourseViewModel.CourseCode
            };

            await studentDbContext.Courses.AddAsync(course);
            await studentDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var course = await studentDbContext.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course != null)
            {
                var courseViewModel = new UpdateCourseViewModel()
                {
                    Id = course.Id,
                    CourseName = course.CourseName,
                    CourseCode = course.CourseCode
                };
                return await Task.Run(() => View("View", courseViewModel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateCourseViewModel updateCourseViewModel)
        {
            var course = await studentDbContext.Courses.FindAsync(updateCourseViewModel.Id);
            if (course != null)
            {
                course.Id = updateCourseViewModel.Id;
                course.CourseName = updateCourseViewModel.CourseName;
                course.CourseCode = updateCourseViewModel.CourseCode;

                await studentDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateCourseViewModel updateCourseViewModel)
        {
            var course = await studentDbContext.Courses.FindAsync(updateCourseViewModel.Id);
            if (course != null)
            {
                studentDbContext.Courses.Remove(course);
                await studentDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
