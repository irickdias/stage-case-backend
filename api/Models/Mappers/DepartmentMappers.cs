using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.Dtos.Department;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace api.Models.Mappers
{
    public static class DepartmentMappers
    {
        public static DepartmentDto ToDepartmentDto(this Department departmentModel) {
            return new DepartmentDto {
                name = departmentModel.name
            };
        }

        public static Department ToDepartmentFromCreateDto(this DepartmentDto departmentDto) {
            return new Department {
                name = departmentDto.name
            };
        }
    }
}