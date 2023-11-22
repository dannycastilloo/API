namespace API.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public DateTime Datetime { get; set; }

        // Llaves foráneas
        public Student? Student { get; set; }
        public int StudentId { get; set; }

        public Course? Course { get; set; }
        public int CourseId { get; set; }
    }
}
