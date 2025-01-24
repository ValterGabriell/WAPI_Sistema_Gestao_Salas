using WAPI_GS.Dto.User;
using WAPI_GS.Modelos;

namespace WAPI_GS.EM.User
{
    public static class UserEM
    {
        public static TblUser ToEntity(this DtoCreateUser dto)
        {
            return new TblUser
            {
                Name = dto.Name,
                MobilePhone = dto.MobilePhone,
                Email = dto.Email,
                Username = dto.Username,
                Password = dto.Password,
                IsActive = true
            };
        }

        public static DtoGetUser ToDto(this TblUser entity)
        {
            return new DtoGetUser
            {
                Id = entity.Id,
                IsActive = entity.IsActive,
                CreationDate = entity.CreationDate,
                LastLogin = entity.LastLogin,
                Name = entity.Name,
                MobilePhone = entity.MobilePhone,
                Email = entity.Email,
                Username = entity.Username,
                Password = entity.Password,
            };
        }

        public static TblUser ToEntity(this DtoGetUser dto)
        {
            return new TblUser
            {
                Id = dto.Id,
                IsActive = dto.IsActive,
                CreationDate = dto.CreationDate,
                LastLogin = dto.LastLogin,
                Name = dto.Name,
                MobilePhone = dto.MobilePhone,
                Email = dto.Email,
                Username = dto.Username,
                Password = dto.Password,
            };
        }
    }
}

