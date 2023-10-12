namespace StudentRegistration.Models
{
    public class StudentCourse
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string? StudentName { get; set; }
        public Guid CourseId { get; set; }
        public string? CourseName { get; set; }
    }
}
