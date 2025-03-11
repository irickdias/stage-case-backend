using api.Data;
using api.Helpers;
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

        public async Task<Process?> Delete(int id)
        {
            var processModel = await _context.Processes.FirstOrDefaultAsync(p => p.id == id);

            if (processModel == null)
                return null;

            /*var sql = @"
                WITH ProcessHierarchy AS (
                    SELECT id FROM Processes WHERE id = {0}
                    UNION ALL
                    SELECT p.id FROM Processes p
                    INNER JOIN ProcessHierarchy ph ON p.parentProcessId = ph.id
                )
                SELECT id FROM ProcessHierarchy;
            ";

            var processIds = await _context.Processes
                .FromSqlRaw(sql, id)
                .Select(p => p.id)
                .ToListAsync();

            await _context.Processes
                .Where(p => processIds.Contains(p.id))
                .ExecuteDeleteAsync();

            await _context.SaveChangesAsync();*/

            var sql = @"
                WITH ProcessHierarchy AS (
                    SELECT id FROM Processes WHERE id = {0}
                    UNION ALL
                    SELECT p.id FROM Processes p
                    INNER JOIN ProcessHierarchy ph ON p.parentProcessId = ph.id
                )
                DELETE FROM Processes WHERE id IN (SELECT id FROM ProcessHierarchy);
            ";

            await _context.Database.ExecuteSqlRawAsync(sql, id);

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

        public async Task<List<HierarchyProcessDto>> GetProcessesHierarchy(QueryObject query)
        {
            // var processes = await _context.Processes.Select(p => p.ToHierarchyProcessDto()).ToListAsync();
            var processesQuery = _context.Processes
                .Join(_context.Sectors,
                  process => process.sectorId,
                  sector => sector.id,
                  (process, sector) => new { process, sector })
                .Join(_context.Departments,
                  ps => ps.sector.departmentId,
                  department => department.id,
                  (ps, department) => new {ps.process, sectorName = ps.sector.name, departmentName = department.name})
                .AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.search))
            {
                processesQuery = processesQuery.Where(p => p.process.name.Contains(query.search));
            }

            if (query.department != null)
            {
                processesQuery = processesQuery.Where(p => p.process.sector != null && p.process.sector.departmentId == query.department);
            }

            if (query.sector != null)
            {
                processesQuery = processesQuery.Where(p => p.process.sectorId == query.sector);
            }

            //var skipNumber = (query.pageNumber - 1) * query.pageSize;

            var processes = await processesQuery
                .Select(p => p.process.ToHierarchyProcessDto(p.departmentName, p.sectorName, null))
                //.Skip(skipNumber)
                //.Take(query.pageNumber)
                .ToListAsync();

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

            foreach(var process in processMap.Values)
            {
                var count = process.children.Count();
                process.finishedSubprocesses = count > 0 ? process.children.Count(children => children.finished) : null;
            }

            var skipNumber = (query.pageNumber - 1) * query.pageSize;

            return hierarchy.Skip(skipNumber).Take(query.pageSize).ToList();
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

        private async Task<List<int>> GetAllSubProcessesAsync(int parentId)
        {
            // consulta por todos os subprocessos (traz apenas os ids) existentes com o id do pai
            var subProcesses = await _context.Processes
                .Where(p => p.parentProcessId == parentId)
                .Select(p => p.id)
                .ToListAsync();

            var allSubProcessIds = new List<int>(subProcesses);

            // para cada subprocesso encontrado, ele vai entrar de novo na função recursivamente, procurando a existencia de nós filhos
            foreach (var subProcessId in subProcesses)
            {
                // quando retornado do heap, adiciona todos os ids encontrados na lista de is para excluir
                allSubProcessIds.AddRange(await GetAllSubProcessesAsync(subProcessId));
            }

            return allSubProcessIds;
        }
    }
}
