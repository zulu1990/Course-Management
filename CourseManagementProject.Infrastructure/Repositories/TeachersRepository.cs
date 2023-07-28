using CourseManagementProject.Application.Repositories;
using CourseManagementProject.Domain;
using CourseManagementProject.Domain.DomainModels;
using CourseManagementProject.Domain.Enums;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagementProject.Infrastructure.Repositories;

public class TeachersRepository : ITeachersRepository
{
    private readonly string _connectionString;

    public TeachersRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SqlConnection");
    }


    public async Task<Result<User>> GetTeacherByUsername(string username)
    {
        try
        {
            using IDbConnection connection = new SqlConnection(_connectionString);

            var par = new DynamicParameters();
            par.Add("@username", username);

            var user = await connection.QueryFirstOrDefaultAsync<Tutor>("[dbo].[GetTeacherByUsername]", par, commandType: CommandType.StoredProcedure);

            return Result<User>.Succeed(user);
        }
        catch(Exception ex)
        {
            return Result<User>.Fail(500, ex.Message);
        }
        
    }

    public async Task<Result> RegisterTeacher(User tutor)
    {
        try
        {
            using IDbConnection connection = new SqlConnection(_connectionString);
            var par = new DynamicParameters();
            par.Add("@username", tutor.Username);
            par.Add("@subject", 0);
            par.Add("@password_hash", tutor.PasswordHash);
            par.Add("@password_salt", tutor.PasswordSalt);

            await connection.ExecuteAsync("[dbo].[RegisterTeacher]", par, commandType: CommandType.StoredProcedure);
            return Result.Succeed();

        }
        catch (Exception e)
        {
            return Result.Fail(500, e.Message);
        }
        
    }
}
