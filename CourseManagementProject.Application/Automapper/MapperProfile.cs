using AutoMapper;
using CourseManagementProject.Domain.DomainModels;
using CourseManagementProject.Domain.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagementProject.Application.Automapper
{
    internal class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterModel, User>();

        }
    }
}
