using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using api.Models.Dtos.Process;
using api.Models.Mappers;

namespace api.Services
{
    public class ProcessService
    {
        private readonly ApplicationDBContext _context;

        public ProcessService(ApplicationDBContext context)
        {
            _context = context;
        }

        public List<ProcessDto> GetAllProcesses() {
            var processes = _context.Processes.Select(p => p.ToProcessDto()).ToList();
            
            return processes;
        }

        public Process? GetProcessById(int id) {
            var process = _context.Processes.Find(id);

            return process;
        }

        public List<HierarchyProcessDto> GetProcessesHierarchy() {
            var processes = _context.Processes.Select(p => p.ToHierarchyProcessDto()).ToList();
            var processMap = processes.ToDictionary(p => p.id, p => p);

            //System.Diagnostics.Debug.WriteLine(processMap);

            var hierarchy = new List<HierarchyProcessDto>();

            foreach(var process in processes) {
                if(process.parentProcessId.HasValue) {
                    if(processMap.TryGetValue(process.parentProcessId.Value, out var parent))
                        parent.children.Add(process);
                }
                else
                    hierarchy.Add(process);
            }

            return hierarchy;
        }

        public Process CreateProcess(ProcessDto processDto) {
            var processModel = processDto.ToProcessFromCreateDto();
            _context.Processes.Add(processModel);
            _context.SaveChanges(); // commit

            return processModel;
        }

        public ProcessDto? UpdateProcess(int id, UpdateProcessDto updateDto) {
            var processModel = _context.Processes.FirstOrDefault(p => p.id == id);

            if (processModel == null)
                return null;

            processModel.name = updateDto.name;
            processModel.tools = updateDto.tools;
            processModel.responsibles = updateDto.responsibles;
            processModel.documentation = updateDto.documentation;
            processModel.priority = updateDto.priority;
            processModel.finished = updateDto.finished;
            processModel.createdOn = updateDto.createdOn;
            processModel.sectorId = updateDto.sectorId;
            processModel.parentProcessId = updateDto.parentProcessId;

            _context.SaveChanges();

            return processModel.ToProcessDto();
        }
    }
}