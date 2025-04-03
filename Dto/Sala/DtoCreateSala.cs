using WAPI_GS.Modelos;

namespace WAPI_GS.Dto.Sala
{
    public class DtoCreateSala
    {
        public string? Name { get; set; }
        public TblSala ToEntityForUpdate(bool isActive)
        {
            return new TblSala
            {
                IsActive = isActive,
                Name = this.Name,
                CreationDate = DateTime.Now,
            };
        }

        public TblSala ToEntity()
        {
            return new TblSala
            {
                IsActive = true,
                Name = this.Name,
                CreationDate = DateTime.Now,
            };
        }
    }
}
