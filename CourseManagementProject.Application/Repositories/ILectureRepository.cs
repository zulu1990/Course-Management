namespace CourseManagementProject.Application.Repositories;

public interface ILectureRepository
{
    Task CreateNewLecture();

    Task UpdateLectureAttendance();

    Task FinishLecture();

}
