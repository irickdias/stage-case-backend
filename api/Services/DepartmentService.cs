using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using api.Models.Dtos.Department;
using api.Models.Dtos.Sector;
using api.Models.Mappers;

namespace api.Services
{
    public class DepartmentService
    {
        private readonly ApplicationDBContext _context;

        public DepartmentService(ApplicationDBContext context)
        {
            _context = context;
        }

        public List<DepartmentWithIdDto> GetAllDepartments() {
            var departments = _context.Departments.Select(d => d.ToDepartmentWithIdDto()).ToList();

            return departments;
        }

        public List<DepartmentAndSectorsDto> GetAllDepartmentsAndSectors() {
            var ds = _context.Departments.Select(d => new DepartmentAndSectorsDto {
                id = d.id,
                name = d.name,
                sectors = d.sectors.Select(s => new SectorWithIdDto {
                    id = s.id,
                    name = s.name,
                    departmentId = s.departmentId
                }).ToList()
            }).ToList();

            return ds;
        }

        public Department? GetDepartmentById(int id) {
            var department = _context.Departments.Find(id);

            return department;
        }

        public Department CreateDepartment(DepartmentDto departmentDto) {
            var departmentModel = departmentDto.ToDepartmentFromCreateDto();
            _context.Departments.Add(departmentModel);
            _context.SaveChanges(); // commit

            return departmentModel;
        }

        public DepartmentDto? UpdateDepartment(int id, UpdateDepartmentDto updateDto) {
            var departmentModel = _context.Departments.FirstOrDefault(d => d.id == id);

            if(departmentModel == null)
                return null;
            
            departmentModel.name = updateDto.name;

            _context.SaveChanges();

            return departmentModel.ToDepartmentDto();
        }
    }
}