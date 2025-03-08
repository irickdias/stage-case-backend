using api.Data;
using api.Interfaces;
using api.Models;
using api.Models.Dtos.Process;
using api.Models.Mappers;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class ProcessRepository : IProcessRepository
    {
        private readonly ApplicationDBContext _context;

        public ProcessRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Process> Create(ProcessDto processDto)
        {
            var processModel = processDto.ToProcessFromCreateDto();
            await _context.Processes.AddAsync(processModel);
            await _context.SaveChangesAsync();

            return processModel;
        }

        public async Task<List<ProcessDto>> GetAll()
        {
            return await _context.Processes.Select(p => p.ToProcessDto()).ToListAsync();
        }

        public async Task<Process?> GetById(int id)
        {
            return await _context.Processes.FindAsync(id);
        }

        public async Task<List<HierarchyProcessDto>> GetProcessesHierarchy()
        {
            var processes = await _context.Processes.Select(p => p.ToHierarchyProcessDto()).ToListAsync();
            var processMap = processes.ToDictionary(p => p.id, p => p);

            var hierarchy = new List<HierarchyProcessDto>();

            foreach (var process in processes)
            {
                if (process.parentProcessId.HasValue)
                {
                    if (processMap.TryGetValue(process.parentProcessId.Value, out var parent))
                        parent.children.Add(process);
                }
                else
                    hierarchy.Add(process);
            }

            return hierarchy;
        }

        public async Task<ProcessDto?> Update(int id, UpdateProcessDto updateDto)
        {
            var processModel = await _context.Processes.FirstOrDefaultAsync(p => p.id == id);

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

            await _context.SaveChangesAsync();

            return processModel.ToProcessDto();
        }
    }
}
