using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentRegistration.Data;
using StudentRegistration.Models;

namespace StudentRegistration.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentDbContext studentDbContext;

        public StudentsController(StudentDbContext studentDbContext)
        {
            this.studentDbContext = studentDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var students = await studentDbContext.Students.ToListAsync();
            return View(students);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddStudentsViewModel addStudentsViewModel)
        {
            var student = new Students()
            {
                Id = Guid.NewGuid(),
                Name = addStudentsViewModel.Name,
                Marks = addStudentsViewModel.Marks
            };

            await studentDbContext.Students.AddAsync(student);
            await studentDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var student = await studentDbContext.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (student != null)
            {
                var studentViewModel = new UpdateStudentViewModel()
                {
                    Id = student.Id,
                    Name = student.Name,
                    Marks = student.Marks
                };
                return await Task.Run(() => View("View",studentViewModel));
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateStudentViewModel updateStudentViewModel)
        {
            var student = await studentDbContext.Students.FindAsync(updateStudentViewModel.Id);
            if (student != null)
            {
                student.Id = updateStudentViewModel.Id;
                student.Name = updateStudentViewModel.Name;
                student.Marks = updateStudentViewModel.Marks;

                await studentDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateStudentViewModel updateStudentViewModel)
        {
            var student = await studentDbContext.Students.FindAsync(updateStudentViewModel.Id);
            if (student != null)
            {
                studentDbContext.Students.Remove(student);
                await studentDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

    }
}
