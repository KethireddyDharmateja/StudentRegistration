using System.ComponentModel.DataAnnotations;

namespace StudentRegistration.Models
{
    public class ListSCViewModel
    {
        public Students? Students { get; set; }
        public List<Guid>? SelectedCourseIds { get; set; }
    }
}
