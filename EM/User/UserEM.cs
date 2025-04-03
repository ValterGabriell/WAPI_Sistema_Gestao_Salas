using WAPI_GS.Dto.User;
using WAPI_GS.Modelos;

namespace WAPI_GS.EM.User
{
    public static class UserEM
    {
        public static TblProfessor ToEntity(this DtoCreateUpdateUser dto)
        {
            return new TblProfessor
            {
                Name = dto.Name,
                MobilePhone = dto.MobilePhone,
                Email = dto.Email,
                Username = dto.Username,
                Password = dto.Password,
                IsActive = true
            };
        }

        public static DtoGetProfessor ToDto(this TblProfessor entity)
        {
            return new DtoGetProfessor
            {
                Id = entity.Id,
                IsActive = entity.IsActive,
                CreationDate = entity.CreationDate,
                LastLogin = entity.LastLogin,
                Name = entity.Name,
                MobilePhone = entity.MobilePhone,
                Email = entity.Email,
                Username = entity.Username,
                Color = entity.Color,
            };
        }

        public static TblProfessor ToEntity(this DtoGetProfessor dto)
        {
            return new TblProfessor
            {
                Id = dto.Id,
                IsActive = dto.IsActive,
                CreationDate = dto.CreationDate,
                LastLogin = dto.LastLogin,
                Name = dto.Name,
                MobilePhone = dto.MobilePhone,
                Email = dto.Email,
                Username = dto.Username,
                //Password = dto.Password,
            };
        }
    }
}

