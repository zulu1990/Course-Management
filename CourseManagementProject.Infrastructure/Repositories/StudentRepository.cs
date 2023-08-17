using CourseManagementProject.Application.Repositories;
using CourseManagementProject.Domain;
using CourseManagementProject.Domain.DomainModels;
using CourseManagementProject.Domain.Enums;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CourseManagementProject.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;

        public StudentRepository(IConfiguration configuration, ILogger<StudentRepository> logger)
        {
            _logger = logger;

            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        public async Task<Result<Student>> GetStudentByUsername(string username)
        {
            try
            {
                _logger.LogTrace($"getting student by username: {username}");

                using IDbConnection connection = new SqlConnection(_connectionString);
                var par = new DynamicParameters();

                par.Add("@username", username);
                var student = await connection.QueryFirstAsync<Student>("get_student_by_username", par, commandType: CommandType.StoredProcedure);

                if (student == null)
                {
                    _logger.LogInformation($"student not found {username}");
                    return Result<Student>.Fail(404, "Not Found");
                }

                _logger.LogTrace($"done getting user by username: {username}");
                return Result<Student>.Succeed(student);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"exception on username: {username}");
               return Result<Student>.Fail(500, ex.Message);
            }
        }

        public Task<Result> RegisterStudent(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UpadateStudentSubjects(List<Subject> subjects, int id)
        {
            try
            {
                _logger.LogTrace($"Updating Student Subjects {id}, {JsonSerializer.Serialize(subjects)}");
                using IDbConnection connection = new SqlConnection(_connectionString);
                var par = new DynamicParameters();
                par.Add("@subjcets", subjects);
                par.Add("@id", id);

                await connection.ExecuteAsync("update_student_subjects", par, commandType: CommandType.StoredProcedure);
                return Result.Succeed();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on UpadateStudentSubjects, {id}, {JsonSerializer.Serialize(subjects)}");
                return Result.Fail(500, ex.Message);
            }
        }

    }
}
