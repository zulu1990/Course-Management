using CourseManagementProject.Application.Repositories;
using CourseManagementProject.Application.Services;
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


    public AuthenticationController(ITeachersRepository teachersRepository, IStudentRepository studentRepository, 
        ITokenGenerator tokenGenerator, IPasswordHandler passwordHandler)
    {
        _teachersRepository = teachersRepository;
        _studentRepository = studentRepository;
        _tokenGenerator = tokenGenerator;
        _passwordHandler = passwordHandler;
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
        User user = await GetUser(register.Username, register.Role);

        if(user != null)
            return BadRequest("User Already Registered");

        _passwordHandler.CreateSaltAndHash(register.Password, out var passwordHash, out var passwordSalt);
        user.PasswordSalt = passwordSalt;
        user.PasswordHash = passwordHash;

        switch (register.Role)
        {
            case UserRole.Tutor:
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

        user = (role) switch
        {
            UserRole.Student => await _studentRepository.GetStudentByUsername(username),
            UserRole.Tutor => await _teachersRepository.GetTeacherByUsername(username),
            _ => null
        };

        return user;
    }
}
