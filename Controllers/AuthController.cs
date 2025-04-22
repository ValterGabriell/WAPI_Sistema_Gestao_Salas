using Microsoft.AspNetCore.Mvc;
using WAPI_GS.Dto;
using WAPI_GS.Interfaces;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController(IUnitOfWork uow) : ControllerBase
    {
        private readonly IUnitOfWork _uow = uow;

        [HttpPost]
        public async Task<ActionResult<string>> Login(DtoLoginModel dto)
        {
            try
            {
                var result = await _uow.AuthService.Login(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
            }
        }


        [HttpPatch("alterar-senha")]
        public async Task<ActionResult<bool>> EsqueciSenha(string username, string novaSenha)
        {
            try
            {
                var result = await _uow.AuthService.EsqueciSenha(username, novaSenha);
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
