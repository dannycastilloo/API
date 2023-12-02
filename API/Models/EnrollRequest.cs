namespace API.Models
{
    public class EnrollRequest
    {
        public int IdStudent { get; set; }
        public List<Course>? Courses { get; set; }
    }
}
