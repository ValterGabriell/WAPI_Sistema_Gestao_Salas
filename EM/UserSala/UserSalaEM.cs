using WAPI_GS.Dto.UserSala;
using WAPI_GS.Modelos;

namespace WAPI_GS.EM.UserSala
{
    public static class UserSalaEM
    {
        public static TblUsersSala ToEntity(this DtoCreateUserSala dto)
        {
            return new TblUsersSala
            {
                UserId = dto.UserId,
                SalaId = dto.SalaId,
                Dia = dto.Dia
            };
        }

        //public static DtoGetUserSala ToDto(this TblUsersSala entity)
        //{
        //    return new DtoGetUserSala
        //    {
        //        UserId = entity.UserId,
        //        SalaId = entity.SalaId,
        //        Dia = entity.Dia,
        //        TblSala = entity.TblSala,
        //        TblUser = entity.TblUser
        //    };
        //}
    }
}
