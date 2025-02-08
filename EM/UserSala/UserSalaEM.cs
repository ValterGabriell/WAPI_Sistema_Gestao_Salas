using WAPI_GS.Dto.UserSala;
using WAPI_GS.Modelos;

namespace WAPI_GS.EM.UserSala
{
    public static class UserSalaEM
    {
        public static TblPtd ToEntity(this DtoCreateUserSala dto)
        {
            return new TblPtd
            {
                UserId = dto.UserId,
                SalaId = dto.SalaId,
                DisciplinaId = dto.DisciplinaId,
                Dia = dto.Dia,
                HoraFinal = dto.HoraFinal,
                HoraInicial = dto.HoraInicial
            };
        }

        public static DtoGetUserSala ToDto(this TblPtd entity)
        {
            return new DtoGetUserSala
            {
                Dia = entity.Dia
            };
        }
    }
}
