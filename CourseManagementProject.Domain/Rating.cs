namespace CourseManagementProject.Domain;

public class Rating
{
    public int LectureId { get; set; }


    public int StudentId { get; set; }
    public int Rate { get; set; }

    public string Comment { get; set; }
}
