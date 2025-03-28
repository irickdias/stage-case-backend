﻿using api.Models;
using api.Models.Dtos.Sector;

namespace api.Interfaces
{
    public interface ISectorRepository
    {
        Task<List<SectorWithIdDto>> GetAll(int? id);
        Task<Sector?> GetById(int id);
        Task<Sector> Create(SectorDto sectorDto);
        Task<SectorDto?> Update(int id, UpdateSectorDto updateDto);
        Task<Sector?> Delete(int id);
    }
}
