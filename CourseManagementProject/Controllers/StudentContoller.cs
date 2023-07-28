using CourseManagementProject.Application.Repositories;
using CourseManagementProject.Application.Services;
using CourseManagementProject.Domain.DomainModels;
using CourseManagementProject.Domain.Enums;
using CourseManagementProject.Domain.Input.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentContoller : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHandler _passwordHandler;

        public StudentContoller(IStudentRepository studentRepository, IConfiguration configuration, IPasswordHandler passwordHandler)
        {
            _studentRepository = studentRepository;
            _configuration = configuration;
            _passwordHandler = passwordHandler;
        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateStudent(CreateStudentModel createStudentModel)
        {
            var studentResult = await _studentRepository.GetStudentByUsername(createStudentModel.Username);

            if(studentResult.Success == false)
            {
                return BadRequest(studentResult.ErrorMessage);
            }

            var student = studentResult.Data;

            if(student == null)
            {
                _passwordHandler.CreateSaltAndHash(_configuration.GetSection("Defaults:StudentPassword").Value, out var passwordHash, out var passwordSalt);

                student = new Student
                {
                    Subjects = new List<Subject> { createStudentModel.Subject},
                    Username = createStudentModel.Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    UserRole = UserRole.Student
                };

                var registerResult = await _studentRepository.RegisterStudent(student);
                if(registerResult.Success == false)
                {
                    return BadRequest("Cant create Student");
                }


                return Ok(student);
            }

            student.Subjects.Add(createStudentModel.Subject);

            var updateResult = await _studentRepository.UpadateStudentSubjects(student.Subjects, student.Id);

            if(updateResult.Success == false)
                return BadRequest("Couldnt add user new subject");

            return Ok(student);
        }
    }
}
