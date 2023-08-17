using AutoMapper;
using CourseManagementProject.Application.Repositories;
using CourseManagementProject.Application.Services;
using CourseManagementProject.Domain;
using CourseManagementProject.Domain.DomainModels;
using CourseManagementProject.Domain.Enums;
using CourseManagementProject.Domain.Input;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementProject.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ITeachersRepository _teachersRepository;
    private readonly IStudentRepository _studentRepository;

    private readonly ITokenGenerator _tokenGenerator;
    private readonly IPasswordHandler _passwordHandler;

    public readonly IMapper _mapper;
    private readonly ILogger _logger;

    public AuthenticationController(ITeachersRepository teachersRepository, IStudentRepository studentRepository,
        ITokenGenerator tokenGenerator, IPasswordHandler passwordHandler, IMapper mapper, ILogger<AuthenticationController> logger)
    {
        _teachersRepository = teachersRepository;
        _studentRepository = studentRepository;
        _tokenGenerator = tokenGenerator;
        _passwordHandler = passwordHandler;
        _mapper = mapper;
        _logger = logger;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel login)
    {
        _logger.LogTrace($"Incoming request to login {login.Username}");

        User user = await GetUser(login.Username, login.Role);

        if (user == null)
        {
            _logger.LogInformation($"User not found, username: {login.Username}, role: {login.Role}");
            return NotFound();
        }
            

        if(_passwordHandler.VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
            return BadRequest("Incorrect Credentials");


        var token = _tokenGenerator.GenerateToken(user);

        return Ok(token);
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel register)
    {
        _logger.LogTrace($"Incoming request to register user {register.Username}");

        User user = await GetUser(register.Username, register.UserRole);

        if(user != null)
        {
            _logger.LogInformation($"User not Register user, username: {register.Username}, role: {register.UserRole}");
            return BadRequest("User Already Registered");
        }
            


        _passwordHandler.CreateSaltAndHash(register.Password, out var passwordHash, out var passwordSalt);

        switch (register.UserRole)
        {
            case UserRole.Tutor:
                user = new Tutor { PasswordHash = passwordHash, PasswordSalt = passwordSalt, Subject = register.Subject, Username = register.Username, UserRole = UserRole.Tutor };
                await _teachersRepository.RegisterTeacher(user);
                break;
            case UserRole.Student:
                await _studentRepository.RegisterStudent(user);
                break;
        }

        var token = _tokenGenerator.GenerateToken(user);

        return Ok(token);
    }





    private async Task<User> GetUser(string username, UserRole role) 
    {
        User user = null;

        if(role == UserRole.Student)
        {
            var studentResult = await _studentRepository.GetStudentByUsername(username);
            user = studentResult.Data;
        }
        else
        {
            var tutorResult = await _teachersRepository.GetTeacherByUsername(username);
            user = tutorResult.Data;
        }

        return user;
    }
}
