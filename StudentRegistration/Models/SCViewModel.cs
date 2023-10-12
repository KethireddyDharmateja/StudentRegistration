using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.Models
{
    public class SCViewModel
    {
        [Display(Name = "Student Name")]
        public Students? Students { get; set; }
        [Display(Name = "Course Name")]
        public Course? Courses { get; set; }
    }
}
