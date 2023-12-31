﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagementProject.Domain
{
    public class Result
    {
        public bool Success { get; set; }       
        public string? ErrorMessage { get; set; }
        public int StatusCode { get; set; }

        public static Result Succeed()
        {
            return new Result { Success = true };
        }

        public static Result Fail(int statusCode, string message = null)
        {
            return new Result { Success = false, ErrorMessage = message, StatusCode = statusCode };
        }


    }


    public class Result<T>
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set;}
        public int StatusCode { get; set; }
        public T? Data { get; set; }

        public static Result<T> Succeed(T data)
        {
            return new Result<T> { Success = true, Data = data };
        }

        public static Result<T> Fail(int statusCode, string? message = null)
        {
            return new Result<T>() { Success = false, ErrorMessage = message, StatusCode = statusCode };
        }
    }
}
