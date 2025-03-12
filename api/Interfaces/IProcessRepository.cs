using api.Helpers;
using api.Models;
using api.Models.Dtos.Process;

namespace api.Interfaces
{
    public interface IProcessRepository
    {
        Task<List<ProcessDto>> GetAll();
        Task<Process?> GetById(int id);
        Task<PaginatedList<HierarchyProcessDto>> GetProcessesHierarchy(QueryObject query);
        Task<Process> Create(ProcessDto processDto);
        Task<ProcessDto?> Update(int id, UpdateProcessDto updateDto);
        Task<Process?> Delete(int id);
        Task<Process?> FinishProcess(int id);
    }
}
