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

        public static DepartmentWithIdDto ToDepartmentWithIdDto(this Department departmentModel) {
            return new DepartmentWithIdDto {
                id = departmentModel.id,
                name = departmentModel.name
            };
        }

        // public static DepartmentAndSectorsDto ToDepartmentAndSectorsDto(this Department departmentModel) {
        //     return new DepartmentAndSectorsDto {
        //         id = departmentModel.id,
        //         name = departmentModel.name,
        //         sectors = departmentModel.sectors
        //         .Select(s => new SectorWithoutDepartmentIdDto
        //         {
        //             id = s.id,
        //             name = s.name
        //         })
        //         .ToList()
        //     };
        // }

        public static Department ToDepartmentFromCreateDto(this DepartmentDto departmentDto) {
            return new Department {
                name = departmentDto.name
            };
        }
    }
}