using CourseManagementProject.Application.Repositories;
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


    public async Task<User> GetTeacherByUsername(string username)
    {
        using IDbConnection connection = new SqlConnection(_connectionString);

        var par = new DynamicParameters();
        par.Add("@username", username);

        var user = await connection.QueryFirstOrDefaultAsync<Tutor>("[dbo].[GetTeacherByUsername]", par, commandType: CommandType.StoredProcedure);

        return user;
    }

    public async Task RegisterTeacher(User user)
    {
        using IDbConnection connection = new SqlConnection(_connectionString);
        var par = new DynamicParameters();
        par.Add("@username", user.Username);
        par.Add("@subject", 0);
        par.Add("@password_hash", user.PasswordHash);
        par.Add("@password_salt", user.PasswordSalt);

        await connection.ExecuteAsync("[dbo].[RegisterTeacher]", par, commandType: CommandType.StoredProcedure);
    }
}
