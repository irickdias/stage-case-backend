using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.Dtos.Sector;

namespace api.Models.Mappers
{
    public static class SectorMappers
    {
        public static SectorDto ToSectorDto(this Sector sectorModel) {
            return new SectorDto {
                name = sectorModel.name,
                departmentId = sectorModel.departmentId
            };
        }

        public static SectorWithIdDto ToSectorWithIdDto(this Sector sectorModel) {
            return new SectorWithIdDto {
                id = sectorModel.id,
                name = sectorModel.name,
                departmentId = sectorModel.departmentId
            };
        }

        public static Sector ToSectorFromCreateDto(this SectorDto sectorDto) {
            return new Sector {
                name = sectorDto.name,
                departmentId = sectorDto.departmentId
            };
        }
    }
}