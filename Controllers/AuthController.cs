namespace WAPI_GS.Controllers
{
    //[ApiController]
    //[Route("api/v1/auth")]
    //public class AuthController(IUnitOfWork uow) : ControllerBase
    //{
    //    private readonly IUnitOfWork _uow = uow;

    //    [HttpPost]
    //    public async Task<ActionResult<string>> Login(DtoLoginModel dto)
    //    {
    //        try
    //        {
    //            var result = await _uow.AuthRepository.Login(dto.Username, dto.Password, dto.IsAdmin);
    //            return Ok(result);
    //        }
    //        catch (Exception ex)
    //        {
    //            return BadRequest(HelperExceptions.CreateExceptionMessage(ex));
    //        }
    //    }

    //    [HttpGet]
    //    public ActionResult<string> OK()
    //    {
    //        return Ok("OK");
    //    }
    //}
}
