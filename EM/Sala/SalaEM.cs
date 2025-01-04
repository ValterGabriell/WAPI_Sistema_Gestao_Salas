using WAPI_GS.Dto.Sala;
using WAPI_GS.Modelos;

namespace WAPI_GS.EM.Sala
{
    public static class SalaEM
    {
        public static TblSala ToEntity(this DtoCreateSala dto)
        {
            return new TblSala
            {
                IsActive = true,
                CreationDate = DateTime.UtcNow,
                Name = dto.Name
            };
        }

        public static DtoGetSala ToDto(this TblSala entity)
        {
            return new DtoGetSala
            {
                Id = entity.Id,
                IsActive = entity.IsActive,
                CreationDate = entity.CreationDate,
                Name = entity.Name,
                UserSalas = entity.UserSalas,
            };
        }
    }
}
