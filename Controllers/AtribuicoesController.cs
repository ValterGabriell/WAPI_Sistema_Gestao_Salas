using Microsoft.AspNetCore.Mvc;
using WAPI_GS.Dto.UserSala;
using WAPI_GS.Interfaces;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Controllers
{
    [ApiController]
    [Route("api/v1/atribuicoes")]
    public class AtribuicoesController(IUnitOfWork uow) : ControllerBase
    {
        private readonly IUnitOfWork _uow = uow;

        [HttpPost]
        public async Task<ActionResult<string>> AtribuirProfessorASala(DtoAtribuirProfessorASala dto)
        {
            try
            {
                var result = await _uow.AtribuicaoService.AtribuirProfessorASala(dto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public class EmailSolicitacaoDto
        {
            public string DestEmail { get; set; }
            public string Body { get; set; }
            public string Title { get; set; }
            public string FullUrl { get; set; }
            public int SalaId { get; set; }
            public DateOnly Dia { get; set; }
            public int AntigoUsuarioID { get; set; }
            public int NovoUsuarioID { get; set; }
            public int HoraInit { get; set; }
            public int HoraFinal { get; set; }
        }
        [HttpPost("solicitar-troca")]
        public async Task<ActionResult<bool>> SolicitarTrocaHorario(EmailSolicitacaoDto emailSolicitacaoDto)
        {
            try
            {
                var result = await _uow.AtribuicaoService.SendEmailSolicitacao(
                    emailSolicitacaoDto.DestEmail,
                    emailSolicitacaoDto.Body,
                    emailSolicitacaoDto.Title,
                    emailSolicitacaoDto.FullUrl,
                    emailSolicitacaoDto.SalaId,
                    emailSolicitacaoDto.Dia,
                    emailSolicitacaoDto.AntigoUsuarioID,
                    emailSolicitacaoDto.NovoUsuarioID,
                    emailSolicitacaoDto.HoraInit,
                    emailSolicitacaoDto.HoraFinal
                    );

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("accept")]
        public async Task<IActionResult> Accept(
        int salaId, DateOnly dia, int antigoUsuarioID,
             int novoUsuarioID, int horaInit, int horaFinal)
        {
            try
            {
                var result = await _uow.AtribuicaoService.Accept(
                    salaId, dia, antigoUsuarioID, novoUsuarioID, horaInit, horaFinal
                    );

                string html = @"
        <html>
        <head>
            <meta charset='UTF-8'>
            <title>Reserva Aceita</title>
            <style>
                body {
                    background-color: #f4f6f8;
                    font-family: 'Segoe UI', Arial, sans-serif;
                    margin: 0;
                    padding: 0;
                }
                .container {
                    max-width: 500px;
                    margin: 60px auto;
                    background: #fff;
                    border-radius: 12px;
                    box-shadow: 0 4px 24px rgba(0,0,0,0.08);
                    padding: 32px 24px;
                    text-align: center;
                }
                .icon {
                    font-size: 48px;
                    color: #28a745;
                    margin-bottom: 16px;
                }
                h2 {
                    color: #222;
                    margin-bottom: 12px;
                }
                p {
                    color: #555;
                    font-size: 18px;
                    margin-bottom: 24px;
                }
                .footer {
                    font-size: 13px;
                    color: #888;
                    margin-top: 24px;
                }
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='icon'>✔️</div>
                <h2>Reserva aceita com sucesso!</h2>
                <p>Um e-mail será enviado ao professor informando sobre a confirmação da reserva.</p>
                <div class='footer'>Gestão de Salas &copy; 2025</div>
            </div>
        </body>
        </html>";
                return Content(html, "text/html");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("notAccept")]
        public async Task<IActionResult> NotAccept(
       int salaId, int usuarioQueSolicitou)
        {
            try
            {
                var result = await _uow.AtribuicaoService.NotAccept(
                    salaId, usuarioQueSolicitou
                    );

                string html = @"
        <html>
        <head>
            <meta charset='UTF-8'>
            <title>Reserva Recusada</title>
            <style>
                body {
                    background-color: #f4f6f8;
                    font-family: 'Segoe UI', Arial, sans-serif;
                    margin: 0;
                    padding: 0;
                }
                .container {
                    max-width: 500px;
                    margin: 60px auto;
                    background: #fff;
                    border-radius: 12px;
                    box-shadow: 0 4px 24px rgba(0,0,0,0.08);
                    padding: 32px 24px;
                    text-align: center;
                }
                .icon {
                    font-size: 48px;
                    color: #dc3545;
                    margin-bottom: 16px;
                }
                h2 {
                    color: #222;
                    margin-bottom: 12px;
                }
                p {
                    color: #555;
                    font-size: 18px;
                    margin-bottom: 24px;
                }
                .footer {
                    font-size: 13px;
                    color: #888;
                    margin-top: 24px;
                }
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='icon'>❌</div>
                <h2>Reserva recusada!</h2>
                <p>Um e-mail será enviado ao professor informando sobre a recusa da reserva.</p>
                <div class='footer'>Gestão de Salas &copy; 2025</div>
            </div>
        </body>
        </html>";
                return Content(html, "text/html");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete]
        public async Task RemoverAtribuicaoProfessorSala([FromQuery] int userId, [FromQuery] int salaId, [FromQuery] string turmaID, [FromQuery] DateOnly dia)
        {
            await _uow.AtribuicaoService.RemoverAtribuicaoProfessorSala(userId, salaId, turmaID, dia);
        }

        [HttpDelete("todos")]
        public async Task RemoverTodasAtribuicaoProfessorSala([FromQuery] int userId, [FromQuery] int salaId, [FromQuery] string turmaID)
        {
            await _uow.AtribuicaoService.RemoverTodasAtribuicaoProfessorSala(userId, salaId, turmaID);
        }


        [HttpGet]
        public async Task<ActionResult<PagedList<DtoGetUserSala>>> GetList()
        {
            try
            {
                var result = await _uow.AtribuicaoService.GetList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<string>> AtualizarAtribuicaoProfessorASala(
            DtoAtualizarAtribuicaoProfessorSala dto, [FromQuery] int salaId, [FromQuery] int oldUserId)
        {
            try
            {
                var result = await _uow.AtribuicaoService.AtualizarAtribuicaoProfessorASala(dto, oldUserId, salaId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
