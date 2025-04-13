using System.ComponentModel.DataAnnotations;

namespace WAPI_GS.Dto
{
    public class DtoLoginModel
    {

        [Required]
        public string UserName { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";
    }
}
