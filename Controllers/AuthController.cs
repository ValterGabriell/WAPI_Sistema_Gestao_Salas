using Microsoft.AspNetCore.Mvc;
using WAPI_GS.Dto;
using WAPI_GS.Interfaces;

namespace WAPI_GS.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController(ICS_UnitOfWork uow) : ControllerBase
    {
        private readonly ICS_UnitOfWork _uow = uow;

        [HttpPost]
        public async Task<ActionResult<string>> Login(DtoLogin dto)
        {
            try
            {
                var result = await _uow.AuthRepository.Login(dto.Username, dto.Password, dto.IsAdmin);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        [HttpGet]
        public ActionResult<string> OK()
        {
            return Ok("OK");
        }
    }
}
