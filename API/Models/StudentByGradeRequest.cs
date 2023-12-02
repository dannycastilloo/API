namespace API.Models
{
    public class StudentByGradeRequest
    {
        public int IdGrade { get; set; }
        public List<Student>? Students { get; set; }
    }
}
