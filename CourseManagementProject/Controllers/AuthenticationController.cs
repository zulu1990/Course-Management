using AutoMapper;
using CourseManagementProject.Application.Repositories;
using CourseManagementProject.Application.Services;
using CourseManagementProject.Domain;
using CourseManagementProject.Domain.DomainModels;
using CourseManagementProject.Domain.Enums;
using CourseManagementProject.Domain.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.Win32;

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


    public AuthenticationController(ITeachersRepository teachersRepository, IStudentRepository studentRepository, 
        ITokenGenerator tokenGenerator, IPasswordHandler passwordHandler, IMapper mapper)
    {
        _teachersRepository = teachersRepository;
        _studentRepository = studentRepository;
        _tokenGenerator = tokenGenerator;
        _passwordHandler = passwordHandler;
        _mapper = mapper;
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel login)
    {
        User user = await GetUser(login.Username, login.Role);

        if (user == null)
            return NotFound();

        if(_passwordHandler.VerifyPasswordHash(login.Password, user.PasswordHash, user.PasswordSalt))
            return BadRequest("Incorrect Credentials");


        var token = _tokenGenerator.GenerateToken(user);

        return Ok(token);
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterModel register)
    {
        User user = await GetUser(register.Username, register.UserRole);

        if(user != null)
            return BadRequest("User Already Registered");


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
        Result<User> userResult = null;



        userResult = (role) switch
        {
            UserRole.Student => await _studentRepository.GetStudentByUsername(username),
            UserRole.Tutor => await _teachersRepository.GetTeacherByUsername(username),
            _ => null
        };

        return userResult.Data;
    }
}
