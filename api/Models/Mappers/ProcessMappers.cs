using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.Dtos.Process;

namespace api.Models.Mappers
{
    public static class ProcessMappers
    {
        public static ProcessDto ToProcessDto(this Process processModel) {
            return new ProcessDto {
                name = processModel.name,
                tools = processModel.tools,
                responsibles = processModel.responsibles,
                documentation = processModel.documentation,
                priority = processModel.priority,
                finished = processModel.finished,
                createdOn = processModel.createdOn,
                sectorId = processModel.sectorId,
                parentProcessId = processModel.parentProcessId
            };
        }

        public static HierarchyProcessDto ToHierarchyProcessDto(this Process processModel, string departmentName = "", string sectorName = "", decimal? progress = 0) {
            return new HierarchyProcessDto {
                id = processModel.id,
                name = processModel.name,
                tools = processModel.tools,
                responsibles = processModel.responsibles,
                documentation = processModel.documentation,
                priority = processModel.priority,
                finished = processModel.finished,
                createdOn = processModel.createdOn,
                sectorId = processModel.sectorId,
                departmentName = departmentName,
                progress = progress,
                sectorName = sectorName,
                parentProcessId = processModel.parentProcessId
            };
        }

        public static Process ToProcessFromCreateDto(this ProcessDto processDto) {
            return new Process {
                name = processDto.name,
                tools = processDto.tools,
                responsibles = processDto.responsibles,
                documentation = processDto.documentation,
                priority = processDto.priority,
                finished = processDto.finished,
                createdOn = processDto.createdOn,
                sectorId = processDto.sectorId,
                parentProcessId = processDto.parentProcessId
            };
        }
    }
}