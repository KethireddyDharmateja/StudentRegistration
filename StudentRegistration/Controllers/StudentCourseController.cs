using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentRegistration.Data;
using StudentRegistration.Models;

namespace StudentRegistration.Controllers
{
    public class StudentCourseController : Controller
    {
        private readonly StudentDbContext studentDbContext;

        public StudentCourseController(StudentDbContext studentDbContext)
        {
            this.studentDbContext = studentDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var studentCourses = await studentDbContext.StudentCourses.ToListAsync();
            return View(studentCourses);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Students = new SelectList(studentDbContext.Students, "Id", "Name");
            ViewBag.Courses = new SelectList(studentDbContext.Courses, "Id", "CourseName");

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Add(SCViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var selectedStudent = studentDbContext.Students.FirstOrDefault(s => s.Id == viewModel.Students.Id);
                var selectedCourse = studentDbContext.Courses.FirstOrDefault(c => c.Id == viewModel.Courses.Id);

                if (selectedStudent != null && selectedCourse != null)
                {
                    var studentCourse = new StudentCourse
                    {
                        StudentId = selectedStudent.Id,
                        StudentName = selectedStudent.Name,
                        CourseId = selectedCourse.Id,
                        CourseName = selectedCourse.CourseName
                    };

                    await studentDbContext.StudentCourses.AddAsync(studentCourse);
                    await studentDbContext.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }

            ViewBag.Students = new SelectList(studentDbContext.Students, "Id", "Name");
            ViewBag.Courses = new SelectList(studentDbContext.Courses, "Id", "CourseName");
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult AddList()
        {
            ViewBag.Students = new SelectList(studentDbContext.Students, "Id", "Name");
            ViewBag.Courses = new SelectList(studentDbContext.Courses, "Id", "CourseName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddList(ListSCViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var selectedStudent = studentDbContext.Students.FirstOrDefault(s => s.Id == viewModel.Students.Id);

                if (selectedStudent != null)
                {
                    // Create a new StudentCourse object for each selected course
                    foreach (var courseId in viewModel.SelectedCourseIds)
                    {
                        var selectedCourse = studentDbContext.Courses.FirstOrDefault(c => c.Id == courseId);

                        if (selectedCourse != null)
                        {
                            var studentCourse = new StudentCourse
                            {
                                StudentId = selectedStudent.Id,
                                StudentName = selectedStudent.Name,
                                CourseId = selectedCourse.Id,
                                CourseName = selectedCourse.CourseName
                            };

                            await studentDbContext.StudentCourses.AddAsync(studentCourse);
                        }
                    }

                    // Save changes after adding all selected courses
                    await studentDbContext.SaveChangesAsync();

                    return RedirectToAction("Index"); // Redirect to a different action
                }
            }

            // If the model is not valid, return to the Add view
            ViewBag.Students = new SelectList(studentDbContext.Students, "Id", "Name");
            ViewBag.Courses = new MultiSelectList(studentDbContext.Courses, "Id", "CourseName", viewModel.SelectedCourseIds);
            return View(viewModel);
        }

    }
}
